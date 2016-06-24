using Livet;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Core.Constractures
{
	public sealed class ApplicationSettingInfo : NotificationObject
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ApplicationSettingInfo));

		private DateTime? _LastUpdate;

		private Point _WindowLocation;

		private Size _WindowSize;

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastUpdate
		{
			get
			{ return _LastUpdate; }
			set
			{
				if (_LastUpdate == value)
					return;
				_LastUpdate = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// アプリケーションウィンドウの位置を取得、または設定します。
		/// </summary>
		/// <remarks>
		/// 値が0の場合は初期値ですので、任意の位置で修正して下さい。
		/// </remarks>
		public Point WindowLocation
		{
			get
			{ return _WindowLocation; }
			set
			{
				if (_WindowLocation == value)
					return;
				_WindowLocation = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// アプリケーションウィンドウのサイズを取得、または設定します。
		/// </summary>
		/// <remarks>
		/// 値が0の場合は初期値ですので、任意のサイズで修正して下さい。
		/// </remarks>
		public Size WindowSize
		{
			get
			{ return _WindowSize; }
			set
			{
				if (_WindowSize == value)
					return;
				_WindowSize = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// ファイルから設定を読み込みます
		/// </summary>
		/// <param name="sr"></param>
		public void Load(StreamReader sr)
		{
			try
			{
				// オブジェクトの状態をマージします。
				// コレクションの中身もマージするので注意してください。
				JsonConvert.PopulateObject(sr.ReadToEnd(), this);
			}
			catch (JsonSerializationException expr)
			{
				LOG.Warn(expr.Message);
				Reset();
			}

		}

		/// <summary>
		/// デフォルトの値を読み込みます
		/// </summary>
		public void Reset()
		{
			WindowSize = new Size(0, 0);
			WindowLocation = new Point(0, 0);
		}
		/// <summary>
		/// ファイルに出力します
		/// </summary>
		/// <param name="sw"></param>
		public void Save(StreamWriter sw)
		{
			LastUpdate = DateTime.Now;

			string json = JsonConvert.SerializeObject(this, Formatting.Indented);
			sw.Write(json);
		}

		#endregion メソッド

	}
}
