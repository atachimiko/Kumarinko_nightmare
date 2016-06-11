using Livet.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Wpf.Mvvm.Behavior
{
	/// <summary>
	///
	/// </summary>
	public class ParameterCallMethodAction : LivetCallMethodAction
	{
		protected override void Invoke(object parameter)
		{
			_callbackMethod.Invoke(MethodTarget, MethodName, parameter);
		}

		private MethodBinderWithArgument _callbackMethod = new MethodBinderWithArgument();
	}
}