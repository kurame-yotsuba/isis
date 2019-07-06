using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Isis
{
	/// <summary>
	/// シナリオコマンドを表します。
	/// </summary>
	class Command
	{
		/// <summary>
		/// シナリオコマンドの名前です。
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// シナリオコマンドを入力するための正規パターンです。
		/// </summary>
		public Regex InputPattern { get; }

		/// <summary>
		/// シナリオを出力する位置を表す正規パターンです。
		/// </summary>
		public Regex OutputPattern { get; }

		public Command(string name, string inputPattern, string outputPattern)
		{
			Name = name;
			InputPattern = new Regex("^" + inputPattern + @"(?<text>.*)", RegexOptions.Compiled | RegexOptions.Singleline);
			OutputPattern = new Regex(outputPattern, RegexOptions.Compiled | RegexOptions.Singleline);
		}
	}
}
