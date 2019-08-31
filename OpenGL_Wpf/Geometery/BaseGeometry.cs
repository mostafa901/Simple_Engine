using Assimp;
using Assimp.Configs;
using OpenGL_CSharp.Graphic;
using OpenGL_CSharp.Shaders;
using OpenGL_Wpf;
using OpenTK;
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
using Utility.MVVM;

namespace OpenGL_CSharp.Geometery
{
	public class BaseGeometry : BaseDataObject
	{
		public List<Vertex> points = new List<Vertex>();
		public List<int> Indeces = new List<int>();
		public Vector3 objectColor;


		public FrontFaceDirection FaceDirection = FrontFaceDirection.Ccw;
		public OpenTK.Graphics.OpenGL.PrimitiveType primitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
		public Matrix4 model = Matrix4.Identity;

		public int vbo = -1;
		public int ebo = -1;
		public int vao = -1;

		public float[] vers;

		public BaseShader shader;

		public string modelpath = "";
		private float increment = 0.5f;

		#region ShowModel

		private bool _ShowModel = false;

		public bool ShowModel
		{
			get
			{
				return _ShowModel;
			}
			set { SetProperty(ref _ShowModel, value); }

		}
		#endregion
		 
		public BoundingBox Bbx { get; set; }

		public BaseGeometry()
		{
			MainWindow.mv.Geos.Add(this);
			//setup Show model in the view Command
			CMD.action = (a) =>
			{
				ShowModel = (bool)a;
				if (ShowModel)
				{
					LoadGeometry();
				}
			};
		}

		public void RenderGeometry()
		{
			if (vers == null) //it will be null maually whenever we are updating position of verteces
				vers = points.SelectMany(o => o.data()).ToArray();

			GL.BindVertexArray(vao);
			GL.BufferData(BufferTarget.ArrayBuffer, vers.Length * sizeof(float), vers, BufferUsageHint.StaticDraw);
			GL.BufferData(BufferTarget.ElementArrayBuffer, Indeces.ToArray().Length * sizeof(int), Indeces.ToArray(), BufferUsageHint.StaticDraw);

			if (shader != null)
			{
				shader.Use();

				shader.SetUniformMatrix(nameof(BaseGeometry.model), ref model);
				shader.SetUniformMatrix(nameof(MainWindow.mv.ViewCam.View), ref MainWindow.mv.ViewCam.View);
				shader.SetUniformMatrix(nameof(MainWindow.mv.ViewCam.Projection), ref MainWindow.mv.ViewCam.Projection);
			}
		}

		public virtual void LoadGeometry()
		{
			if (vers == null) //no need to recreate if already created
				vers = points.SelectMany(o => o.data()).ToArray();

			//Element buffer object
			if (vao == -1) //no need to recreate if already created
				vao = GL.GenBuffer();
			//Binding Vertex array object
			GL.BindVertexArray(vao);

			if (vbo == -1) //no need to recreate if already created
				vbo = GL.GenBuffer();

			//define the type of buffer in gpu memory
			//fill up the buffer with the data
			//we need to define the type of data to be filled and the size in the memory
			GL.BufferData(BufferTarget.ArrayBuffer, vers.Length * sizeof(float), vers, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

			//element buffer
			//--------------
			if (ebo == -1) ////no need to recreate if already created
				ebo = GL.GenBuffer();
			//Element Buffer
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, Indeces.ToArray().Length * sizeof(int), Indeces.ToArray(), BufferUsageHint.StaticDraw);
		}


		public void Dispose()
		{
			shader.Dispose();

		}

		public BoundingBox GetBoundingBox()
		{
			return new BoundingBox(this);
		}

		public void ScaleGeo(float value = 1)
		{
			var matscale = Matrix4.CreateScale(value);

			model = matscale * model;
		}
		public void RotateGeo(System.Windows.Input.KeyEventArgs e)
		{

			if (e.Key == Key.E)
			{
				var rotmatz = Matrix4.CreateRotationZ(increment);
				model = rotmatz * model;
			}

			if (e.Key == Key.D)
			{
				var rotmatz = Matrix4.CreateRotationZ(-increment);
				model = rotmatz * model;
			}


			if (e.Key == Key.W)
			{
				var rotmatz = Matrix4.CreateRotationY(increment);
				model = rotmatz * model;
			}

			if (e.Key == Key.S)
			{
				var rotmatz = Matrix4.CreateRotationY(-increment);
				model = rotmatz * model;
			}

			if (e.Key == Key.Q)
			{
				var rotmat = Matrix4.CreateRotationX(-increment);
				model = rotmat * model;
			}

			if (e.Key == Key.A)
			{
				var rotmat = Matrix4.CreateRotationX(increment);
				model = rotmat * model;
			}
		}
	}
}
