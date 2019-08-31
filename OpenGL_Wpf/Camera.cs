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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utility.IO;

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
		private string v;

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



		public Camera()
		{
			Initialize();
		}

		private void Initialize()
		{
			Target = new Vertex3(0, 0, 0);
			Position = new Vertex3(0, 0, 10);
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
#if false
			else if (fdeg > 45)
			{
				fov = MathHelper.DegreesToRadians(45);

			}
			else 
#endif
			else
			{
				fov = MathHelper.DegreesToRadians(fdeg);
			}
		}
		public void WheelControl(MouseWheelEventArgs e)
		{
			var dis = e.Delta * MouseSensivity;
			Fov(dis);
			Projection = Matrix4.CreatePerspectiveFieldOfView(fov, Aspect, 0.01f, 1000f);
		}
		public override void LoadGeometry()
		{
			points = new List<Graphic.Vertex>();
			var Vc = new Vertex4(objectColor.X, objectColor.Y, objectColor.Z, 1);

			points.Add(new Graphic.Vertex()
			{
				Position = Position,
				Normal = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Vcolor = Vc
			});

			points.Add(new Graphic.Vertex()
			{
				Position = Target,
				Normal = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Vcolor = Vc
			});
			Indeces = new List<int> { 0, 1 };
			primitiveType = PrimitiveType.Lines;

			vers = null; //set this to null to update vertex information.						
			base.LoadGeometry();
			shader = new ObjectColor();
		}
		public void MoveForward(TimeSpan et)
		{
			// move target and position on the same Camera direction
			Position.Update(Position.vector3 + (increment * Vector3.Normalize(MainWindow.mv.ViewCam.Direction)));
			Target.Update(Target.vector3 + (increment * Vector3.Normalize(MainWindow.mv.ViewCam.Direction)));
			View = Matrix4.LookAt(Position.vector3, Target.vector3, Up);
			UpdateDistance();
		}
		public void MoveRight(TimeSpan et)
		{
			// move target and position on the same Camera direction
			Position.Update(Position.vector3 + (increment * Vector3.Normalize(MainWindow.mv.ViewCam.Right)));
			Target.Update(Target.vector3 + (increment * Vector3.Normalize(MainWindow.mv.ViewCam.Right)));
			View = Matrix4.LookAt(Position.vector3, Target.vector3, Up);
			UpdateDistance();
		}
		public void MoveBackward(TimeSpan et)
		{
			// move target and position on the same Camera direction
			Position.Update(Position.vector3 - (increment * Vector3.Normalize(MainWindow.mv.ViewCam.Direction)));
			Target.Update(Target.vector3 - (increment * Vector3.Normalize(MainWindow.mv.ViewCam.Direction)));
			View = Matrix4.LookAt(Position.vector3, Target.vector3, Up);
			UpdateDistance();
		}
		public void MoveLeft(TimeSpan et)
		{
			// move target and position on the same Camera direction			  
			Position.Update(Position.vector3 - (increment * Vector3.Normalize(MainWindow.mv.ViewCam.Right)));
			Target.Update(Target.vector3 - (increment * Vector3.Normalize(MainWindow.mv.ViewCam.Right)));
			View = Matrix4.LookAt(Position.vector3, Target.vector3, Up);
			UpdateDistance();
		}

		public void FirstPerson(MouseEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftShift))
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
			}
		}

		void UpdateDistance()
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

		public void Control(System.Windows.Input.KeyEventArgs e, TimeSpan t)
		{
			var currentHAngle = Math.Atan2(Position.X, Position.Z);
			var currentVAngle = Math.Atan2(Position.Z, Position.Y);
			var currentIAngle = Math.Atan2(Position.Y, Position.X);

			var stat = e.KeyStates;
			var win = e.OriginalSource as Window;

			if (win == null)
			{
				win = Utility.IO.wpf.FindParent<Window>(((FrameworkElement)e.OriginalSource), "");
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
				MainWindow.mv.Geos.ForEach(o => o.shader.IsBlin = !o.shader.IsBlin);
			}

			if (e.Key == Key.Z || e.Key == Key.X)
			{
				if (e.Key == Key.Z)
				{
					MainWindow.mv.Geos.ForEach(o => o.shader.specintens -= 1f);
				}
				else
				{
					MainWindow.mv.Geos.ForEach(o => o.shader.specintens += 1f);
				}
			}

			if (e.Key == Key.Right)
			{
				MoveRight(t);
			}
			if (e.Key == Key.Left)
			{
				MoveLeft(t);
			}

			if (e.Key == Key.Down)
			{
				MoveBackward(t);
			}

			if (e.Key == Key.Up)
			{
				MoveForward(t);
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
