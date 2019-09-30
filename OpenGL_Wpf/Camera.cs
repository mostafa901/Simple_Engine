using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Graphic;
using OpenGL_CSharp.Shaders;
using OpenGL_Wpf;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
#if RAAPSII
using RAAPSII_APPS.APP_Test.OpenGL; 
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utility;
using Utility.MVVM;

namespace OpenGL_CSharp
{
	public class Camera : BaseGeometry
	{
		//Camera Properties
		public Vector3 Up;
		public Vector3 Direction;
		public Vector3 Right;

		public Matrix4 Matyaw;
		public float yaw;
		public Matrix4 Matpitch;
		public float pitch;
		public Matrix4 View;
		public Matrix4 Projection;
		private float fov = MathHelper.DegreesToRadians(15);
		public float Aspect = 1.0f;
		public float DistanceToTarget = 5f;
		public float MouseSensivity = 0.025f;
		static float oldx = 0;
		static float oldy = 0;
		static float increment = 0.1744f;

		#region Target

		private Vertex3 _Target;

		public Vertex3 Target
		{
			get
			{
				return _Target;
			}
			set { SetProperty(ref _Target, value); }

		}
		#endregion

		#region Position

		private Vertex3 _Position;
		public Vertex3 Position
		{
			get
			{
				return _Position;
			}
			set
			{
				SetProperty(ref _Position, value);

			}
		}
		#endregion


		#region ShowDepth

		private bool _ShowDepth;

		public bool ShowDepth
		{
			get
			{
				return _ShowDepth;
			}
			set { SetProperty(ref _ShowDepth, value); }

		}
		#endregion

		#region CMD_ShowDepth

		private cus_CMD _CMD_ShowDepth;

		public cus_CMD CMD_ShowDepth
		{
			get
			{
				if (_CMD_ShowDepth == null) _CMD_ShowDepth = new cus_CMD();
				return _CMD_ShowDepth;
			}
			set { SetProperty(ref _CMD_ShowDepth, value); }

		}
		#endregion

		public Camera()
		{
			Initialize();
		}

		private void Initialize()
		{
			Target = new Vertex3(0, 0, 0);
			Position = new Vertex3(0, 2, 0);
			Direction = -Vector3.UnitZ;
			Right = Vector3.UnitX;
			Up = Vector3.UnitY;
			UpdateDistance();
			updateCamera();
			Visible = Visibility.Visible;

			objectColor = new Vector3(0, 0, 1);

			Position.PropertyChanged += delegate
			{
				UpdateDirections();
				updateCamera();
			};

			Target.PropertyChanged += delegate
			{
				UpdateDirections();
				updateCamera();
			};

			CMD_ShowDepth.Action = (a) =>
			{
				ShowDepth = (bool)a;
			};
		}
		public Camera(string v)
		{
			Initialize();
			Name = v;
		}

		public void Fov(float value)
		{
			var fdeg = MathHelper.RadiansToDegrees(fov);
			fdeg += value;

			if (fdeg < 1)
			{
				fov = MathHelper.DegreesToRadians(1);
			}
#if true
			else if (fdeg > 180)
			{
				fov = MathHelper.DegreesToRadians(180);

			}

#endif
			else
			{
				fov = MathHelper.DegreesToRadians(fdeg);
			}
		}

		public override void LoadGeometry()
		{
			points = new List<Graphic.Vertex>();
			var Vc = new Vertex4(objectColor.X, objectColor.Y, objectColor.Z, 1);
			points.Clear();

			points.Add(new Graphic.Vertex()
			{
				Position = Position,
				Normal = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Vcolor = new Vertex4(0, 0, 1, 1)
			}); ;

			points.Add(new Graphic.Vertex()
			{
				Position = Vertex3.FromVertex3(Position.vector3 + Direction),
				Normal = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Vcolor = new Vertex4(0, 0, 1, 1)
			});

			//UpVector
			points.Add(new Graphic.Vertex()
			{
				Position = Vertex3.FromVertex3(Position.vector3 + Up),
				Normal = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Vcolor = new Vertex4(0, 1, 0, 1)
			});

			//RightVector
			points.Add(new Graphic.Vertex()
			{
				Position = Vertex3.FromVertex3(Position.vector3 + Right),
				Normal = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Vcolor = new Vertex4(1, 0, 0, 1)
			});


			Indeces = new List<int>
			{
				0, 1, //Target
				0, 2, //UP
				0, 3 //Right
			};
			primitiveType = PrimitiveType.Lines;

			vers = null; //set this to null to update vertex information.						
			base.LoadGeometry();
			shader = new ObjectColor();
		}

		public void UpdateDistance()
		{
			UpdateDirections();
			DistanceToTarget = (Position.vector3 - Target.vector3).Length;
		}

		public void UpdateDirections()
		{
			Direction = Position.vector3 - Target.vector3;
			Direction.Normalize();

			Up = Vector3.UnitY;
			Right = Vector3.Normalize(Vector3.Cross(Up, Direction));
			Up = Vector3.Cross(Direction, Right);
		}

		public void updateCamera()
		{
			View = Matrix4.LookAt(Position.vector3, Target.vector3, Up);
			Projection = Matrix4.CreatePerspectiveFieldOfView(fov, Aspect, 0.01f, 1000f);
			if (ShowModel) LoadGeometry();
		}

		bool Rightmousepressed = false;

		public void FirstPerson(MouseEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftShift) && e.RightButton == MouseButtonState.Pressed)
			{
				var gl = e.OriginalSource as OpenTK.Wpf.GLWpfControl;
				if (gl == null) return;
				var pos = e.GetPosition(gl);

				if (oldx != 0)
				{
					float dx = ((float)pos.X - oldx) * MouseSensivity;
					float dy = ((float)pos.Y - oldy) * MouseSensivity * -1;

					Target.X += dx;
					Target.Y += dy;

				}
				oldx = (float)pos.X;
				oldy = (float)pos.Y;
				Rightmousepressed = true;
			}

			if (e.RightButton == MouseButtonState.Released)
			{
				if (Rightmousepressed)
				{
					oldx = 0;
					UpdateDirections();
					Rightmousepressed = false;
				}
			}
		}
		bool middlemousepressed = false;

		public void PanView(MouseEventArgs e)
		{

			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				var gl = e.OriginalSource as OpenTK.Wpf.GLWpfControl;
				if (gl == null) return;
				var pos = e.GetPosition(gl);
				int speed = 2;
				if (Keyboard.IsKeyDown(Key.LeftShift))
				{
					speed = 5;
				}
				if (oldx != 0)
				{
					float dx = speed * ((float)pos.X - oldx) / (float)MainWindow.Instance.GLWindow.ActualWidth;
					float dy = speed * ((float)pos.Y - oldy) / (float)MainWindow.Instance.GLWindow.ActualHeight * -1;

					var vx = -dx * Vector3.Normalize(MainWindow.mv.ViewCam.Right);
					var vy = -dy * Vector3.Normalize(MainWindow.mv.ViewCam.Up);

					Position.Update(Position.vector3 + vx + vy);
					Target.Update(Target.vector3 + vx + vy);
					View = Matrix4.LookAt(Position.vector3, Target.vector3, Up);
					UpdateDistance();
					middlemousepressed = true;
				}
				oldx = (float)pos.X;
				oldy = (float)pos.Y;
			}
			if (e.MiddleButton == MouseButtonState.Released)
			{
				if (middlemousepressed)
				{
					oldx = 0;
					UpdateDirections();
					middlemousepressed = false;
				}

			}
		}
		public void WheelControl(MouseWheelEventArgs e)
		{
			var dis = -1 * e.Delta * MouseSensivity;
			Fov(dis);
			Projection = Matrix4.CreatePerspectiveFieldOfView(fov, Aspect, 0.01f, 1000f);
		}
		public void Control(System.Windows.Input.KeyEventArgs e, TimeSpan t)
		{
			var currentHAngle = Math.Atan2(Position.X, Position.Z);
			var currentVAngle = Math.Atan2(Position.Z, Position.Y);
			var currentIAngle = Math.Atan2(Position.Y, Position.X);

			var stat = e.KeyStates;
			var win = e.OriginalSource as Window;

			if (win == null)
			{
				win = MainWindow.Instance;
			}

			if (e.Key == Key.Escape)
			{
				win.Close();
			}

			if (e.IsRepeat && e.Key == Key.Z)
			{
				Target.Update(Vector3.Zero);
				UpdateDistance();
				updateCamera();
				return;
			}

			if (e.Key == Key.V)
			{
				foreach (var o in MainWindow.mv.Geos)
				{


					o.shader.IsBlin = !o.shader.IsBlin;
				}
			}

			if (e.Key == Key.Z || e.Key == Key.X)
			{
				if (e.Key == Key.Z)
				{
					foreach (var o in MainWindow.mv.Geos)
					{

						o.shader.specintens -= 1f;
					}
				}
				else
				{
					foreach (var o in MainWindow.mv.Geos)
					{

						 o.shader.specintens += 1f;
					}
				}
			}

			//Rotat Camera Around Z Axis
			//---------------------------
			if (e.Key == Key.W || e.Key == Key.S)
			{
				var theta = (currentIAngle + increment);
				if (e.Key == Key.S)
					theta = (currentIAngle - increment);

				var x = Math.Sin(theta) * DistanceToTarget;
				var y = Math.Cos(theta) * DistanceToTarget;
				Position.Update(new Vector3((float)y, (float)x, Position.Z));
				View = Matrix4.LookAt(Position.vector3, Target.vector3, Up);
				UpdateDirections();
				updateCamera();
			}
			//Rotat Camera Around Y Axis
			//---------------------------
			if (e.Key == Key.A || e.Key == Key.D)
			{
				var theta = (currentHAngle - increment);
				if (e.Key == Key.D)
					theta = (currentHAngle + increment);
				var y = Math.Cos(theta) * DistanceToTarget;
				var x = Math.Sin(theta) * DistanceToTarget;
				Position.Update(new Vector3((float)x, Position.Y, (float)y));
				View = Matrix4.LookAt(Position.vector3, Target.vector3, Up);
				UpdateDirections();
				updateCamera();
			}
		}
	}
}
