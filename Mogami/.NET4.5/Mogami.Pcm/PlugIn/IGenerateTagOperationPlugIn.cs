﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm.PlugIn
{
	/// <summary>
	/// 文字列からタグ生成を行うプラグインのインターフェース
	/// </summary>
	public interface IGenerateTagOperationPlugIn : IPlugIn
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		List<string> GenerateTag(string target);
	}
}
