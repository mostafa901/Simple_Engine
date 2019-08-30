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


		public MainWindow()
		{
			InitializeComponent();


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

			GLWindow.Loaded += delegate
			  {
				  var c = Color4.FromHsv(new Vector4(0, 0, 0.3f, 0.5f));
				  GL.ClearColor(c);//set background color
				  
				  var builderConfig = new QFontBuilderConfiguration(true)
				  {
					  ShadowConfig =
				{
					BlurRadius = 2,
					BlurPasses = 1,
					Type = ShadowType.Blurred
				},
					  TextGenerationRenderHint = TextGenerationRenderHint.ClearTypeGridFit,
					  Characters = CharacterSet.General | CharacterSet.Japanese | CharacterSet.Thai | CharacterSet.Cyrillic
				  };

				  _myFont = new QFont("Fonts/! pepsi !.ttf", 72, builderConfig);
				  _myFont2 = new QFont("Fonts/Onyx.ttf", 72, builderConfig);
				  _drawing = new QFontDrawing();
				  _controlsDrawing = new QFontDrawing();
				  _controlsTextOpts = new QFontRenderOptions { Colour = Color.FromArgb(new Color4(0.8f, 0.1f, 0.1f, 1.0f).ToArgb()), DropShadowActive = true };
				  GL.Enable(EnableCap.CullFace);
				  GL.FrontFace(FrontFaceDirection.Ccw);
				  GL.CullFace(CullFaceMode.Back); //set which face to be hidden            
				  GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill); //set polygon draw mode
				  GL.Enable(EnableCap.DepthTest);

				  Program.pipe = new Program.Pipelinevars();

				  var cub = new Cube();
				  cub.LoadGeometry();
				  //cub.shader = new ObjectColor();
				  cub.shader = new Tex2Frag(new Vector3(1, .2f, .3f));
				  cub.shader.LightSources = LightSource.SetupLights(3);
				  Program.pipe.geos.Add(cub);

			  };
		}


		private void MainWindow_MouseMove(object sender, MouseEventArgs e)
		{
			Program.pipe.cam.FirstPerson(e);
		}


		TimeSpan elapsedTime;
		private QFont _myFont;
		private QFont _myFont2;
		private QFontDrawing _drawing;
		private QFontDrawing _controlsDrawing;
		private QFontRenderOptions _controlsTextOpts;
		float yOffset = 0;
		private void OpenTkControl_OnRender(TimeSpan _elapsedTime)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			elapsedTime = _elapsedTime;
#if true

			//Program.pipe.cam.updateCamera();

			foreach (var geo in Program.pipe.geos)
			{
				geo.RenderGeometry();
				GL.DrawElements(geo.primitiveType, geo.Indeces.Count, DrawElementsType.UnsignedInt, 0);
			}

#if false

			_drawing.DrawingPrimitives.Clear();
			_drawing.Print(_myFont, "text1", new Vector3(0), QFontAlignment.Left);

			// draw with options
			var textOpts = new QFontRenderOptions()
			{
				Colour = Color.FromArgb(new Color4(0.8f, 0.1f, 0.1f, 1.0f).ToArgb()),
				DropShadowActive = true
			};
			SizeF size = _drawing.Print(_myFont, "text2", new Vector3(.5f, .5f, .5f), QFontAlignment.Left, textOpts);

			var dp = new QFontDrawingPrimitive(_myFont2);
			size = dp.Print("tx2", new Vector3(0, (float)Height - yOffset, 0), new SizeF(100f, float.MaxValue), QFontAlignment.Left);
			_drawing.DrawingPrimitives.Add(dp);

			// after all changes do update buffer data and extend it's size if needed.
			_drawing.RefreshBuffers();
			_drawing.Draw(); 
#endif
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
			Program.pipe.cam.WheelControl(e);
		}

		private void Win_KeyDown(object sender, KeyEventArgs e)
		{
			Program.pipe.cam.Control(e, elapsedTime);
		}
		#endregion
	}
}
