using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	/// <summary>
	/// Artifact ⇔ Category
	/// </summary>
	[Table("T_Artifact2Category")]
	public class T_Artifact2Category : ServiceModel
	{


		#region フィールド

		private Artifact _Artifact;
		private Category _Category;

		private int _OrderNo;

		#endregion フィールド


		#region コンストラクタ

		public T_Artifact2Category()
		{
		}

		#endregion コンストラクタ


		#region プロパティ

		public virtual Artifact Artifact
		{
			get { return _Artifact; }
			set
			{
				if (_Artifact == value)
					return;
				_Artifact = value;
			}
		}


		public virtual Category Category
		{
			get
			{ return _Category; }
			set
			{
				if (_Category == value)
					return;
				_Category = value;
			}
		}


		public int OrderNo
		{
			get
			{ return _OrderNo; }
			set
			{
				if (_OrderNo == value)
					return;
				_OrderNo = value;
			}
		}

		#endregion プロパティ

	}
}
