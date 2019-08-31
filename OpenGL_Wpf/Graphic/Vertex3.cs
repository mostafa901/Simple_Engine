using System;
using Assimp;
using OpenTK;

namespace OpenGL_CSharp.Graphic
{

	public class Vertex3 : Vertex2
	{
		public static new int vcount = 3;

		#region vector3		 
		public Vector3 vector3
		{
			get
			{
				return  new Vector3(X, Y, Z);
			}			
		}
		#endregion


		public Vertex3(float v1, float v2, float v) : base(v1, v2)
		{
			Z = v;
		}

		

		//Coordinates
		#region Z

		private float _Z;
		private Vector3 objectColor;

		public float Z
		{
			get
			{
				return _Z;
			}
			set { SetProperty(ref _Z, value); }

		}
		#endregion

		override public float[] data()
		{
			return new float[] { X, Y, Z };
		}

		public override string ToString()
		{
			return $"{vector2.ToString()}, {Z.ToString()}";
		}

		internal void Update(Vector3 v3)
		{
			X = v3.X;
			Y = v3.Y;
			Z = v3.Z;
		}
	}
}
