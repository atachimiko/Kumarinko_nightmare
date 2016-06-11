using Akalib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami
{
	public class BuildAssemblyParameter : IBuildAssemblyParameter
	{

		#region フィールド

		/// <summary>
		/// 
		/// </summary>
		/// <remarks><pre>
		/// "ApplicationDirectoryPath" - アプリケーション情報を保存するベースディレクトリ。
		///                              最終的なパスは、「マイドキュメント＋ApplicationDirectoryPath」になる。
		/// </pre>
		/// </remarks>
		private readonly Dictionary<string, string> _Params = new Dictionary<string, string>();

		#endregion フィールド


		#region コンストラクタ

		public BuildAssemblyParameter()
		{
			BuildParams();
		}

		#endregion コンストラクタ


		#region プロパティ

		public Dictionary<string, string> Params { get { return _Params; } }

		#endregion プロパティ


		#region メソッド

		private void BuildParams()
		{
			if (!_Params.ContainsKey("ApplicationDirectoryPath"))
			{
#if DEBUG
				this.Params.Add("ApplicationDirectoryPath", @"\Mogami_dev");
#else
				this.Params.Add("ApplicationDirectoryPath", @"\Mogami");
#endif
			}
		}

		#endregion メソッド
	}
}
