using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Infrastructure
{
	/// <summary>
	/// ワークフロー内で使用するデータベースに関するコンテキストを返すためのインターフェース
	/// </summary>
	public interface IWorkflowContext
	{
		#region プロパティ

		/// <summary>
		/// データベースのコンテキストを取得します
		/// </summary>
		DbContext DbContext { get; }

		/// <summary>
		/// サムネイル生成マネージャ
		/// </summary>
		IThumbnailManager ThumbnailManager { get; }

		#endregion プロパティ
	}
}
