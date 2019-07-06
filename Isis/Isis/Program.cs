using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KurameLibrary;

namespace Isis
{
	class Program
	{
		static void Main(string[] args)
		{
			string scriptFilePath = "script.txt";
			string scenarioFilePath = "scenario.txt";
			string outputFilePath = "output.txt";

			var script = ReadContents(scriptFilePath);
			var scenarioText = ReadContents(scenarioFilePath);

			foreach (var line in script)
			{
				Console.WriteLine(line);
			}

			Console.WriteLine();
			var commands = new[]{
				new Command("monologlue", @"^[#＃](?<text>.*)", "#"),
				new Command("serif", @"(?<text>.*)", @"$"),
			};

			var scenario = new Scenario(scenarioText, commands);

			foreach (var (key, queue) in scenario)
			{
				foreach (var item in queue)
				{
					Console.WriteLine(item);
				}
			}

			Console.WriteLine("-------------------");

			foreach (var item in Replace(script, scenario, commands))
			{
				Console.WriteLine(item);
			}

			var output = Replace(script, scenario, commands);

			WriteContents(outputFilePath, output);
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

		static IEnumerable<string> Replace(IEnumerable<string> script, Scenario scenario, Command[] commands)
		{
			const string tagName = "command";
			var extracter = new Regex($"\\{{(?<{tagName}>.+)\\}}");
			foreach (var line in script)
			{
				string output = extracter.Replace(line, m => scenario.Next(m.Groups[tagName].Value));
				yield return output;
			}
		}
	}
}
