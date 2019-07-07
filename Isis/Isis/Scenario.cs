using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KurameLibrary;

namespace Isis
{
	class Scenario/* : IEnumerable<(string key, List<ScenarioElement> elements)>*/
	{
		#region private member

		/// <summary>
		/// シナリオをコマンド名ごとにリストとして保持します。
		/// </summary>
		readonly Dictionary<string, List<ScenarioElement>> scenario;

		/// <summary>
		/// シナリオを取得する際の現在の位置を表します。
		/// </summary>
		readonly Dictionary<string, int> indexes;

		void Parse(IEnumerable<string> contents, Command[] commands)
		{
			foreach (var line in contents)
			{
				foreach (var cmd in commands)
				{
					var match = cmd.InputPattern.Match(line);
					string key = cmd.Name;
					if (match.Success)
					{
						if (!scenario.ContainsKey(key))
						{
							scenario[key] = new List<ScenarioElement>();
							indexes[key] = 0;
						}
						scenario[cmd.Name].Add(new ScenarioElement(cmd, match.Groups["text"].Value));
						break;
					}
				}
			}
		}

		#endregion

		#region public member

		public Scenario(IEnumerable<string> contents, Command[] commands)
		{
			scenario = new Dictionary<string, List<ScenarioElement>>();
			indexes = new Dictionary<string, int>();
			var cont = contents.Split("")
				.Select(x => x.Aggregate((y, z) => y + Environment.NewLine + z));

			Parse(cont, commands);
		}

		/// <summary>
		/// 次の要素を取得します。
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string Next(string key)
		{
			var text = scenario[key][indexes[key]++].Text;
			//if(indexes[key] == scenario[key].Count)
			//{
			//	indexes[key] = 0;
			//}
			return text;
		}

		public IEnumerator<(string key, List<ScenarioElement> elements)> GetEnumerator()
		{
			foreach (var (key, elements) in scenario)
			{
				yield return (key, elements);
			}
		}

		#endregion
	}
}
