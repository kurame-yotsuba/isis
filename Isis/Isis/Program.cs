using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isis
{
	class Program
	{
		static void Main(string[] args)
		{
			string scriptFilePath = "script.txt";
			string scenarioFilePath = "scenario.txt";

			var script = ReadScript(scriptFilePath);

			foreach (var line in script)
			{
				Console.WriteLine(line);
			}

			Console.WriteLine();
			//var cmd = new Command("default", @".+", @"\$");

			var scenario = ReadScenario(scenarioFilePath);

			foreach (var sc in scenario)
			{
				Console.WriteLine(sc);
			}
		}

		/// <summary>
		/// スクリプトファイルを読み込みます。
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		static IEnumerable<string> ReadScript(string fileName)
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

		static Queue<string> ReadScenario(string fileName)
		{
			var contents = File.ReadAllLines(fileName);
			var result = new Queue<string>();
			var tmp = "";
			foreach (var line in contents)
			{
				if(line == "")
				{
					result.Enqueue(tmp);
					tmp = "";
				}
				else
				{
					tmp += line + "\r\n" ;
				}
			}

			if(tmp != "")
			{
				result.Enqueue(tmp);
			}

			return result;
		}
	}
}
