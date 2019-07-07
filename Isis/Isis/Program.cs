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
		static Settings settings;
		static IEnumerable<string> script;
		static Scenario scenario;

		static int Main(string[] args)
		{
			FileExistCheck(SettingsFilePath);

			Initialize();

			//シナリオのコンソールへの出力
			OutputScenario(scenario);

			//置換を行うインスタンスの作成
			var replacer = new Replacer(settings.BeginTag, settings.EndTag);
			var output = replacer.Replace(script, scenario, settings.Commands);

			Output(output);

			WriteContents(settings.OutputFilePath, output);


			Console.WriteLine("\n処理を終了しました。");
			Console.WriteLine("終了するには何かキーを押してください。");

			Console.ReadKey(intercept: true);

			return (int)ExitCode.Success;
		}

		static void Initialize()
		{
			//設定ファイルの読み込み
			var json = File.ReadAllText(SettingsFilePath);
			try
			{
				settings = Settings.Deserialize(json);
			}
			catch(Settings.SettingsException e)
			{
				Console.WriteLine(e.Message);
				Exit(ExitCode.WrongSettingFile);
			}

			FileExistCheck(settings.ScenarioFilePath);
			FileExistCheck(settings.ScriptFilePath);

			if (File.Exists(settings.OutputFilePath))
			{
				Console.WriteLine(settings.OutputFilePath + "は既に存在します。");
				Console.WriteLine("上書きしますか？[Y/n]");
				var overwritten = KurameUtility.InputYesNo(Console.ReadLine, Console.WriteLine, true);
				if (!overwritten)
				{
					Exit(ExitCode.DenyOverwritten);
				}
			}

			//シナリオとスクリプトの読み込み
			var scenarioText = ReadContents(settings.ScenarioFilePath);
			scenario = new Scenario(scenarioText, settings.Commands);

			script = ReadContents(settings.ScriptFilePath);
		}

		#region DEBUG用

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

		#endregion

		/// <summary>
		/// ファイルの存在チェックを行います。
		/// </summary>
		/// <param name="filePath"></param>
		static void FileExistCheck(string filePath)
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine(filePath + "が見つかりません。");
				Exit(ExitCode.NotFoundFile);
			}
		}

		static void Exit(ExitCode exitCode)
		{
			Console.WriteLine("\n処理を終了しました。");
			Console.WriteLine("終了するには何かキーを押してください。");

			Console.ReadKey(intercept: true);

			Environment.Exit((int)exitCode);
		}

		#region ファイル読み込み、書き込み用

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

		#endregion
	}

	/// <summary>
	/// 終了コード
	/// </summary>
	enum ExitCode
	{
		Success,
		NotFoundFile,
		WrongSettingFile,
		DenyOverwritten,
	}
}
