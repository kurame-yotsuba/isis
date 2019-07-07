using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KurameLibrary;
using Newtonsoft.Json;

namespace Isis
{
	class Program
	{
		const string SettingsFilePath = "settings.json";

		static void Main(string[] args)
		{
			//設定ファイルの読み込み
			var json = File.ReadAllText(SettingsFilePath);
			var settings = Settings.Deserialize(json);

			//シナリオとスクリプトの読み込み
			var scenarioText = ReadContents(settings.ScenarioFilePath);
			var script = ReadContents(settings.ScriptFilePath);

			var scenario = new Scenario(scenarioText, settings.Commands);

			//シナリオのコンソールへの出力
			OutputScenario(scenario);

			//置換を行うインスタンスの作成
			var replacer = new Replacer(settings.BeginTag, settings.EndTag);
			var output = replacer.Replace(script, scenario, settings.Commands);

			Output(output);

			WriteContents(settings.OutputFilePath, output);
		}

		[Conditional("DEBUG")]
		static void OutputScenario(Scenario scenario)
		{
			var line = new string('-', 20);
			var nl = Environment.NewLine;
			Console.WriteLine(line + "解析結果" + line + nl);

			foreach (var (key, queue) in scenario)
			{
				foreach (var item in queue)
				{
					Console.WriteLine(item);
				}
			}

			Console.WriteLine(nl + line + "--------" + line + Environment.NewLine);
		}

		[Conditional("DEBUG")]
		static void Output(IEnumerable<string> contents)
		{
			foreach (var item in contents)
			{
				Console.WriteLine(item);
			}
		}

		/// <summary>
		/// コンテンツファイルを読み込みます。
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		static IEnumerable<string> ReadContents(string fileName)
		{
			using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			using (var sr = new StreamReader(fs))
			{
				while (!sr.EndOfStream)
				{
					yield return sr.ReadLine();
				}
			}
		}

		/// <summary>
		/// コンテンツファイルを出力します。
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="contents"></param>
		static void WriteContents(string fileName, IEnumerable<string> contents)
		{
			using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
			using (var sw = new StreamWriter(fs))
			{
				foreach (var line in contents)
				{
					sw.WriteLineAsync(line);
				}
			}
		}
	}
}
