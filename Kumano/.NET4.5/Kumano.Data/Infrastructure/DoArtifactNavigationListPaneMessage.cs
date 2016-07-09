using Kumano.Data.ViewModel;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Data.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public class DoArtifactNavigationListPaneMessage : InteractionMessage
	{


		#region フィールド

		public static readonly DependencyProperty FindByCategoryIdProperty = DependencyProperty.Register(
			"FindByCategoryId", // プロパティ名を指定
			typeof(long), // プロパティの型を指定
			typeof(DoArtifactNavigationListPaneMessage), // プロパティを所有する型を指定
			new PropertyMetadata(0L));

		public static readonly DependencyProperty FindByTagIdProperty = DependencyProperty.Register(
			"FindByTagId", // プロパティ名を指定
			typeof(long), // プロパティの型を指定
			typeof(DoArtifactNavigationListPaneMessage), // プロパティを所有する型を指定
			new PropertyMetadata(0L));


		#endregion フィールド


		#region コンストラクタ

		public DoArtifactNavigationListPaneMessage()
		{
			this.MessageKey = "DoArtifactNavigationListPane";
		}

		#endregion コンストラクタ

		#region プロパティ

		public long FindByCategoryId
		{
			get { return (long)GetValue(FindByCategoryIdProperty); }
			set { SetValue(FindByCategoryIdProperty, value); }
		}

		public long FindByTagId
		{
			get { return (long)GetValue(FindByTagIdProperty); }
			set { SetValue(FindByTagIdProperty, value); }
		}
		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 派生クラスでは必ずオーバーライドしてください。Freezableオブジェクトとして必要な実装です。<br/>
		/// 通常このメソッドは、自身の新しいインスタンスを返すように実装します。
		/// </summary>
		/// <returns>自身の新しいインスタンス</returns>
		protected override System.Windows.Freezable CreateInstanceCore()
		{
			var cpy = new DoArtifactNavigationListPaneMessage();
			cpy.MessageKey = this.MessageKey;
			return cpy;
		}

		#endregion メソッド

	}
}
