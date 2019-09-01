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
using Utility.IO;

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
				BindingExpression be = BindingOperations.GetBindingExpression(txb, ((DependencyProperty)TextBox.TextProperty));
				string Name = be.ParentBinding.Path.Path;
				ver.InjectPropertyValue(Name, v + e.Delta * 0.025f);

			}
		}
	}
}
