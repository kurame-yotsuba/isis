using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Isis
{
	class Replacer
	{
		const string tagName = "command";
		readonly Regex extracter;

		public Replacer(string beginTag, string endTag)
		{
			(beginTag, endTag) = (Regex.Escape(beginTag), Regex.Escape(endTag));
			string tagPattern =$"{beginTag}(?<{tagName}>.+){endTag}";
			extracter = new Regex(tagPattern, RegexOptions.Compiled);
		}

		public IEnumerable<string> Replace(IEnumerable<string> script, Scenario scenario, Command[] commands)
		{
			foreach (var line in script)
			{
				var varText = extracter.Match(line);
				if (!varText.Success)
				{
					yield return line;
					continue;
				}

				foreach (var cmd in commands)
				{
					var m = cmd.OutputPattern.Match(varText.Groups[tagName].Value);
					string key = cmd.Name;
					if (m.Success)
					{
						string output = extracter.Replace(line, m => scenario.Next(key));
						yield return output;
						break;
					}
				}
			}
		}
	}
}
