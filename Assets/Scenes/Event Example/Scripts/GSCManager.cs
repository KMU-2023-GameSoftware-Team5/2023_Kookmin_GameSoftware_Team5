using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GSC
{
	public class GSCManager : MonoBehaviour
	{
		[Serializable]
		public class ScriptCallback
		{
			public string name;
			public UnityEvent onCall;
		}

		[SerializeField] TMP_Text m_textBox;
		[SerializeField] TextAsset m_GSCScript;
		[SerializeField] GameObject m_buttonPrefab;
		[SerializeField] LayoutGroup m_buttonLayout;

		[SerializeField] string m_startNode;
		[SerializeField] char m_prefix;

		public List<ScriptCallback> callbacks;

		const string NamePattern = "[a-zA-Z_][a-zA-Z_0-9]*";

		readonly Queue<GameObject> m_buttonObjectPool = new();
		readonly Dictionary<string, int> m_nodeLineDict = new();

		string[] m_scriptLines;
		int m_lineIndex;

		bool m_buttonPressed = false;

		void Start()
		{
			string scriptText = m_GSCScript.text.Replace("\r", "");
			m_scriptLines = scriptText.Split("\n");

			string nowNode = string.Empty;
			bool ifStateEnded = true;

			// Parse script nodes and check if-waitif
			for (int i = 0; i < m_scriptLines.Length; i++)
			{
				string line = m_scriptLines[i].Trim();

				if (Regex.IsMatch(line, $"^{m_prefix}if \".*\" -> {NamePattern}$"))
					ifStateEnded = false;
				else if (Regex.IsMatch(line, $"^{m_prefix}waitif$"))
				{
					// If statement has already ended, but multiple waitif detected.
					if (ifStateEnded)
					{
						Debug.LogError($"(GSC)Multiple 'waitif' detected: line {i + 1}");
						return;
					}

					ifStateEnded = true;
				}
				else if (Regex.IsMatch(line, $@"^{m_prefix}node {NamePattern}$"))
				{
					// Add node to dictionary
					string nodeName = line.Split()[1];

					if (m_nodeLineDict.ContainsKey(nodeName))
					{
						Debug.LogError($"(GSC)Multiple node name definition: line {i + 1}");
						return;
					}

					if (!ifStateEnded)
					{
						Debug.LogError($"(GSC)'if' must end with 'waitif': node {nowNode}");
						return;
					}

					m_nodeLineDict.Add(nodeName, i);
					nowNode = nodeName;
				}
			}

			m_textBox.text = string.Empty;

			StartCoroutine(StartScript());
		}

		IEnumerator StartScript()
		{
			if (!m_nodeLineDict.TryGetValue(m_startNode, out m_lineIndex))
			{
				Debug.LogError($"(GSC)Invalid start node name: ");
				yield break;
			}

			string nowNode = m_startNode;

			Debug.Log($"(GSC)Start script: node {nowNode}, line {m_lineIndex + 1}");

			while (m_lineIndex < m_scriptLines.Length)
			{
				string rawLine = m_scriptLines[m_lineIndex++];
				string line = rawLine.Trim();

				if (line == string.Empty)
					continue;

				// Prefix check: Is not a command line?
				if (!Regex.IsMatch(line, $"{m_prefix}.*"))
				{
					m_textBox.text += rawLine;
					continue;
				}

				// Remove prefix from command
				line = line.TrimStart(m_prefix);

				// Check command regexes
				if (Regex.IsMatch(line, $"^goto {NamePattern}$"))
				{
					string nextNode = line.Split()[1];

					if (!m_nodeLineDict.TryGetValue(nextNode, out int nextNodeIndex))
					{
						Debug.LogError($"(GSC)Invalid node name: {nextNode} line {m_lineIndex}");
						break;
					}

					nowNode = GotoNode(nextNodeIndex);
				}
				else if (Regex.IsMatch(line, $"^if \".*\" -> {NamePattern}$"))
				{
					string buttonText = Regex.Match(line, "(\".*\")").Groups[1].Value.Trim('"');
					string nextNode = line.Split("-> ")[1];

					if (!m_nodeLineDict.TryGetValue(nextNode, out int nextNodeIndex))
					{
						Debug.LogError($"(GSC)Invalid node name: {nextNode} line {m_lineIndex}");
						break;
					}

					// Instantiate and set button as children of button layout, or dequeue
					if (!m_buttonObjectPool.TryDequeue(out GameObject buttonObject))
						buttonObject = Instantiate(m_buttonPrefab, m_buttonLayout.transform);

					buttonObject.transform.SetParent(m_buttonLayout.transform);
					buttonObject.SetActive(true);

					// Set button onClick events
					Button buttonComponent = buttonObject.GetComponent<Button>();

					buttonComponent.onClick.RemoveAllListeners();
					buttonComponent.onClick.AddListener(() =>
					{
						nowNode = GotoNode(nextNodeIndex);
						m_buttonPressed = true;
					});

					buttonObject.GetComponentInChildren<TMP_Text>().text = buttonText;
				}
				else if (Regex.IsMatch(line, "^waitif"))
				{
					if (m_buttonLayout.GetComponentsInChildren<Button>().Length == 0)
					{
						Debug.LogError($"(GSC)No active if found: line {m_lineIndex}");
						break;
					}

					yield return new WaitUntil(() => m_buttonPressed);
					m_buttonPressed = false;
				}
				else if (Regex.IsMatch(line, $"^call {NamePattern}$"))
				{
					string callbackName = line.Split()[1];
					bool callbackNotFound = true;

					foreach (var callback in callbacks)
					{
						if (callback.name == callbackName)
						{
							callback.onCall.Invoke();
							callbackNotFound = false;
							break;
						}
					}

					if (callbackNotFound)
					{
						Debug.LogError($"(GSC)Invalid callback name: {callbackName}");
						break;
					}
				}
				else if (Regex.IsMatch(line, $"^scene \".*\"$"))
				{
					string scenePath = line[(line.IndexOf(' ') + 1)..].Trim('"');

					if (SceneUtility.GetBuildIndexByScenePath(scenePath + "") != -1)
						SceneManager.LoadScene(scenePath);
					else
						Debug.LogError($"(GSC)Invalid scene path: {scenePath}");

					break;
				}
				else if (Regex.IsMatch(line, $"^node {NamePattern}$"))
				{
					string nodeName = line.Split()[1];

					if (nodeName != nowNode)
						break;
				}
				else
				{
					Debug.LogError($"Invalid command line: \"{rawLine}\"");
					break;
				}
			}

			Debug.Log($"(GSC)End script: node {nowNode}, line {m_lineIndex}");
		}

		// Detach all buttons from button layout with disable it
		// And enqueue to the button object pool
		void ClearButtonLayout()
		{
			foreach (var childButton in m_buttonLayout.GetComponentsInChildren<Button>())
			{
				var childObject = childButton.gameObject;

				childObject.SetActive(false);
				childObject.transform.SetParent(null);
				m_buttonObjectPool.Enqueue(childObject);
			}
		}

		// Goto the node in line nodeLineIndex and return that node name.
		string GotoNode(int nodeLineIndex)
		{
			m_textBox.text = string.Empty;

			ClearButtonLayout();

			m_lineIndex = nodeLineIndex;

			return m_scriptLines[m_lineIndex].Split()[1];
		}
	}
}
