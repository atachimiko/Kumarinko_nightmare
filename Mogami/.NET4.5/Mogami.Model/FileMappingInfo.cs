using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	[Table("alFileMappingInfo")]
	public class FileMappingInfo : ServiceModel
	{


		#region フィールド

		private string _AclHash;

		private string _MappingFilePath;

		private string _Mimetype;

		private Workspace _Workspace;

		#endregion フィールド


		#region プロパティ

		public string AclHash
		{
			get
			{ return _AclHash; }
			set
			{
				if (_AclHash == value)
					return;
				_AclHash = value;
			}
		}

		public string MappingFilePath
		{
			get
			{ return _MappingFilePath; }
			set
			{
				if (_MappingFilePath == value)
					return;
				_MappingFilePath = value;
			}
		}

		public string Mimetype
		{
			get { return _Mimetype; }
			set
			{
				if (_Mimetype == value) return;
				_Mimetype = value;
			}
		}
		public virtual Workspace Workspace
		{
			get
			{ return _Workspace; }
			set
			{
				if (_Workspace == value)
					return;
				_Workspace = value;
			}
		}

		#endregion プロパティ

	}
}

