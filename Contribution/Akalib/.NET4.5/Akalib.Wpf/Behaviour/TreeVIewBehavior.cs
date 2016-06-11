using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Akalib.Wpf.Behaviour
{
	/// <summary>
	///     TreeVIewのビヘイビア
	/// </summary>
	/// <remarks>
	/// 実装済みのビヘイビア
	///
	/// ◆ SelectedItem
	///     TreeView.SelectedItemプロパティのバインディングを行うためのビヘイビア
	///     <TreeView>
	///       <e:Interaction.Behaviors>
	///         <behaviours:BindableSelectedItemBehavior SelectedItem="{Binding SelectedItem, Mode=TwoWay}" />
	///       </e:Interaction.Behaviors>
	///     </TreeView>
	/// </remarks>
	public class TreeViewBehavior : Behavior<TreeView>
	{
		#region SelectedItem Property

		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register("SelectedItem", typeof(object), typeof(TreeViewBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));

		private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var item = e.NewValue as TreeViewItem;
			if (item != null)
			{
				item.SetValue(TreeViewItem.IsSelectedProperty, true);
			}
		}

		#endregion SelectedItem Property

		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			if (this.AssociatedObject != null)
			{
				this.AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
			}
		}

		private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			this.SelectedItem = e.NewValue;
		}
	}
}