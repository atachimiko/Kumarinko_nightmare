using Akalib.Wpf.Mvvm;
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
	public class EditCategoryDialogViewModel : DialogViewModelBase
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(EditCategoryDialogViewModel));

		#endregion フィールド


		#region メソッド

		protected override bool OnRequestClose()
		{
			return true;
		}

		#endregion メソッド

	}
}
