using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Graphic;
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
using System.Windows.Input;

namespace OpenGL_CSharp
{
	public class Camera : BaseGeometry
	{
		//Camera Properties
		public Vector3 Position;
		public Vector3 Up;
		public Vector3 Direction;
		public Vector3 Right;
		public Vector3 Target;
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
		public Camera()
		{
			Target = Vector3.Zero;
			Position = new Vector3(0, 0, 10);
			Direction = -Vector3.UnitZ;
			Right = Vector3.UnitX;
			Up = Vector3.UnitY;
			UpdateDistance();
			updateCamera();
		}
		public void RenderCameraLine()
		{
			points = new List<Graphic.Vertex>();
			points.Add(new Graphic.Vertex()
			{
				Position = Vertex.FromVertex3(Position),
				Normal = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Vcolor = new Vertex4(0, 1, 0, 1)
			});

			points.Add(new Graphic.Vertex()
			{
				Position = Vertex.FromVertex3(Target),
				Normal = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Vcolor = new Vertex4(0, 1, 0, 1)
			});
			Indeces = new List<int> { 0, 1 };
			primitiveType = PrimitiveType.Lines;
			vers = null; //set this to null to update vertex information.
		}
		public void MoveForward(TimeSpan et)
		{
			// move target and position on the same Camera direction
			Position = Position + (increment * Vector3.Normalize(Program.pipe.cam.Direction));
			Target = Target + (increment * Vector3.Normalize(Program.pipe.cam.Direction));
			View = Matrix4.LookAt(Position, Target, Up);
			UpdateDistance();
		}
		public void MoveRight(TimeSpan et)
		{
			// move target and position on the same Camera direction
			Position = Position + (increment * Vector3.Normalize(Program.pipe.cam.Right));
			Target = Target + (increment * Vector3.Normalize(Program.pipe.cam.Right));
			View = Matrix4.LookAt(Position, Target, Up);
			UpdateDistance();
		}
		public void MoveBackward(TimeSpan et)
		{
			// move target and position on the same Camera direction
			Position = Position - (increment * Vector3.Normalize(Program.pipe.cam.Direction));
			Target = Target - (increment * Vector3.Normalize(Program.pipe.cam.Direction));
			View = Matrix4.LookAt(Position, Target, Up);
			UpdateDistance();
		}
		public void MoveLeft(TimeSpan et)
		{
			// move target and position on the same Camera direction			  
			Position = Position - (increment * Vector3.Normalize(Program.pipe.cam.Right));
			Target = Target - (increment * Vector3.Normalize(Program.pipe.cam.Right));
			View = Matrix4.LookAt(Position, Target, Up);
			UpdateDistance();
		}

		public void FirstPerson(MouseEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftShift))
			{
				var pos = e.GetPosition((OpenTK.Wpf.GLWpfControl)e.OriginalSource);

				if (oldx != 0)
				{
					float dy = (float)pos.X - oldx;
					float dx = (float)pos.Y - oldy;

					yaw += MouseSensivity * dy;
					pitch += MouseSensivity * dx;

					float maxang = 89;
					if (pitch > maxang)
						pitch = maxang;
					if (pitch < -maxang)
						pitch = -maxang;

					yaw = MathHelper.DegreesToRadians(yaw);
					pitch = MathHelper.DegreesToRadians(pitch);
					Matyaw = Matrix4.CreateRotationY(yaw);
					Matpitch = Matrix4.CreateRotationX(pitch);
					View = View * Matyaw * Matpitch;
				}
				oldx = (float)pos.X;
				oldy = (float)pos.Y;

				UpdateDistance();
			}
		}

		void UpdateDistance()
		{
			UpdateDirections();
			DistanceToTarget = (Position - Target).Length;
		}

		void UpdateDirections()
		{
			Direction = Position - Target;
			Direction.Normalize();

			Up = Vector3.UnitY;
			Right = Vector3.Normalize(Vector3.Cross(Up, Direction));
			Up = Vector3.Cross(Direction, Right);			 
		}

		public void updateCamera()
		{
			 
			View = Matrix4.LookAt(Position, Target, Up);
			Projection = Matrix4.CreatePerspectiveFieldOfView(fov, Aspect, 0.01f, 1000f);
			RenderCameraLine();
		}

		public void Control(System.Windows.Input.KeyEventArgs e, TimeSpan t)
		{
			var currentHAngle = Math.Atan2(Position.X, Position.Z);
			var currentVAngle = Math.Atan2(Position.Z, Position.Y);
			var currentIAngle = Math.Atan2(Position.Y, Position.X);

			var stat = e.KeyStates;
			var win = ((Window)e.OriginalSource);
			if (e.Key == Key.Escape)
			{
				win.Close();
			}

			if (e.IsRepeat && e.Key == Key.Z)
			{
				Target = Vector3.Zero;
				UpdateDistance();
				updateCamera();
				return;
			}

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
				Position.X = (float)y;
				Position.Y = (float)x;
				View = Matrix4.LookAt(Position, Target, Up);

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
				Position.X = (float)x;
				Position.Z = (float)y;
				View = Matrix4.LookAt(Position, Target, Up);
			}

			UpdateDirections();
			updateCamera();
		}

		public void Fov(float value)
		{
			var fdeg = MathHelper.RadiansToDegrees(fov);
			fdeg += value;

			if (fdeg < 1)
			{
				fov = MathHelper.DegreesToRadians(1);
			}
			else if (fdeg > 45)
			{
				fov = MathHelper.DegreesToRadians(45);

			}
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
	}
}
