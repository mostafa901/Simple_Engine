using OpenGL_CSharp.Geometery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Graphic
{
	public class BoundingBox
	{
		public Vertex3 Min { get; set; }
		public Vertex3 Max { get; set; }

		public Vertex3 Mid { get; set; }

		public BoundingBox()
		{

		}

		public BoundingBox(BaseGeometry geo)
		{
			float minX = geo.points.Select(o => o.Position).Min(x => x.X);
			float minY = geo.points.Select(o => o.Position).Min(x => x.Y);
			float minZ = geo.points.Select(o => o.Position).Min(x => x.Z);

			float maxX = geo.points.Select(o => o.Position).Max(x => x.X);
			float maxY = geo.points.Select(o => o.Position).Max(x => x.Y);
			float maxZ = geo.points.Select(o => o.Position).Max(x => x.Z);

			Min = new Vertex3(minX, minY, minZ);
			Max = new Vertex3(maxX, maxY, maxZ);

			Mid = Vertex.FromVertex3(0.5f * (Min.vector3 + Max.vector3));
		}

	}
}
