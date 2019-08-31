using OpenGL_CSharp;
using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Shaders;
using OpenGL_CSharp.Shaders.Light;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using QuickFont;
using QuickFont.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using Color = System.Drawing.Color;

namespace OpenGL_Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public static MV_App mv;
		public MainWindow()
		{
			InitializeComponent();
			Name = "MainWindow";

			WindowStyle = WindowStyle.None;
			Background = System.Windows.Media.Brushes.Transparent;
			WindowState = System.Windows.WindowState.Maximized;
			MouseLeftButtonDown += delegate
			{
				DragMove();
			};

			PreviewMouseWheel += Win_MouseWheel;
			MouseMove += MainWindow_MouseMove;
			KeyDown += Win_KeyDown;

			AllowsTransparency = true;
			var settings = new GLWpfControlSettings();
			settings.MajorVersion = 3;
			settings.MinorVersion = 6;

			GLWindow.Start(settings);
			GLWindow.PreviewMouseLeftButtonDown += delegate
			{
				GLWindow.Focus();
			};

			GLWindow.Loaded += delegate
			  {
				  DataContext = mv = new MV_App();

				  var c = Color4.FromHsv(new Vector4(0, 0, 0.3f, 0.5f));
				  GL.ClearColor(c);//set background color				 
				  GL.Enable(EnableCap.CullFace);
				  GL.FrontFace(FrontFaceDirection.Ccw);
				  GL.CullFace(CullFaceMode.Back); //set which face to be hidden            
				  GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill); //set polygon draw mode
				  GL.Enable(EnableCap.DepthTest);

				  mv.ViewCam = new Camera("Camera 01");
				  new Camera("Camera 02");


				  var cub = new Cube();
				  cub.LoadGeometry();
				  //cub.shader = new ObjectColor();
				  cub.shader = new Tex2Frag(new Vector3(1, .2f, .3f));
				  cub.shader.LightSources = LightSource.SetupLights(1);
				  
			  };
		}


		private void MainWindow_MouseMove(object sender, MouseEventArgs e)
		{
			mv.ViewCam.FirstPerson(e);
		}


		TimeSpan elapsedTime;

		private void OpenTkControl_OnRender(TimeSpan _elapsedTime)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			elapsedTime = _elapsedTime;
			
			foreach (var geo in mv.Geos)
			{
				if (!geo.ShowModel) continue;
				geo.RenderGeometry();
				GL.DrawElements(geo.primitiveType, geo.Indeces.Count, DrawElementsType.UnsignedInt, 0);
			}

			GL.Finish();


#if false
			GL.LoadIdentity();
			GL.Begin(PrimitiveType.Triangles);
			GL.Color4(1.0f, 0.0f, 0.0f, 1.0f); GL.Vertex2(0.0f, 1.0f);
			GL.Color4(0.0f, 1.0f, 0.0f, 1.0f); GL.Vertex2(0.87f, -0.5f);
			GL.Color4(0.0f, 0.0f, 1.0f, 1.0f); GL.Vertex2(-0.87f, -0.5f);
			GL.End();
			GL.Finish(); 
#endif

		}


		#region Navigation
		private void Win_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			mv.ViewCam.WheelControl(e);
		}

		private void Win_KeyDown(object sender, KeyEventArgs e)
		{
			mv.ViewCam.Control(e, elapsedTime);
		}
		#endregion
	}
}
