﻿using Livet;
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

		private IList<Tag> _ChildTags;

		private string _Name;

		private Tag _ParentTag;

		#endregion フィールド


		#region コンストラクタ

		public Tag()
		{
			this.Artifacts = new ObservableSynchronizedCollection<Artifact>();

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