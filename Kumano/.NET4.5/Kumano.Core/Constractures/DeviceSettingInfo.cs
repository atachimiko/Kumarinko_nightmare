using Kumano.Core.Constractures.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Core.Constractures
{
	/// <summary>
	/// タグ入力ボタン
	/// </summary>
	public sealed class AttachTagButtonItem
	{


		#region プロパティ

		public AttachTagButtonModeType ButtonMode { get; set; }
		public string Label { get; set; }
		public long TagId { get; set; }

		#endregion プロパティ

	}

	public sealed class AttachTagGroupItem
	{


		#region コンストラクタ

		public AttachTagGroupItem()
		{
			this.Buttons = new List<AttachTagButtonItem>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public List<AttachTagButtonItem> Buttons { get; private set; }
		public string GroupName { get; set; }

		#endregion プロパティ

	}

	public sealed class DeviceSettingInfo
	{

		#region コンストラクタ

		public DeviceSettingInfo()
		{
			this.AttachTagGroups = new List<AttachTagGroupItem>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public List<AttachTagGroupItem> AttachTagGroups { get; private set; }

		#endregion プロパティ
	}
}
