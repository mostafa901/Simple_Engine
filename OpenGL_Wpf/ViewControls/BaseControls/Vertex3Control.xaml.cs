using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using OpenGL_CSharp.Graphic;
using System.Diagnostics;

namespace OpenGL_Wpf.ViewControls.BaseControls
{
	/// <summary>
	/// Interaction logic for Vertex3Control.xaml
	/// </summary>
	public partial class Vertex3Control : UserControl
	{
		public Vertex3Control()
		{
			InitializeComponent();
			txtname.DataContext = this;
		}

		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			//Regex regex = new Regex("[^0-9]+.-");
			//e.Handled = regex.IsMatch(e.Text);
		}

		#region DescName
		public static readonly DependencyProperty DescNameprop = DependencyProperty.Register(nameof(DescName), typeof(string), typeof(Vertex3Control));


		public string DescName
		{
			get
			{
				return (string)GetValue(DescNameprop);
			}
			set { SetValue(DescNameprop, value); }

		}



		#endregion

		private void AddValue(object sender, MouseWheelEventArgs e)
		{

			var txb = sender as TextBox;
			if (txb == null)
			{
				e.Handled = true;
			}
			else
			{
				e.Handled = false;
				float v = 0;
				float.TryParse(txb.Text, out v);
				var ver = txb.DataContext as Vertex3;
				BindingExpression txtprop = BindingOperations.GetBindingExpression(txb, ((DependencyProperty)TextBox.TextProperty));
				string Name = txtprop.ParentBinding.Path.Path;
				ver.InjectPropertyValue(Name, v + e.Delta * 0.025f);

			}
		}
	}

	public static partial class Utility
	{
		#region PropertyInfo

		public static void InjectPropertyValue(this object obj, string propname, object value)
		{
			try
			{
				if (value == null) return;

				var props = obj.GetType().GetProperties();
				var p = props.Where(o => o.Name == propname).First();
				if (p.PropertyType == typeof(int))
				{
					int intval;
					if (int.TryParse(value.ToString(), out intval))
						p.SetValue(obj, intval);
				}
				else if (p.PropertyType == typeof(double))
				{
					double intval;
					if (double.TryParse(value.ToString(), out intval))
						p.SetValue(obj, intval);
				}
				else if (p.PropertyType == typeof(float))
				{
					float intval;
					if (float.TryParse(value.ToString(), out intval))
						p.SetValue(obj, intval);
				}
				else if (p.PropertyType == typeof(DateTime))
				{
					p.SetValue(obj, DateTime.Parse(value.ToString()));
				}
				else p.SetValue(obj, value);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error Injecting Value to object.\r\n"+ex.ToString());
			}
		}

		#endregion PropertyInfo
	}
}
