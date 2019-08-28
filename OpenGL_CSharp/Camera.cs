using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp
{
	public class Camera
	{
		//Camera Properties

		public Vector3 Position;
		public Vector3 Up;
		public Vector3 Direction;
		public Vector3 Right;
		public Vector3 Target;
		public float yaw = 0;
		public float pitch = 0;
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



		public void updateCamera()
		{
			//Direction = Vector3.Normalize(Position - Target);

			if (pitch > 89.0f)
				pitch = 89.0f;
			if (yaw < -89.0f)
				yaw = -89.0f;


			Direction.X = (float)Math.Sin(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));
			Direction.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
			Direction.Z = -(float)Math.Cos(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));

			Up = Vector3.UnitY;
			Right = Vector3.Normalize(Vector3.Cross(Up, Direction));
			Up = Vector3.Cross(Direction, Right);

			View = Matrix4.LookAt(Position, Target, Up);
			Projection = Matrix4.CreatePerspectiveFieldOfView(fov, Aspect, 0.01f, 1000f);

		}

		#region Navigation
		public void Control(System.Windows.Input.KeyEventArgs e)
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
				((MainWindow)sender).Close();
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

		public void WheelControl(System.Windows.Input.MouseWheelEventArgs e)
		{
			if (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift))
			{
				Program.pipe.cam.Target += new Vector3(0, e.Delta, 0);
			}
			else
			{
				Program.pipe.cam.Fov(e.Delta);
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
