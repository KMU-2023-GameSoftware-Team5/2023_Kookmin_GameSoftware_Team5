using System.Collections.Generic;

namespace GSC
{
	public enum GSCCommand
	{
		Text,
		Node,
		Goto,
		If,
		Waitif,
		Call,
		Scene
	}

	public class GSCScriptLine
	{
		public readonly GSCCommand Command;
		public readonly List<string> Args;

		public GSCScriptLine(GSCCommand command, List<string> args)
		{
			Command = command;
			Args = args;
		}
	}
}
