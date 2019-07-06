using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KurameLibrary;

namespace Isis
{
	class Scenario : IEnumerable<(string key, List<ScenarioElement> elements)>
	{
		Dictionary<string, List<ScenarioElement>> scenario;
		Dictionary<string, int> indexes;

		public Scenario(IEnumerable<string> contents, Command[] commands)
		{

			scenario = new Dictionary<string, List<ScenarioElement>>();
			indexes = new Dictionary<string, int>();
			var cont = kirinuki(contents).ToArray();
			parse(cont, commands);

			static IEnumerable<string> kirinuki(IEnumerable<string> contents)
			{
				string tmp = "";
				foreach (var line in contents)
				{
					if (line == "")
					{
						yield return tmp;
						tmp = "";
					}
					else
					{
						if(tmp != "")
						{
							tmp += Environment.NewLine;
						}
						tmp += line;
					}
				}

				if (tmp != "")
				{
					yield return tmp;
				}
			}

			void parse(IEnumerable<string> contents, Command[] commands)
			{
				foreach (var line in contents)
				{
					foreach (var cmd in commands)
					{
						var m = cmd.InputPattern.Match(line);
						//var outputPat = cmd.OutputPattern.ToString();
						string key = cmd.Name;
						if (m.Success)
						{
							if (!scenario.ContainsKey(key))
							{
								scenario[key] = new List<ScenarioElement>();
								indexes[key] = 0;
							}
							scenario[cmd.Name].Add(new ScenarioElement(cmd, m.Groups["text"].Value));
							break;
						}
					}
				}
			}
		}

		public string Next(string key)
		{
			var text = scenario[key][indexes[key]++].Text;
			if(indexes[key] == scenario[key].Count)
			{
				indexes[key] = 0;
			}
			return text;
		}

		public IEnumerator<(string key, List<ScenarioElement> elements)> GetEnumerator()
		{
			foreach (var (key, elements) in scenario)
			{
				yield return (key, elements);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
