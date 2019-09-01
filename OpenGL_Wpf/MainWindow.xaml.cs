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
		public static MainWindow Instance;
		public MainWindow()
		{
			InitializeComponent();
			Name = "MainWindow";

			Instance = this;

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
				  GL.Enable(EnableCap.StencilTest);
				  GL.DepthFunc(DepthFunction.Less);//this is the default value and can be ignored.
					GL.StencilFunc(StencilFunction.Notequal, 1, 1);
				  GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

				  mv.ViewCam = new Camera("Camera 01");
				  mv.ViewCam.Position.Update(new Vector3(4, 7, 8));
				  mv.ViewCam.UpdateDirections();
				  mv.ViewCam.updateCamera();
				  new Camera("Camera 02");

				  //setup global project lights
				  BaseShader.LightSources = LightSource.SetupLights(1);

				  for (int i = 0; i < 20; i++)
				  {
					  var cub = new Cube();
					  cub.Name += i;
					  cub.model *= Matrix4.CreateTranslation(0, 0, i * 3);
					  cub.LoadGeometry();
					  cub.ShowModel = true;
					  //cub.shader = new ObjectColor();
					  cub.shader = new Tex2Frag(new Vector3(1, .2f, .3f));

				  }
#if false
				  var plan = new Plan();
				  plan.model *= Matrix4.CreateScale(3);
				  plan.LoadGeometry();
				  plan.ShowModel = true;
				  plan.shader = new Tex2Frag(new Vector3(0, 1f, .3f)); 
#endif


			  };
		}


		private void MainWindow_MouseMove(object sender, MouseEventArgs e)
		{
			mv.ViewCam.FirstPerson(e);
		}


		TimeSpan elapsedTime;

		private void OpenTkControl_OnRender(TimeSpan _elapsedTime)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
			elapsedTime = _elapsedTime;

			foreach (var geo in mv.Geos)
			{
				if (!geo.ShowModel) continue;
				
				if (geo is LightSource)
				{
					//update light positions
					geo.LoadGeometry();

				}


				if (geo.Name == "Cube1")
				{

					//now scale the cube a bit and redraw the stensil with the colot
					GL.StencilFunc(StencilFunction.Notequal, 1, 1);
					GL.StencilMask(0);
					GL.Disable(EnableCap.DepthTest);
					geo.shader.SetInt($"SelectionMode", 1);
					var originalmat = geo.model;

					geo.model *= Matrix4.CreateTranslation(-originalmat.ExtractTranslation()) * Matrix4.CreateScale(1.01f) * Matrix4.CreateTranslation(originalmat.ExtractTranslation());

					geo.RenderGeometry();
					GL.FrontFace(geo.FaceDirection);
					GL.DrawElements(geo.primitiveType, geo.Indeces.Count, DrawElementsType.UnsignedInt, 0);
					GL.StencilMask(1);
					GL.Enable(EnableCap.DepthTest);
					geo.model *= Matrix4.CreateTranslation(-originalmat.ExtractTranslation())*Matrix4.CreateScale(1/1.01f)* Matrix4.CreateTranslation(originalmat.ExtractTranslation());

					//draw normal cube and record the sensil buffer as 1.
					GL.StencilMask(0xFF);
					GL.StencilFunc(StencilFunction.Always, 1, 1);					 
					geo.shader.SetInt($"SelectionMode", 0);

					geo.RenderGeometry();
					GL.FrontFace(geo.FaceDirection);
					GL.DrawElements(geo.primitiveType, geo.Indeces.Count, DrawElementsType.UnsignedInt, 0);
				}
				else
				{
					geo.RenderGeometry();
					GL.FrontFace(geo.FaceDirection);
					GL.DrawElements(geo.primitiveType, geo.Indeces.Count, DrawElementsType.UnsignedInt, 0);
				}

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
