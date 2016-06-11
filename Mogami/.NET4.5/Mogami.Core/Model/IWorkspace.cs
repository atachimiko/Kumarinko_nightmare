using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mogami.Core.Model
{
	public interface IWorkspace
	{

		#region プロパティ

		/// <summary>
		/// 
		/// </summary>
		long Id { get; }

		/// <summary>
		/// ワークスペース名
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 物理空間パス
		/// </summary>
		string PhysicalPath { get; set; }

		/// <summary>
		/// 仮想空間パス
		/// </summary>
		string WorkspacePath { get; }

		#endregion プロパティ

	}

	public static class WorkspaceExtension
	{

		#region メソッド

		/// <summary>
		/// 指定した文字列からワークスペースのパス部分を削除した文字列を返します。
		/// </summary>
		/// <param name="this"></param>
		/// <param name="path"></param>
		/// <param name="removeAclExtension">ACLファイル拡張子を除去する</param>
		/// <returns></returns>
		public static string TrimWorekspacePath(this IWorkspace @this, string path, bool removeAclExtension = false)
		{
			var escaped = Regex.Escape(@this.WorkspacePath);
			Regex re = new Regex("^" + escaped + @"[\\]*", RegexOptions.Singleline);
			string key = re.Replace(path, "");

			if (removeAclExtension)
			{
				Regex re2 = new Regex("\\.aclgene$", RegexOptions.Singleline);
				key = re2.Replace(key, "");
			}

			return key;
		}

		#endregion メソッド

	}
}
