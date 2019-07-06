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
			string tagPattern = $"{beginTag}(?<{tagName}>.+){endTag}";
			extracter = new Regex(tagPattern, RegexOptions.Compiled);
		}

		public IEnumerable<string> Replace(IEnumerable<string> script, Scenario scenario, Command[] commands)
		{
			foreach (var line in script)
			{
				string output = extracter.Replace(line, m => scenario.Next(m.Groups[tagName].Value));
				yield return output;
			}
		}
	}
}
