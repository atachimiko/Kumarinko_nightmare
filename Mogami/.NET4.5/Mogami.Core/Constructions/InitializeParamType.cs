using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Constructions
{
	/// <summary>
	/// 初期化処理フラグ
	/// </summary>
	[Flags]
	public enum InitializeParamType
	{
		DIRECTORY, DATABASE
	}
}
