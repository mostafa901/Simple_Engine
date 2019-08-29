using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Graphic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
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

		public Camera()
		{
			Target = Vector3.Zero;
			Position = new Vector3(0, 0, 10);
			Direction = -Vector3.UnitZ;
			Right = Vector3.UnitZ;
			r = 5;

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

			var increment = 1f;

			Position = Position + (increment * Vector3.Normalize(Program.pipe.cam.Direction));
			Target = Target + (increment * Vector3.Normalize(Program.pipe.cam.Direction));

		}
		public void MoveRight(TimeSpan et)
		{           // move target and position on the same Camera direction

			var increment = 1f;
			Position = Position - (increment * Vector3.Normalize(Program.pipe.cam.Right));
			Target = Target - (increment * Vector3.Normalize(Program.pipe.cam.Right));
		}
		public void MoveBackward(TimeSpan et)
		{
			// move target and position on the same Camera direction

			var increment = 1f;
			Position = Position - (increment * Vector3.Normalize(Program.pipe.cam.Direction));
			Target = Target - (increment * Vector3.Normalize(Program.pipe.cam.Direction));

		}
		public void MoveLeft(TimeSpan et)
		{
			// move target and position on the same Camera direction
			var increment = 1f;
			Position = Position + (increment * Vector3.Normalize(Program.pipe.cam.Right));
			Target = Target + (increment * Vector3.Normalize(Program.pipe.cam.Right));

		}



		public void FirstPerson(MouseEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftShift))
			{
				var pos = e.GetPosition((OpenTK.Wpf.GLWpfControl)e.OriginalSource);

				if (oldx != 0)
				{
					float dx = (float)pos.X - oldx;
					float dy = (float)pos.Y - oldy;

					yaw = 0.25f * dy;
					pitch = 0.25f * dx;

					//Matyaw = Matrix4.CreateRotationY(yaw); 
					//Matpitch = Matrix4.CreateRotationX(pitch);
					//var rotmat = Matpitch * Matyaw;
					//var trans = Matrix4.CreateTranslation(-Position);
					//View = rotmat * trans;

					if (pitch > 89.0f)
						pitch = 89.0f;
					if (yaw < -89.0f)
						yaw = -89.0f;
					yaw += MathHelper.DegreesToRadians(yaw);
					pitch += MathHelper.DegreesToRadians(pitch);
					Direction.X = (float)Math.Cos(pitch) * (float)Math.Cos(yaw);
					Direction.Y = (float)Math.Sin(pitch);
					Direction.Z = (float)Math.Cos(pitch) * (float)Math.Sin(yaw);

				}
				oldx = (float)pos.X;
				oldy = (float)pos.Y;
			}
		}


		public void updateCamera()
		{
			Direction = Vector3.Normalize(Target - Position);

			//if (pitch > 89.0f)
			//	pitch = 89.0f;
			//if (yaw < -89.0f)
			//	yaw = -89.0f;


			//Direction.X = (float)Math.Sin(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));
			//Direction.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
			//Direction.Z = -(float)Math.Cos(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));

			Up = Vector3.UnitY;
			Right = Vector3.Normalize(Vector3.Cross(Up, Direction));
			Up = Vector3.Cross(Direction, Right);
			 
			View = Matrix4.LookAt(Position, Target, Up);
			Projection = Matrix4.CreatePerspectiveFieldOfView(fov, Aspect, 0.01f, 1000f);

			RenderCameraLine();
		}

		public void Control(System.Windows.Input.KeyEventArgs e, TimeSpan t)
		{

			var stat = e.KeyStates;
			var win = ((Window)e.OriginalSource);
			if (e.Key == Key.Escape)
			{
				win.Close();
			}

			if (Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt))
			{
				Target = Program.pipe.geos.First(o => o.primitiveType == PrimitiveType.Triangles).GetBoundingBox().Mid.vector3;

				Position = Target * -new Vector3(1.2f);
				r = Position.Length;
			}

			Hangle = Math.Atan2(Position.X, Position.Z);
			Vangle = Math.Atan2(Position.Z, Position.Y);

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
				//var increment = Right * new Vector3(1.1f);

				//var oldr = Position.Length;
				//if (Keyboard.IsKeyDown(Key.LeftShift)) Target += increment;
				//else Position += increment;
				//r += r - Position.Length;
				MoveRight(t);
			}
			if (e.Key == Key.Left)
			{
				//var increment = Right * new Vector3(1.1f);

				//var oldr = Position.Length;
				//if (Keyboard.IsKeyDown(Key.LeftShift)) Target -= increment;
				//else Position -= increment;
				//r += r - Position.Length;
				MoveLeft(t);
			}

			if (e.Key == Key.Down)
			{
				//var increment = Up * new Vector3(1.1f);


				//var oldr = Position.Length;
				//if (Keyboard.IsKeyDown(Key.LeftShift)) Target -= increment;
				//else Position -= increment;
				//r += r - Position.Length;

				MoveBackward(t);
			}

			if (e.Key == Key.Up)
			{
				//var increment = Up * new Vector3(1.1f);

				//var oldr = Position.Length;
				//if (Keyboard.IsKeyDown(Key.LeftShift)) Target += increment;
				//else Position += increment;
				//r += r - Position.Length;

				MoveForward(t);
			}


			if (e.Key == Key.W)
			{
				Vangle += inrement;

				Position = new Vector3(Position.X, (float)Math.Cos(Vangle) * r, (float)Math.Sin(Vangle) * r);

			}

			if (e.Key == Key.S)
			{
				Vangle -= inrement;

				Position = new Vector3(Position.X, (float)Math.Cos(Vangle) * r, (float)Math.Sin(Vangle) * r);

			}

			if (e.Key == Key.A)
			{
				Hangle -= inrement;

				Position = new Vector3((float)Math.Sin(Hangle) * r, Position.Y, (float)Math.Cos(Hangle) * r);

			}

			if (e.Key == Key.D)
			{
				Hangle += inrement;

				Position = new Vector3((float)Math.Sin(Hangle) * r, Position.Y, (float)Math.Cos(Hangle) * r);
			}

			var mouse = OpenTK.Input.Mouse.GetState();
			if (e.Key == Key.LeftCtrl)
			{
				var dx = mouse.X / (float)win.ActualWidth - oldx;
				var dy = mouse.Y / (float)win.ActualHeight - oldy;


				Target += new Vector3(dx, dy, dx);

				oldx = mouse.X / (float)win.ActualWidth;
				oldy = mouse.Y / (float)win.ActualHeight;
			}
			else
			{

				//  cam.Target = Vector3.Zero;
			}

			updateCamera();


		}

		static float oldx = 0;
		static float oldy = 0;

		static float r = 5f;
		static float inrement = 0.1744f;
		static double Hangle = 0;
		static double Vangle = 0;


		public void WheelControl(MouseWheelEventArgs e)
		{
			if (e.Delta > 0)
			{
				Position *= 1.2f;
			}
			if (e.Delta < 0)
			{
				Position *= 0.8f;
			}
			r = Position.Length;
			updateCamera();
		}
	}
}
