using System.Collections;
using UnityEngine;

namespace GSC
{
	public class GSCExecuter
	{
		readonly GSCManager m_parent;

		public GSCExecuter(GSCManager parent) =>
			m_parent = parent;

		public IEnumerator Execute()
		{
			while (m_parent.Cursor.MoveNext())
			{
				GSCScriptLine now = m_parent.Cursor.Current;

				switch (now.Command)
				{
					case GSCCommand.Text:
						m_parent.AddText(now.Args[0]);
						break;

					case GSCCommand.Goto:
						m_parent.SetCursorToNode(now.Args[0]);
						break;

					case GSCCommand.If:
						m_parent.AddIfStatement(now.Args[0], now.Args[1]);
						break;

					case GSCCommand.Waitif:
						m_parent.StartType();

						var ifTask = m_parent.GetIfStatement();

						yield return new WaitUntil(() => ifTask.IsCompleted);
						m_parent.SetCursorToNode(ifTask.Result);

						break;

					case GSCCommand.Call:
						var callback = m_parent.GetCallback(now.Args[0]);

						if (callback is null)
							Debug.LogWarning($"(GSC)Callback not found: {now.Args[0]}");
						else
							callback.Invoke();

						break;

					case GSCCommand.Scene:
						m_parent.LoadSceneAsync(now.Args[0]);
						yield break;

					default:
						break;
				}
			}

			m_parent.StartType();
		}
	}
}
