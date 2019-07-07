using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Isis.Utility;

namespace Isis
{
	class Replacer
	{
		const string tagName = "command";
		
		/// <summary>
		/// シナリオ挿入タグの中身の文字列を抽出するための正規表現です。
		/// </summary>
		readonly Regex extracter;

		/// <summary>
		/// スクリプトファイル内のシナリオ挿入タグを扱うインスタンスを作成します。
		/// </summary>
		/// <param name="beginTag"></param>
		/// <param name="endTag"></param>
		public Replacer(string beginTag, string endTag)
		{
			//tagパターンのエスケープ
			beginTag = beginTag.EscapeExceptOr();
			endTag = endTag.EscapeExceptOr();

			string tagPattern =$"{beginTag}(?<{tagName}>.+){endTag}";
			extracter = new Regex(tagPattern, RegexOptions.Compiled);
		}

		/// <summary>
		/// シナリオ挿入タグをシナリオで置換します。
		/// </summary>
		/// <param name="script"></param>
		/// <param name="scenario"></param>
		/// <param name="commands"></param>
		/// <returns></returns>
		public IEnumerable<string> Replace(IEnumerable<string> script, Scenario scenario, Command[] commands)
		{
			foreach (var line in script)
			{
				var insertTagMatch = extracter.Match(line);

				//挿入タグが含まれなかった場合
				if (!insertTagMatch.Success)
				{
					yield return line;
					continue;
				}

				string insertTag = insertTagMatch.Groups[tagName].Value;
				foreach (var cmd in commands)
				{
					//コマンドの出力フォーマットにマッチしたら
					//シナリオ挿入タグをシナリオに置換
					var match = cmd.OutputPattern.Match(insertTag);
					string key = cmd.Name;
					if (match.Success)
					{
						string output = extracter.Replace(line, m => scenario.Next(key));
						yield return output;
						break;
					}
					else
					{
						ErrorPrint("登録されていないタグが検出されました。");
						Exit(ExitCode.UnknownTag);
					}
				}
			}
		}
	}
}
