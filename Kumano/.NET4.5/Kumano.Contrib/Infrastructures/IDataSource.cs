using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Contrib.Infrastructures
{
	/// <summary>
	/// データソースを持つコレクションクラスのインターフェース
	/// </summary>
	/// <typeparam name="T"></typeparam>
	interface IDataSource<T>
		where T : ICollection
	{
		T Items { get; }
	}
}
