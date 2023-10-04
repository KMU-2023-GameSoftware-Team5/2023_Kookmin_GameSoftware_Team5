using System.Collections;

namespace GSC
{
	public class GSCExecuter
	{
		readonly GSCController m_controller;

		public GSCExecuter(GSCController controller) =>
			m_controller = controller;

		public IEnumerator Execute()
		{
			GSCScriptLine now;

			while ((now = m_controller.Next()) is not null)
			{
				switch (now.Command)
				{
					case GSCCommand.Text:
						m_controller.AddText(now.Args[0]);
						break;

					case GSCCommand.Goto:
						m_controller.BranchTo(now.Args[0]);
						break;

					case GSCCommand.If:
						m_controller.AddConditional(now.Args[0], now.Args[1]);
						break;

					case GSCCommand.Waitif:
						yield return m_controller.BranchConditional();
						break;

					case GSCCommand.Call:
						m_controller.InvokeCallback(now.Args[0]);
						break;

					case GSCCommand.Scene:
						m_controller.LoadScene(now.Args[0]);
						yield break;

					default:
						break;
				}
			}
		}
	}
}