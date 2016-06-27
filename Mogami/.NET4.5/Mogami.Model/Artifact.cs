using Livet;
using Mogami.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	[Table("alArtifact")]
	public class Artifact : ServiceModel, IArtifact
	{

		#region フィールド

		private FileMappingInfo _FileMappingInfo;

		private string _IdentifyKey;

		private IList<Tag> _Tags;

		private string _ThumbnailKey;

		private string _Title;

		#endregion フィールド


		#region コンストラクタ

		public Artifact()
		{
			this.Tags = new ObservableSynchronizedCollection<Tag>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public virtual FileMappingInfo FileMappingInfo
		{
			get { return _FileMappingInfo; }
			set { _FileMappingInfo = value; }
		}

		public string IdentifyKey
		{
			get { return _IdentifyKey; }
			set
			{
				_IdentifyKey = value;
			}
		}

		public virtual IList<Tag> Tags
		{
			get
			{ return _Tags; }
			set
			{
				if (_Tags == value)
					return;
				_Tags = value;
			}
		}
		public string ThumbnailKey
		{
			get { return _ThumbnailKey; }
			set
			{
				if (_ThumbnailKey == value) return;
				_ThumbnailKey = value;
			}
		}
		public string Title
		{
			get { return _Title; }
			set { _Title = value; }
		}

		#endregion プロパティ

	}
}
