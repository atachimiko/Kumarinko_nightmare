using Mogami.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	[Table("alWorkspace")]
	public class Workspace : ServiceModel, IWorkspace
	{


		#region フィールド

		private string _Name;
		private string _PhysicalPath;
		private string _WorkspacePath;

		#endregion フィールド


		#region プロパティ

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

		public string PhysicalPath
		{
			get
			{ return _PhysicalPath; }
			set
			{
				if (_PhysicalPath == value)
					return;
				_PhysicalPath = value;
			}
		}

		public string WorkspacePath
		{
			get
			{ return _WorkspacePath; }
			set
			{
				if (_WorkspacePath == value)
					return;
				_WorkspacePath = value;
			}
		}

		#endregion プロパティ

	}
}
