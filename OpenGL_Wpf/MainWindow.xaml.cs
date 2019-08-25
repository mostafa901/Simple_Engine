using OpenGL_CSharp;
using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Shaders;
using OpenGL_CSharp.Shaders.Light;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
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

namespace OpenGL_Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var settings = new GLWpfControlSettings();
			settings.MajorVersion = 3;
			settings.MinorVersion = 3;
			GLWindow.Start(settings);

			GLWindow.Loaded += delegate
			  {
				  GL.Viewport(0, 0, 800, 800);
				  var c = Color4.FromHsv(new Vector4(0.1f, 1f, 0.9999f, 1));
				  GL.ClearColor(c);//set background color
				  GL.Enable(EnableCap.CullFace);
				  GL.FrontFace(FrontFaceDirection.Ccw);
				  GL.CullFace(CullFaceMode.Back); //set which face to be hidden            
				  GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill); //set polygon draw mode
				  GL.Enable(EnableCap.DepthTest);
			  };

			pipe = new pip();
			var cub = new Cube();
			cub.LoadGeometry();
			cub.shader = new Tex2Frag(new Vector3(1, .2f, .3f));
			cub.shader.LightSources = LightSource.SetupLights();
		}

		public static pip pipe;
		public class pip
		{
			public List<BaseGeometry> geos = new List<BaseGeometry>();
			public Camera cam = new Camera();

		}

		private void RenderScene(TimeSpan obj)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			foreach (var geo in pipe.geos)
			{

			}

		}
	}
}
