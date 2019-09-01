using OpenGL_CSharp;
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
using Utility.MVVM;

namespace OpenGL_Wpf.ViewControls.BaseControls
{
	/// <summary>
	/// Interaction logic for BoolControl.xaml
	/// </summary>
	public partial class BoolControl : UserControl
	{
		public BoolControl()
		{
			InitializeComponent();
			Desc.DataContext = this;			 
		}


		#region IChecked
		public static readonly DependencyProperty ICheckedprop = DependencyProperty.Register(nameof(IChecked), typeof(bool), typeof(BoolControl));

		public bool IChecked
		{
			get
			{
				return (bool)GetValue(ICheckedprop);
			}
			set { SetValue(ICheckedprop, value); }

		}
		#endregion


		#region ICMD
		public static readonly DependencyProperty ICMDprop = DependencyProperty.Register(nameof(ICMD), typeof(cus_CMD), typeof(BoolControl));


		public cus_CMD ICMD
		{
			get
			{
				return (cus_CMD)GetValue(ICMDprop);
			}
			set { SetValue(ICMDprop, value); }

		}
		#endregion



		#region DescName
		public static readonly DependencyProperty DescNameprop = DependencyProperty.Register(nameof(DescName), typeof(string), typeof(BoolControl));


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
