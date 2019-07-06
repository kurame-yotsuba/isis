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
		public string ScenarioFilePath { get; } = "scenario.txt";
		public string ScriptFilePath { get; } = "script.txt";
		public string OutputFilePath { get; } = "output.txt";
		public string BeginTag { get; } = "{";
		public string EndTag { get; } = "}";
		public Command[] Commands { get; }

		public Settings(string scenarioFilePath, string scriptFilePath, string outputFilePath,
			string beginTag, string endTag,
			CommandPattern[] commandPatterns)
		{
			ScriptFilePath = scriptFilePath;
			ScenarioFilePath = scenarioFilePath;
			OutputFilePath = outputFilePath;
			BeginTag = beginTag;
			EndTag = endTag;
			Commands = commandPatterns.Select(x => new Command(x.Name, x.InputPattern, x.OutputPattern)).ToArray();
		}

		public string Serialize()
		{
			var obj = new
			{
				ScenarioFilePath,
				ScriptFilePath,
				OutputFilePath,
				BeginTag,
				EndTag,
				CommandPatterns = Commands.Select(x => new
				{
					x.Name,
					InputPattern = x.InputPattern.ToString(),
					OutputPattern = x.OutputPattern.ToString(),
				}),
			};
			var json = JsonConvert.SerializeObject(obj, Formatting.Indented);

			return json;
		}

		public static Settings Deserialize(string json)
		{
			var obj = JsonConvert.DeserializeObject<Settings>(json);
			return obj;
		}

		//public static Settings Default
		//{
		//	get
		//	{
		//		var commands = new[]
		//		{
		//			new CommandPattern { Name = "monologlue", InputPattern = @"^[#＃](?<text>.*)", OutputPattern = "#" },
		//			new CommandPattern{ Name = "serif", InputPattern = @"(?<text>.*)", OutputPattern = @"$" },
		//		};

		//		var result = new Settings(
		//			"scenario.txt", "script.txt", "output.txt", commands);

		//		return result;
		//	}
		//}

		public class CommandPattern
		{
			public string Name;
			public string InputPattern;
			public string OutputPattern;
		}
	}
}
