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
		/// <summary>
		/// 文字列中の|（or）以外の正規表現文字をエスケープします。
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static string EscapeExceptOr(this string pattern)
		{
			if (pattern == "") return pattern;

			var result = new StringBuilder("(");
			char orChar = '|';
			var patList = pattern.Split(orChar);

			for(int i = 0; i < patList.Length - 1; i++)
			{
				result.Append(Regex.Escape(patList[i]))
					.Append(orChar);
			}

			result.Append(patList[patList.Length - 1])
				.Append(")");
			return result.ToString();
		}
	}
}
