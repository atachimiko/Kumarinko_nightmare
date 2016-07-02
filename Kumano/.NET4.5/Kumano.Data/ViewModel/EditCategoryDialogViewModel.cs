using Akalib.Wpf.Mvvm;
using Kumano.Contrib.Infrastructures;
using Kumano.Data.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.ViewModel
{
	/// <summary>
	/// EditCategoryDialog
	/// </summary>
	public class EditCategoryDialogViewModel : DialogViewModelBase, IDataImport<DataCategory>
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(EditCategoryDialogViewModel));

		private DataCategory _Category;

		#endregion フィールド


		#region コンストラクタ

		public EditCategoryDialogViewModel()
		{
		}

		#endregion コンストラクタ


		#region プロパティ

		public DataCategory Category
		{
			get { return _Category; }
			set
			{
				if (_Category == value) return;
				_Category = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ


		#region メソッド

		public void Import(IDataExport<DataCategory> exporter)
		{
			var immobj = exporter.Export();
			this.Category = immobj;
		}

		protected override bool OnRequestClose()
		{
			try
			{
				using (var proxy = new MogamiApiServiceClient())
				{
					var req = new REQUEST_UPDATECATEGORY();
					var rsp = proxy.UpdateCategory(req);
					if (!rsp.Success)
					{
						// Fault
						LOG.Warn("カテゴリ情報の更新に失敗しました。");
					}
				}
			}
			catch (Exception expr)
			{
				LOG.Error(expr.Message);
			}

			return true;
		}

		#endregion メソッド

	}
}
