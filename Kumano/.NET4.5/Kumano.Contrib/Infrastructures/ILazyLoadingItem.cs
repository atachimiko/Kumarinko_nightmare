using System;

namespace Kumano.Contrib.Infrastructures
{
	public interface ILazyLoadingItem
	{
		/// <summary>
		/// Bindingで遅延ロードを実行したかどうかの判定フラグを取得します
		/// </summary>
		bool IsLoaded { get; }

		/// <summary>
		/// 遅延読み込みで取得したデータを引数にして呼び出します 実装により必要なデータをコピーしてください。
		/// </summary>
		/// <param name="loadedData"></param>
		void LoadedFromData(ILazyLoadingItem loadedData);

		/// <summary>
		/// IsLoadedプロパティ内で呼び出すイベントハンドラ
		/// </summary>
		event EventHandler Loaded;
	}
}