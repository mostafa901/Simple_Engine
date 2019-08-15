using OpenTK;
using OpenTK.Graphics.OpenGL;
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
layout (location=1) in vec2 aTexCoord; 
layout (location=2) in vec3 aVerColor; 

out vec2 texCoord;
out vec3 vcolor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
texCoord = aTexCoord ;
vcolor=aVerColor;

gl_Position=  vec4(aPos,1.0f) * model * view * projection;
}
";
        }

        public static void SetUniformMatrix(int programId, string name, ref Matrix4 value)
        {
            var matloc = GL.GetUniformLocation(programId, name);
            GL.UniformMatrix4(matloc, true, ref value);
        }
    }
}
