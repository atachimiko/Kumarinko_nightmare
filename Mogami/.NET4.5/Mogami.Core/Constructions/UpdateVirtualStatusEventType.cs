using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Constructions
{
	/// <summary>
	/// UpdateVirtualSpaceFlowで使用する、更新イベントの種類
	/// </summary>
	public enum UpdateVirtualStatusEventType
	{
		UPDATE,
		RENAME,
		DELETE
	}
}
