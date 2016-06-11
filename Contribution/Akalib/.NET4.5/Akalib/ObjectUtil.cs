using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Akalib
{
	public static class ObjectUtil
	{
		/// <summary>
		/// 2つのオブジェクトの値をマージする
		/// </summary>
		/// <remarks>
		/// src → destへコピー
		/// </remarks>
		/// <param name="srcClazz"></param>
		/// <param name="src"></param>
		/// <param name="destClazz"></param>
		/// <param name="dest"></param>
		static public void marge(Type srcClazz, object src, Type destClazz, object dest)
		{
			PropertyInfo[] fields = srcClazz.GetProperties();
			foreach (PropertyInfo f in fields)
			{
				//System.Console.WriteLine("Property Name=" + f.Name);

				PropertyInfo allowSetField = destClazz.GetProperty(f.Name);

				object srcValue = f.GetGetMethod().Invoke(src, null);
				//System.Console.WriteLine(String.Format("Name={0} Value={1}",f.Name,srcValue));

				allowSetField.SetValue(dest, srcValue, null);
			}

		}
	}
}
