using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Shaders.Light
{
	public class LightSource
	{
		public Vector3 ambient = new Vector3(1);
		public Vector3 diffuse = new Vector3(1);
		public Vector3 specular = new Vector3(1);
		public Vector3 lightPosition = new Vector3(1);
		public Vector3 Direction = new Vector3(1);
		public float OuterAngle = 0;
		public float InnerAngle = 0;

		public int LightType;  //0=>Point, 1=>Direction 2=>Spot Light

		public float Constance = 1;
		public float Linear = 0.09f;
		public float Quaderic = .032f;


		public static List<LightSource> SetupLights()
		{
			var lsources = new List<LightSource>();

			for (int i = 0; i < 2; i++)
			{
				var l = new LightSource();
				l.LightType = i;
				l.Direction = -Vector3.UnitY;
				l.lightPosition = new Vector3(2, 4f, 0);
				l.diffuse = new Vector3(1, 1, 1);

				l.InnerAngle = 12.5f;
				l.OuterAngle = 0;
				lsources.Add(l);
			}

#if true
			//sky light
			lsources[1].diffuse = new Vector3(0, .01f, .2f);
			lsources[1].specular = new Vector3(0, .01f, .2f);
			lsources[1].ambient = new Vector3(0, .01f, .2f);
#endif

			return lsources;

		}
	}
}
