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
			List<string> ret = new();

			// Keep slicing argStr to parse
			while (argStr != string.Empty)
			{
				argStr = argStr.TrimStart();

				// Only allow double-quoted or \w+ string
				// Match with double-quoted string first
				Match match = Regex.Match(argStr, "^(\"[^\"]*\")");

				// And match with word string secondly
				if (match.Success || (match = Regex.Match(argStr, "^(\\w+)")).Success)
				{
					string temp = match.Groups[1].Value;

					argStr = argStr[temp.Length..];

					ret.Add(temp.Trim('"'));
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

				if (trimmed == string.Empty)
					continue;

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
