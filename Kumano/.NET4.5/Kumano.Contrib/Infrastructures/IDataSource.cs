using System.Collections;

namespace Kumano.Contrib.Infrastructures
{
	/// <summary>
	/// データソースを持つコレクションクラスのインターフェース
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal interface IDataSource<T>
		where T : ICollection
	{
		T Items { get; }
	}
}