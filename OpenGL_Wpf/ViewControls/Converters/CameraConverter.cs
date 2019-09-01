using OpenGL_CSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OpenGL_Wpf.ViewControls.Converters
{
	class CameraConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(parameter.ToString() == nameof(Visibility))
			{
				if (value is Camera)
				{
					return Visibility.Visible;
				}
				else return Visibility.Collapsed;
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
