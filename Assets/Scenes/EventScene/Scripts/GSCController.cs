using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GSC
{
	/// <summary>
	/// Button is target, TMP_Text(Can be null) is text of button
	/// </summary>
	/// <returns>
	/// Return components tuple of it
	/// </returns>
	public delegate Tuple<Button, TMP_Text> ButtonCreator();

	/// <summary>
	/// Destroy all buttons that created by ButtonCreator
	/// </summary>
	public delegate void ButtonCleaner();

	public class GSCController : UnityEngine.Object
	{
		readonly List<GSCScriptLine> m_scripts;
		readonly List<GSCCallback> m_callbacks;
		readonly GSCTextbox m_textbox;

		readonly ButtonCreator m_buttonCreator;
		readonly ButtonCleaner m_buttonCleaner;

		readonly List<GameObject> m_buttonObjects = new();

		// The cursor that points to script line to be executed next
		IEnumerator<GSCScriptLine> m_scriptCursor;

		// When a button is pressed, the value is set
		string m_branchTarget = null;

		public GSCController(
			List<GSCScriptLine> scripts,
			List<GSCCallback> callbacks,
			GSCTextbox textbox,
			ButtonCreator buttonCreator,
			ButtonCleaner buttonCleaner,
			string startNode
		)
		{
			m_scripts = scripts;
			m_callbacks = callbacks;
			m_textbox = textbox;

			m_buttonCreator = buttonCreator;
			m_buttonCleaner = buttonCleaner;

			BranchTo(startNode);
		}

		public GSCScriptLine Next()
		{
			// End of script
			if (!m_scriptCursor.MoveNext())
			{
				m_textbox.StartTyping();
				return null;
			}

			return m_scriptCursor.Current;
		}

		public void AddText(string text) =>
			m_textbox.AddText(text);

		public void BranchTo(string nodeName)
		{
			m_scriptCursor = m_scripts.GetEnumerator();

			while (m_scriptCursor.MoveNext())
			{
				var now = m_scriptCursor.Current;
				if (now.Command == GSCCommand.Node && now.Args[0] == nodeName)
					return;
			}

			throw new UnityException($"(GSC)Invalid node name: {nodeName}");
		}

		public void AddConditional(string description, string branchTarget)
		{
			var (button, textbox) = m_buttonCreator.Invoke();

			button.onClick.AddListener(() => m_branchTarget ??= branchTarget);

			if (textbox != null)
				textbox.SetText(description);

			m_buttonObjects.Add(button.gameObject);
		}

		public IEnumerator BranchConditional()
		{
			m_textbox.StartTyping();

			yield return new WaitWhile(() => m_branchTarget is null);

			m_buttonCleaner.Invoke();
			m_textbox.Clear();

			BranchTo(m_branchTarget);
			m_branchTarget = null;
		}

		public void InvokeCallback(string name)
		{
			foreach (var callback in m_callbacks)
			{
				if (callback.name == name)
				{
					callback.onCall.Invoke();
					return;
				}
			}

			Debug.LogWarning($"(GSC)Callback not found: {name}");
		}

		// Is it OK...?
		public void LoadScene(string sceneName)
		{
			if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
				throw new UnityException($"(GSC)Scene not exist: {sceneName}");

			SceneManager.LoadSceneAsync(sceneName);
		}
	}
}
