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
			var script = ReadScript();

			foreach (var line in script)
			{
				Console.WriteLine(line);
			}
		}

		static IEnumerable<string> ReadScript()
		{
			string fileName = "script.txt";

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
