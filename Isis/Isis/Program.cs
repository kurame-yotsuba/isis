using System;
using System.Collections.Generic;
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
			var json = File.ReadAllText(SettingsFilePath);
			var settings = Settings.Deserialize(json);

			var scenarioText = ReadContents(settings.ScenarioFilePath);
			var script = ReadContents(settings.ScriptFilePath);

			foreach (var line in script)
			{
				Console.WriteLine(line);
			}

			Console.WriteLine();

			var scenario = new Scenario(scenarioText, settings.Commands);

			foreach (var (key, queue) in scenario)
			{
				foreach (var item in queue)
				{
					Console.WriteLine(item);
				}
			}

			Console.WriteLine("-------------------");
			var replacer = new Replacer("{", "}");
			foreach (var item in replacer.Replace(script, scenario, settings.Commands))
			{
				Console.WriteLine(item);
			}

			var output = replacer.Replace(script, scenario, settings.Commands);

			WriteContents(settings.OutputFilePath, output);
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
