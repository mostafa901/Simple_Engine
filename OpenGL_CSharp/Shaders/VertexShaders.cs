using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Shaders
{
    public static class VertexShaders
    {
        public static string VShader()
        {
            return @"
#version 330 core
layout (location=0) in vec3 aPos;
 
void main()
{
gl_Position=vec4(aPos,1.0f);
}
";
        }
    }
}
