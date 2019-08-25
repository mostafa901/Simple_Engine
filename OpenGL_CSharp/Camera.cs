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
			Projection = Matrix4.CreatePerspectiveFieldOfView(fov, Aspect, 0.01f, 100f);

		}
	}
}
