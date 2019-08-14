using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Shaders
{
    class FragmentShaders
    {
        public static string Frag()
        {
            return @"
#version 330 core
out vec4 FragColor;
void main(){
FragColor=vec4(1.0f,1.0f,0,1.0f);
}
";
        }
    }
}
