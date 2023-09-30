using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GSC
{
	[Serializable]
	public class ScriptCallback
	{
		public string name;
		public UnityEvent onCall;
	}

	public class GSCManager : MonoBehaviour
	{
		[SerializeField] GSCScript m_GSCScript;

		[SerializeField] GSCTextbox m_GSCTextbox;
		[SerializeField] GameObject m_buttonPrefab;
		[SerializeField] Transform m_buttonParent;

		[SerializeField] string m_startNode;
		[SerializeField] char m_prefix;

		// Need to be public?
		[SerializeField] List<ScriptCallback> m_callbacks;

		/// <summary>
		/// The cursor that points to script line to be executed next
		/// </summary>
		public IEnumerator<GSCScriptLine> Cursor
		{
			get;
			private set;
		}

		List<GSCScriptLine> m_scriptLines;

		/// <summary>
		/// When a button is pressed, the value is set as target node
		/// </summary>
		TaskCompletionSource<string> m_ifStatement = new();

		IEnumerator m_scriptCoroutine;

		void OnEnable()
		{
			// 1. Parse script if not parsed yet
			if (m_scriptLines is null)
			{
				GSCParser parser = new(m_GSCScript);
				m_scriptLines = parser.Parse(m_prefix);
			}

			// 2. Stop previous coroutine
			if (m_scriptCoroutine is not null)
				StopCoroutine(m_scriptCoroutine);

			// 3. Set Cursor to start node
			SetCursorToNode(m_startNode);

			// 4. Execute with coroutine
			GSCExecuter executer = new(this);
			m_scriptCoroutine = executer.Execute();

			StartCoroutine(m_scriptCoroutine);
		}

		/// <summary>
		/// Move Cursor to nodeName
		/// </summary>
		/// <exception cref="UnityException"> Throws UnityException when node not exist </exception>
		public void SetCursorToNode(string nodeName)
		{
			Cursor = m_scriptLines.GetEnumerator();

			while (Cursor.MoveNext())
			{
				var now = Cursor.Current;
				if (now.Command == GSCCommand.Node && now.Args[0] == nodeName)
					return;
			}

			throw new UnityException($"(GSC)Invalid node name: {nodeName}");
		}

		/// <summary>
		/// Add text to GSC textbox. Must call StartType() method later
		/// </summary>
		public void AddText(string text)
		{
			if (m_GSCTextbox == null)
			{
				Debug.LogWarning("(GSC)Target textbox not exist");
				return;
			}

			m_GSCTextbox.AddText(text);
		}

		/// <summary>
		/// Add if statement as button
		/// </summary>
		/// <param name="text"> The text to write on button </param>
		/// <param name="targetNodeName"> Target node to move when a button is pressed </param>
		public void AddIfStatement(string text, string targetNodeName)
		{
			GameObject buttonObj = Instantiate(m_buttonPrefab, m_buttonParent);

			var textbox = buttonObj.GetComponentInChildren<TMP_Text>();
			var button = buttonObj.GetComponent<Button>();

			textbox.SetText(text);

			// This variables are exist to resolve closure problem
			var t_ifStatement = m_ifStatement;
			var t_textbox = m_GSCTextbox;

			button.onClick.AddListener(() =>
			{
				// Set target node
				t_ifStatement.TrySetResult(targetNodeName);

				// Clear textbox
				t_textbox.Clear();

				// Destroy all buttons
				foreach (Transform buttonTransform in m_buttonParent)
					Destroy(buttonTransform.gameObject);
			});
		}

		/// <summary>
		/// Start type GSC Textbox
		/// </summary>
		public void StartType() =>
			m_GSCTextbox.StartTyping();

		/// <summary>
		/// If statement will be reset.
		/// </summary>
		/// <returns> Return Task of if statement </returns>
		public Task<string> GetIfStatement()
		{
			var ret = m_ifStatement.Task;
			m_ifStatement = new();

			return ret;
		}

		/// <summary>
		/// Get registered callback by name
		/// </summary>
		/// <returns> Return callback event. If not exist, return null </returns>
		public UnityEvent GetCallback(string callbackName)
		{
			foreach (var callback in m_callbacks)
				if (callback.name == callbackName)
					return callback.onCall;

			return null;
		}

		// Is it OK...?
		public void LoadSceneAsync(string sceneName)
		{
			if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
				throw new UnityException($"(GSC)Scene not exist: {sceneName}");

			SceneManager.LoadSceneAsync(sceneName);
		}
	}
}
