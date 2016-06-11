using Akalib.Sample.TreeList.DataModel;
using Akalib.Wpf.Control.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Sample.TreeList
{
	/// <summary>
	/// TreeListで表示するためのアイテムを格納したデータモデル
	/// </summary>
	public class PersonTreeModel : ITreeModel
	{

		#region コンストラクタ

		public PersonTreeModel()
		{
			Root = new Person();
		}

		#endregion コンストラクタ


		#region プロパティ

		public Person Root { get; private set; }

		#endregion プロパティ


		#region メソッド

		/// <summary>
		/// テストデータ
		/// </summary>
		/// <param name="count1">第1階層(ルート階層)に作成するアイテム数</param>
		/// <param name="count2">第2階層に生成するアイテムの数</param>
		/// <param name="count3">第3階層に生成するアイテムの数</param>
		/// <returns></returns>
		public static PersonTreeModel CreateTestModel(int count1, int count2, int count3)
		{
			var model = new PersonTreeModel();
			for (int i = 0; i < count1; i++)
			{
				var p = new Person() { Name = "Person A " + i.ToString() };
				model.Root.Children.Add(p);

				for (int n = 0; n < count2; n++) // 第2階層
				{
					var p2 = new Person() { Name = "Person B" + n.ToString() };
					p.Children.Add(p2);

					for (int k = 0; k < count3; k++) // 第3階層
					{
						p2.Children.Add(new Person() { Name = "Person C" + k.ToString() });
					}
				}
			}
			return model;
		}
		public System.Collections.IEnumerable GetChildren(object parent)
		{
			if (parent == null)
				parent = Root;
			return (parent as Person).Children;
		}

		public bool HasChildren(object parent)
		{
			return (parent as Person).Children.Count > 0;
		}

		#endregion メソッド
	}
}
