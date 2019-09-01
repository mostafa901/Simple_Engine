using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Graphic;
using OpenGL_CSharp.Shaders;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Wpf
{
	class Import
	{


		static private Matrix4 FromMatrix(Assimp.Matrix4x4 mat)
		{
			Matrix4 m = new Matrix4();
			m.M11 = mat.A1;
			m.M12 = mat.A2;
			m.M13 = mat.A3;
			m.M14 = mat.A4;
			m.M21 = mat.B1;
			m.M22 = mat.B2;
			m.M23 = mat.B3;
			m.M24 = mat.B4;
			m.M31 = mat.C1;
			m.M32 = mat.C2;
			m.M33 = mat.C3;
			m.M34 = mat.C4;
			m.M41 = mat.D1;
			m.M42 = mat.D2;
			m.M43 = mat.D3;
			m.M44 = mat.D4;
			return m;

		}
		public Import(MV_App mv)
		{
			Assimp.AssimpContext imp = new Assimp.AssimpContext();
			imp.SetConfig(new Assimp.Configs.NormalSmoothingAngleConfig(66f));
			var scene = imp.ImportFile("Models/firehydrant.obj", Assimp.PostProcessSteps.Triangulate | Assimp.PostProcessSteps.FlipUVs);

			var model = FromMatrix(scene.RootNode.Transform);
			 
			scene.Meshes.ForEach(m =>
			{
				var duck = new BaseGeometry();
				duck.Name = "Duck";

				duck.points = new List<Vertex>();
				duck.Indeces = new List<int>();

				for (int j = 0; j < m.FaceCount; j++)
				{
					var f = m.Faces[j];
					for (int i = 0; i < f.IndexCount; i++)
					{
						var ind = f.Indices[i];
						duck.Indeces.Add(ind);

						var ver = Vertex.FromVertex3(m.Vertices[ind]);
						var normal = Vertex.FromVertex3(m.Normals[ind]);
						var text = new Vertex2(0, 0);
						if (m.HasTextureCoords(0))
						{
							text = Vertex.FromVertex2(m.TextureCoordinateChannels[0][ind]);
						}

						var vcol = new Vertex4(1f, .5f, 0f, 1f);
						if (m.HasVertexColors(ind))
							vcol = Vertex.FromVertex4(m.VertexColorChannels[0][ind]);

						duck.points.Add(new Vertex()
						{
							Normal = normal,
							Position = ver,
							TexCoor = text,
							Vcolor = vcol
						});

						Debug.WriteLine($"\r\n\r\n--------------\r\nFace: {j}  - Ind: {ind}\r\n{duck.points.Count - 1}: {duck.points.Last().ToString()}");
					}


				}

				duck.LoadGeometry();
				duck.ShowModel = true;
				duck.objectColor = new Vector3(1f, 0f, 1f);
				duck.shader = new Tex2Frag(duck.objectColor);
			});
		}
	}
}
