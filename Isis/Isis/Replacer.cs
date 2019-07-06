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
		readonly string tagName;
		readonly Regex extracter;

		public Replacer(string tagName, string tagPattern)
		{
			this.tagName = tagName;
			this.extracter = new Regex(tagPattern, RegexOptions.Compiled);
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
