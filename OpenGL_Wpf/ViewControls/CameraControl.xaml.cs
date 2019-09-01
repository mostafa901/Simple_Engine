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

namespace OpenGL_Wpf.ViewControls
{
	/// <summary>
	/// Interaction logic for CameraControl.xaml
	/// </summary>
	public partial class CameraControl : UserControl
	{
		public CameraControl()
		{
			InitializeComponent();			
		}

		private void bt_ActivateCamera(object sender, RoutedEventArgs e)
		{
			MainWindow.mv.ViewCam = DataContext as Camera;
			MainWindow.mv.ViewCam.UpdateDistance();
			MainWindow.mv.ViewCam.updateCamera();
		}
	}
}
