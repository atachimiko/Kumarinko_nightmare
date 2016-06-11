using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Sample.TreeList.DataModel
{
	public class Person
	{

		#region フィールド
		/// <summary>
		/// Personクラスをインスタンス化した回数をカウントする変数
		/// </summary>
		static int _i;
		private readonly ObservableCollection<Person> _children = new ObservableCollection<Person>();

		#endregion フィールド


		#region コンストラクタ

		public Person()
		{
			Id = ++_i;
		}

		#endregion コンストラクタ


		#region プロパティ

		public ObservableCollection<Person> Children
		{
			get { return _children; }
		}

		public int Id { get; set; }
		public string Name { get; set; }

		#endregion プロパティ


		#region メソッド

		public override string ToString()
		{
			return Name;
		}

		#endregion メソッド
	}
}
