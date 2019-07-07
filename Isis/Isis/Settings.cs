using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isis
{
	class Settings
	{
		#region inner class

		/// <summary>
		/// JSON用の中間形式です。
		/// </summary>
		public class CommandPattern
		{
			public string Name { get; set; }
			public string InputPattern { get; set; }
			public string OutputPattern { get; set; }
		}

		#endregion

		#region static member

		/// <summary>
		/// JSON形式からインスタンスを生成します。
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public static Settings Deserialize(string json)
		{
			var obj = JsonConvert.DeserializeObject<Settings>(json);
			return obj;
		}

		#endregion

		#region instance member
		#region property
		public string ScenarioFilePath { get; } = "scenario.txt";
		public string ScriptFilePath { get; } = "script.txt";
		public string OutputFilePath { get; } = "output.txt";

		/// <summary>
		/// スクリプトファイルにおけるシナリオを挿入する箇所を表すタグの開始パターンです。
		/// </summary>
		public string BeginTag { get; } = "{";

		/// <summary>
		/// スクリプトファイルにおけるシナリオを挿入する箇所を表すタグの終了パターンです。
		/// </summary>
		public string EndTag { get; } = "}";
		public Command[] Commands { get; }
		#endregion

		public Settings(string scenarioFilePath, string scriptFilePath, string outputFilePath,
			string beginTag, string endTag,
			CommandPattern[] commandPatterns)
		{
			ScriptFilePath = scriptFilePath;
			ScenarioFilePath = scenarioFilePath;
			OutputFilePath = outputFilePath;
			BeginTag = beginTag;
			EndTag = endTag;

			Commands = commandPatterns
				.Select(x => new Command(x.Name, x.InputPattern, x.OutputPattern))
				.ToArray();
		}

		/// <summary>
		/// JSON形式にシリアライズします。
		/// </summary>
		/// <returns></returns>
		public string Serialize()
		{
			var obj = new
			{
				ScenarioFilePath,
				ScriptFilePath,
				OutputFilePath,
				BeginTag,
				EndTag,
				//JSON用の中間形式に変換
				CommandPatterns = Commands.Select(x => new CommandPattern
				{
					Name = x.Name,
					InputPattern = x.InputPattern.ToString(),
					OutputPattern = x.OutputPattern.ToString(),
				}),
			};
			var json = JsonConvert.SerializeObject(obj, Formatting.Indented);

			return json;
		}

		#endregion
	}
}
