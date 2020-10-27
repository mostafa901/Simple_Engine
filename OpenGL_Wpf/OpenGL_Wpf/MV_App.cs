using OpenGL_CSharp;
using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Graphic;
using OpenGL_CSharp.Shaders;
using OpenGL_Wpf.ViewControls;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utility.MVVM;

namespace OpenGL_Wpf
{
	public class MV_App : BaseVModel
	{


		#region Geos

		private ObservableRangeCollection<BaseGeometry> _Geos;

		public ObservableRangeCollection<BaseGeometry> Geos
		{
			get
			{
				if (_Geos == null) _Geos = new ObservableRangeCollection<BaseGeometry>();
				return _Geos;
			}
			set { SetProperty(ref _Geos, value); }

		}
		#endregion

		#region ViewCam

		private Camera _ViewCam;

		public Camera ViewCam
		{
			get
			{
				return _ViewCam;
			}
			set { SetProperty(ref _ViewCam, value); }

		}
		#endregion

		public MV_App()
		{
		}
		
		#region SelectedItem

		private BaseGeometry _SelectedItem;

		public BaseGeometry SelectedItem
		{
			get
			{
				return _SelectedItem;
			}
			set
			{			 
				SetProperty(ref _SelectedItem, value); 
			}
		}

	}
	#endregion

}
