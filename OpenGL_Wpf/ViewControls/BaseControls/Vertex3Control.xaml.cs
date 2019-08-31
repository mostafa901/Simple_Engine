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


	 

	}
}
