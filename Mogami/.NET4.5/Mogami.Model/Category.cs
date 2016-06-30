using Livet;
using Mogami.Core.Constructions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	[Table("alCategory")]
	public class Category : ServiceModel
	{


		#region フィールド

		private IList<T_Artifact2Category> _Artifacts;
		private CategoryType _CategoryTypeCode;
		private IList<Category> _ChildCategories;
		private string _Name;

		private Category _ParentCategory;

		#endregion フィールド


		#region コンストラクタ

		public Category()
		{
			this.Artifacts = new ObservableSynchronizedCollection<T_Artifact2Category>();
			this.ChildCategories = new ObservableSynchronizedCollection<Category>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public virtual IList<T_Artifact2Category> Artifacts
		{
			get
			{
				return _Artifacts;
			}
			set
			{
				if (_Artifacts == value)
					return;
				_Artifacts = value;
			}
		}

		public IOrderedEnumerable<T_Artifact2Category> ArtifactsOrdered
		{
			get { return _Artifacts.OrderBy(prop => prop.OrderNo); }
		}

		public CategoryType CategoryTypeCode
		{
			get
			{ return _CategoryTypeCode; }
			set
			{
				if (_CategoryTypeCode == value)
					return;
				_CategoryTypeCode = value;
			}
		}

		public virtual IList<Category> ChildCategories
		{
			get
			{ return _ChildCategories; }
			set
			{
				if (_ChildCategories == value)
					return;
				_ChildCategories = value;
			}
		}
		public string Name
		{
			get
			{ return _Name; }
			set
			{
				if (_Name == value)
					return;
				_Name = value;
			}
		}

		public virtual Category ParentCategory
		{
			get
			{ return _ParentCategory; }
			set
			{
				if (_ParentCategory == value)
					return;
				_ParentCategory = value;
			}
		}

		#endregion プロパティ

	}
}
