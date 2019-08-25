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
			WindowStyle = WindowStyle.None;
			Background = Brushes.Transparent;
			MouseLeftButtonDown += delegate
			{
				DragMove();
			};

			PreviewMouseWheel += Win_MouseWheel;
			KeyDown += Win_KeyDown;

			AllowsTransparency = true;
			var settings = new GLWpfControlSettings();
			settings.MajorVersion = 3;
			settings.MinorVersion = 6;

			GLWindow.Start(settings);

			GLWindow.Loaded += delegate
			  {
				  var c = Color4.FromHsv(new Vector4(0, 0, 0.3f, 0));
				  GL.ClearColor(c);//set background color

				  GL.Enable(EnableCap.CullFace);
				  GL.FrontFace(FrontFaceDirection.Ccw);
				  GL.CullFace(CullFaceMode.Back); //set which face to be hidden            
				  GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill); //set polygon draw mode
				  GL.Enable(EnableCap.DepthTest);

				  Program.pipe = new Program.Pipelinevars();
				  Program.pipe.cam.Position += new Vector3(3, 10, 5);
				  var cub = new Cube();
				  cub.LoadGeometry();
				  //cub.shader = new ObjectColor();
				  cub.shader = new Tex2Frag(new Vector3(1, .2f, .3f));
				  cub.shader.LightSources = LightSource.SetupLights();
				  Program.pipe.geos.Add(cub);

			  };
		}

		 

		private void OpenTkControl_OnRender(TimeSpan _elapsedTime)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

#if true
			foreach (var geo in Program.pipe.geos)
			{
				Program.pipe.cam.updateCamera();
				geo.RenderGeometry();

				GL.DrawElements(PrimitiveType.Triangles, geo.Indeces.Count, DrawElementsType.UnsignedInt, 0);
			}
			GL.Finish();
#endif

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
			if (Keyboard.IsKeyDown(Key.LeftShift))
			{
				Program.pipe.cam.Target += new Vector3(0, e.Delta, 0);
			}
			else
			{
				Program.pipe.cam.Fov(e.Delta);
			}
			Program.pipe.cam.updateCamera();
		}

		private void Win_KeyDown(object sender, KeyEventArgs e)
		{
			var win = (MainWindow)sender;

			Hangle = Math.Atan2(Program.pipe.cam.Position.X, Program.pipe.cam.Position.Z);
			Vangle = Math.Atan2(Program.pipe.cam.Position.Z, Program.pipe.cam.Position.Y);

			if (e.Key == Key.V)
			{
				Program.pipe.geos.ForEach(o => o.shader.IsBlin = !o.shader.IsBlin);
			}

			if (e.Key == Key.Z || e.Key == Key.X)
			{

				if (e.Key == Key.Z)
				{
					Program.pipe.geos.ForEach(o => o.shader.specintens -= 1f);

				}
				else
				{
					Program.pipe.geos.ForEach(o => o.shader.specintens += 1f);

				}
			}

			if (e.Key == Key.Right)
			{

				var oldr = Program.pipe.cam.Position.Length;
				Program.pipe.cam.Target += new Vector3(.5f, 0f, 0); ;
				r += Program.pipe.cam.Position.Length - r;
			}
			if (e.Key == Key.Left)
			{
				var oldr = Program.pipe.cam.Position.Length;
				Program.pipe.cam.Target += new Vector3(-.5f, 0f, 0); ;
				r += Program.pipe.cam.Position.Length - r;
			}

			if (e.Key == Key.Down)
			{
				Program.pipe.cam.Target += new Vector3(0, -.5f, 0);
			}

			if (e.Key == Key.Up)
			{
				Program.pipe.cam.Target += new Vector3(0, .5f, 0);
			}

			if (e.Key == Key.Escape)
			{
				((GameWindow)sender).Close();
			}

			if (e.Key == Key.W)
			{
				Vangle += inrement;

				Program.pipe.cam.Position = new Vector3(Program.pipe.cam.Position.X, (float)Math.Cos(Vangle) * r, (float)Math.Sin(Vangle) * r);

			}

			if (e.Key == Key.S)
			{
				Vangle -= inrement;

				Program.pipe.cam.Position = new Vector3(Program.pipe.cam.Position.X, (float)Math.Cos(Vangle) * r, (float)Math.Sin(Vangle) * r);

			}

			if (e.Key == Key.A)
			{
				Hangle -= inrement;

				Program.pipe.cam.Position = new Vector3((float)Math.Sin(Hangle) * r, Program.pipe.cam.Position.Y, (float)Math.Cos(Hangle) * r);

			}

			if (e.Key == Key.D)
			{
				Hangle += inrement;

				Program.pipe.cam.Position = new Vector3((float)Math.Sin(Hangle) * r, Program.pipe.cam.Position.Y, (float)Math.Cos(Hangle) * r);
			}

			var mouse = OpenTK.Input.Mouse.GetState();
			if (e.Key == Key.LeftCtrl && win.IsFocused)
			{
				var dx = mouse.X / 800f - oldx;
				var dy = mouse.Y / 800f - oldy;

				 
				Program.pipe.cam.Target += new Vector3(dx, dy, 0);

				oldx = mouse.X / 800f;
				oldy = mouse.Y / 800f;
			}
			else
			{
				 
				//  cam.Target = Vector3.Zero;
			}

			Program.pipe.cam.updateCamera();


		}

		static float oldx = 0;
		static float oldy = 0;

		static float r = 5f;
		static float inrement = 0.1744f;
		static double Hangle = 0;
		static double Vangle = 0;
		#endregion
	}
}
