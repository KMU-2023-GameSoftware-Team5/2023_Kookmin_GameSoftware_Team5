using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GSC
{
	public class GSCParser
	{
		readonly string[] m_lines;

		public GSCParser(GSCScript gsc) =>
			m_lines = gsc.script.Replace("\r", "").Split('\n');

		GSCCommand ParseCommand(string commandStr)
		{
			// Iterate GSCCommand to compare command
			GSCCommand? mayCmd = null;
			foreach (GSCCommand t_cmd in Enum.GetValues(typeof(GSCCommand)))
			{
				if (commandStr.Equals(t_cmd.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					mayCmd = t_cmd;
					break;
				}
			}

			// Check if command is not null
			if (mayCmd is GSCCommand ret)
				return ret;

			throw new UnityException($"(GSC)Invalid command: {commandStr}");
		}

		public List<string> ParseArguments(string argStr)
		{
			ReadOnlySpan<char> t_argStr = argStr.AsSpan();
			List<string> ret = new();

			// Keep slicing argStr to parse
			while (t_argStr.Length != 0)
			{
				Match match;

				// Only allow double-quoted or \w+ string
				// Group 1. Entire group
				// Group 2. Word group
				if ((match = Regex.Match(t_argStr.ToString(), "^(\\s*(\"[^\"]*\"|\\w+))")).Success)
				{
					// Slice this match from string
					int entireLength = match.Groups[1].Length;
					t_argStr = t_argStr[entireLength..];

					// Trim double quote and add to ret
					string text = match.Groups[2].Value;
					ret.Add(text.Trim('"'));
				}
				else
					throw new UnityException($"(GSC)Invalid character argument: {argStr}");
			}

			return ret;
		}

		public List<GSCScriptLine> Parse(char commandPrefix)
		{
			List<GSCScriptLine> ret = new();

			foreach (string line in m_lines)
			{
				string trimmed = line.Trim();

				GSCCommand cmd;
				List<string> args;

				Match match;
				if ((match = Regex.Match(trimmed, $"^{commandPrefix}(\\w+)(.*)")).Success)
				{
					string cmdStr = match.Groups[1].Value;
					string argStr = match.Groups[2].Value;

					cmd = ParseCommand(cmdStr);
					args = ParseArguments(argStr);
				}
				else
				{
					cmd = GSCCommand.Text;
					args = new List<string> { line };
				}

				ret.Add(new(cmd, args));
			}

			return ret;
		}
	}
}
