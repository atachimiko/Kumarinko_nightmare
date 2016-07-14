using Livet;
using Mogami.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	[Table("alTag")]
	public class Tag : ServiceModel, ITag
	{

		#region フィールド

		private IList<Artifact> _Artifacts;

		private IList<Category> _Categories;

		private IList<Tag> _ChildTags;

		private DateTime? _CreateDate;

		private DateTime? _LastUpdate;

		private string _Name;

		private Tag _ParentTag;

		#endregion フィールド


		#region コンストラクタ

		public Tag()
		{
			this.Artifacts = new ObservableSynchronizedCollection<Artifact>();
			this.Categories = new ObservableSynchronizedCollection<Category>();
			this.ChildTag = new ObservableSynchronizedCollection<Tag>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public virtual IList<Artifact> Artifacts
		{
			get
			{ return _Artifacts; }
			set
			{
				if (_Artifacts == value)
					return;
				_Artifacts = value;
			}
		}

		/// <summary>
		/// 関連するカテゴリ一覧を取得、または設定します。
		/// </summary>
		public virtual IList<Category> Categories
		{
			get
			{ return _Categories; }
			set
			{
				if (_Categories == value)
					return;
				_Categories = value;
			}
		}

		public virtual IList<Tag> ChildTag
		{
			get
			{ return _ChildTags; }
			set
			{
				if (_ChildTags == value)
					return;
				_ChildTags = value;
			}
		}

		public DateTime? CreateDate
		{
			get { return _CreateDate; }
			set
			{
				if (_CreateDate == value)
					return;
				_CreateDate = value;
			}
		}

		public DateTime? LastUpdate
		{
			get { return _LastUpdate; }
			set
			{
				if (_LastUpdate == value)
					return;
				_LastUpdate = value;
			}
		}

		[Required]
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

		/// <summary>
		/// アーティファクト同士の階層構造とする場合の親ノード
		/// </summary>
		public virtual Tag ParentTag
		{
			get
			{ return _ParentTag; }
			set
			{
				if (_ParentTag == value)
					return;
				_ParentTag = value;
			}
		}

		#endregion プロパティ
	}
}
