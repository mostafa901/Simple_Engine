using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Shaders
{
    public static class VertexShaders
    {
        public static string VShader()
        {
            return File.ReadAllText("Shaders\\shader.ver");
        }

        public static void SetUniformMatrix(int programId, string name, ref Matrix4 value)
        {
            var matloc = GL.GetUniformLocation(programId, name);
            GL.UniformMatrix4(matloc, true, ref value);
        }
         
    }
}
