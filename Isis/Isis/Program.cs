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
			string fileName = "script.txt";

			var script = ReadScript(fileName);

			foreach (var line in script)
			{
				Console.WriteLine(line);
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
	}
}
