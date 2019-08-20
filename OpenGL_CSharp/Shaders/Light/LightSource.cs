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

        public float Constance = 1;
        public float Linear = 0.09f;
        public float Quaderic = .032f;


    }
}
