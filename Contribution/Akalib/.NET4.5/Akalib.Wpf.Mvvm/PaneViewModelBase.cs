using Akalib.Wpf.Dock;
using Akalib.Wpf.Mvvm.Attribute;
using Livet;
using Livet.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Akalib.Wpf.Mvvm
{
	/// <summary>
	/// AvalonDockを使用したMVVMアーキテクチャで、ペインコンテナのViewModelクラスの基本クラスとして使用します。
	/// </summary>
	public abstract class PaneViewModelBase : ViewModel, IAnchorPaneViewModel
	{
		#region Private Fields

		private IPropertyPaneItem _ActiveContent;

		private ViewModelCommand _HideCommand;

		private Visibility m_visiblity;

		private string _Title;

		private string _ContentId;

		private string _ToolTip;

		/// <summary>
		/// Viewに表示するコンテンツテンプレートです
		/// </summary>
		private DataTemplate _ViewTemplate;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// プロパティペインに表示する表示内容を設定します。
		/// </summary>
		/// <remarks>
		/// Nullをプロパティに与えた場合は、プロパティペインには何も表示しません。
		/// </remarks>
		public IPropertyPaneItem ActiveContent
		{
			get { return _ActiveContent; }
			set
			{
				if (Equals(_ActiveContent, value)) return;
				_ActiveContent = value;
				RaisePropertyChanged(() => ActiveContent);

				OnActiveContentChanged();
			}
		}

		[ContentProperty]
		public string ContentId
		{
			get { return _ContentId; }
			set { _ContentId = value; }
		}

		public ViewModelCommand HideCommand
		{
			get
			{
				if (_HideCommand == null)
				{
					_HideCommand = new ViewModelCommand(Close);
				}
				return _HideCommand;
			}
		}

		[ContentProperty]
		public ImageSource IconSource
		{
			get;
			set;
		}

		/*
		[ContentProperty(BindingMode = BindingMode.TwoWay)]
		public bool IsActive
		{
			get
			{ return _IsActive; }
			set
			{
				if (_IsActive == value)
					return;
				_IsActive = value;
				RaisePropertyChanged();
			}
		}

		[ContentProperty(BindingMode = BindingMode.TwoWay)]
		public bool IsSelected
		{
			get
			{ return _IsSelected; }
			set
			{
				if (_IsSelected == value)
					return;
				_IsSelected = value;
				RaisePropertyChanged();
			}
		}
		*/


		[ContentProperty(BindingMode = BindingMode.TwoWay)]
		public Visibility Visibility
		{
			get { return m_visiblity; }
			set
			{
				if (m_visiblity == value) return;
				m_visiblity = value;
				RaisePropertyChanged("Visibility");
				RaisePropertyChanged("IsVisible");
			}
		}

		public bool IsVisible
		{
			get { return m_visiblity == System.Windows.Visibility.Visible; }
			set
			{
				if (IsVisible == value) return;
				if (value)
				{
					Visibility = System.Windows.Visibility.Visible;
				}
				else
				{
					Visibility = System.Windows.Visibility.Hidden;
				}
			}
		}

		[ContentProperty]
		public string Title
		{
			get { return this._Title; }
			set
			{
				if (Equals(_Title, value)) return;
				this._Title = value;
				RaisePropertyChanged(() => Title);
			}
		}

		[ContentProperty]
		public string ToolTip
		{
			get { return _ToolTip; }
			set
			{
				if (Equals(_ToolTip, value)) return;
				this._ToolTip = value;
			}
		}

		public DataTemplate ViewTemplate
		{
			get
			{
				return _ViewTemplate;
			}
			protected set
			{
				_ViewTemplate = value;
				RaisePropertyChanged(() => ViewTemplate);
			}
		}

		#endregion Public Properties

		#region Public Methods

		public void ActiveChanged()
		{
		}

		public void Close()
		{
			this.IsVisible = false;
		}

		/// <summary>
		///     ペインコンテナの表示状態をトグルしアクティブにする
		/// </summary>
		public void ToggleVisibilityAndActive()
		{
			if (this.IsVisible)
			{
				//this.IsActive = true;
			}
		}

		#endregion Public Methods

		#region Protected Methods

		protected virtual void OnActiveContentChanged()
		{
		}

		protected virtual void OnVisiblePropertyChanged()
		{
		}

		#endregion Protected Methods
	}
}
