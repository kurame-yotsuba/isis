using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Isis
{
	static class Utility
	{
		const char orChar = '|';

		/// <summary>
		/// 文字列中の|（or）以外の正規表現文字をエスケープします。
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static string EscapeExceptOr(this string pattern)
		{
			if (pattern == "") return pattern;

			var result = new StringBuilder();

			//一度orCharで分割してから、エスケープして
			var patList = pattern.Split(orChar)
				.Select(x => Regex.Escape(x));

			//orCharで結合する
			result
				.Append("(")
				.Append(string.Join(orChar.ToString(), patList))
				.Append(")");
			
			return result.ToString();
		}

		/// <summary>
		/// contentsをsplitterに一致する行で分割します。
		/// </summary>
		/// <param name="contents"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<string>> Split(this IEnumerable<string> contents, string splitter)
		{
			List<string> output = new List<string>();
			foreach (var item in contents)
			{
				if (item == splitter && output.Count > 0)
				{
					yield return output;
					output = new List<string>();
				}
				else
				{
					output.Add(item);
				}
			}

			//最終要素がsplitterじゃ無かった場合に
			//最後のoutputをここで返す。
			if (output.Count > 0) yield return output;
		}

		/// <summary>
		/// エラーメッセージの出力を行います。
		/// </summary>
		/// <param name="message"></param>
		public static void ErrorPrint(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("エラー：" + message);
			Console.ResetColor();
		}

		/// <summary>
		/// 警告のメッセージの出力を行います。
		/// </summary>
		/// <param name="message"></param>
		public static void WarningPrint(string message)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("警告：" + message);
			Console.ResetColor();
		}

		/// <summary>
		/// 終了メッセージを出力してから終了します。
		/// </summary>
		/// <param name="exitCode"></param>
		public static void Exit(ExitCode exitCode)
		{
			Console.WriteLine("\n処理を終了しました。");
			Console.WriteLine("終了するには何かキーを押してください。");

			Console.ReadKey(intercept: true);

			Environment.Exit((int)exitCode);
		}
	}
}
