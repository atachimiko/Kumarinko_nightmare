using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Model
{
	public interface IArtifact
	{

		#region プロパティ

		string IdentifyKey { get; }
		string Title { get; }

		#endregion プロパティ
	}
}
