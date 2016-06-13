using Mogami.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Constructions
{
	/// <summary>
	/// サムネイル区分
	/// </summary>
	public enum ThumbnailType
	{
		NON_SETTING,
		/// <summary>
		/// ListIcon用サムネイル
		/// </summary>
		[ThumbnailInfo("ListIcon")]
		LISTICON,
		/// <summary>
		/// プレビュー用サムネイル
		/// </summary>
		[ThumbnailInfo("PreviewImage")]
		PREVIEWIMAGE,
	}
}

