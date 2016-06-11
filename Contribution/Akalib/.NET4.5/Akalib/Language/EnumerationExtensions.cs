using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Language
{
	public static class EnumerationExtensions
	{
		public static bool Has<T>(this System.Enum e, T value)
		{
			try
			{
				return (((int)(object)e & (int)(object)value) == (int)(object)value);
			}
			catch
			{
				return false;
			}
		}
		public static T Set<T>(this System.Enum e, T value)
		{
			try
			{
				return (T)(object)(((int)(object)e | (int)(object)value));
			}
			catch (Exception)
			{
				throw new ArgumentException();
			}
		}

		public static T Clr<T>(this System.Enum e, T value)
		{
			try
			{
				return (T)(object)(((int)(object)e & ~(int)(object)value));
			}
			catch (Exception)
			{
				throw new ArgumentException();
			}
		}
	}
}
