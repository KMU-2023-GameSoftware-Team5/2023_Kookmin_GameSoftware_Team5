using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScriptManager : MonoBehaviour
{
	[Serializable]
	public class ScriptCallback
	{
		public string name;
		public UnityEvent callback;
	}

	public TMP_Text m_textBox;
	public TextAsset m_scriptFile;
	public ScriptCallback[] m_callbacks;
	public GameObject m_buttonPrefab;
	public LayoutGroup m_buttonLayout;

	public string m_startNode;
	public char m_prefix;

	static readonly string NamePattern = "[a-zA-Z_][a-zA-Z_0-9]*";

	readonly Queue<GameObject> m_buttonObjectPool = new();

	readonly Dictionary<string, UnityEvent> m_callbackDict = new();
	readonly Dictionary<string, int> m_nodeLineDict = new();

	string[] m_scriptLines;
	int m_lineIndex = 0;

	bool m_buttonPressed = false;

	void Start()
	{
		string scriptText = m_scriptFile.text.Replace("\r", "");
		m_scriptLines = scriptText.Split("\n");

		// Save callback name and callback as dictionary
		foreach (var scriptCallback in m_callbacks)
			m_callbackDict.Add(scriptCallback.name, scriptCallback.callback);

		// Parse script nodes
		for (int i = 0; i < m_scriptLines.Length; i++)
		{
			string line = m_scriptLines[i];

			if (Regex.IsMatch(line, $@"^{m_prefix}node {NamePattern}$"))
			{
				var spliited = line.Split(' ');

				if (spliited.Length < 2)
				{
					Debug.LogError($"(GSC)Invalid node syntax: line {i + 1}");
					return;
				}

				if (m_nodeLineDict.ContainsKey(spliited[1]))
				{
					Debug.LogError($"(GSC)Multiple node name definition: line {i + 1}");
					return;
				}

				m_nodeLineDict.Add(spliited[1], i);
			}
		}

		// Clear the textbox
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

		while (m_lineIndex < m_scriptLines.Length)
		{
			string rawLine = m_scriptLines[m_lineIndex++];
			string line = rawLine.Trim();

			if (line == string.Empty)
				continue;

			// Prefix check
			if (!Regex.IsMatch(line, $"{m_prefix}.*"))
			{
				m_textBox.text += rawLine;
				continue;
			}

			line = line.TrimStart(m_prefix);

			// Check command regex
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

				// Instantiate and set button as children of button layout
				if (!m_buttonObjectPool.TryDequeue(out GameObject buttonObject))
					buttonObject = Instantiate(m_buttonPrefab, m_buttonLayout.transform);

				// Activate buttonObject
				buttonObject.SetActive(true);

				// Set button onClick events
				Button buttonComponent = buttonObject.GetComponent<Button>();

				buttonComponent.onClick.RemoveAllListeners();
				buttonComponent.onClick.AddListener(() =>
				{
					nowNode = GotoNode(nextNodeIndex);
					m_buttonPressed = true;
				});

				// Set button textbox
				buttonObject.GetComponentInChildren<TMP_Text>().text = buttonText;
			}
			else if (Regex.IsMatch(line, "^waitif"))
			{
				yield return new WaitUntil(() => m_buttonPressed);
				m_buttonPressed = false;
			}
			else if (Regex.IsMatch(line, $"^call {NamePattern}$"))
			{
				string callbackName = line.Split()[1];

				if (!m_callbackDict.TryGetValue(callbackName, out UnityEvent callback))
				{
					Debug.LogError($"(GSC)Invalid callback name: {callbackName}");
					break;
				}

				callback.Invoke();
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
	}

	// Goto the node in line nodeLineIndex and return that node name.
	string GotoNode(int nodeLineIndex)
	{
		// Clear textbox
		m_textBox.text = string.Empty;

		// Clear button layout
		foreach (var childButton in m_buttonLayout.GetComponentsInChildren<Button>())
		{
			var childObject = childButton.gameObject;

			childObject.SetActive(false);
			m_buttonObjectPool.Enqueue(childObject);
		}

		// Goto node
		m_lineIndex = nodeLineIndex;

		// Return node name
		string nodeName = m_scriptLines[m_lineIndex].Split()[1];
		return nodeName;
	}

	public void Temp()
	{
		print("callback!!!!!!!!!!");
	}
}
