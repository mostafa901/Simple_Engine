using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Graphic;
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
    public class BaseShader
    {
        public readonly int programId = -1;
        public int vershad = -1;

        public readonly Dictionary<string, int> _uniformLocations;
        public BaseShader()
        {
            programId = GL.CreateProgram();
            vershad = CreateShader(ReadVerShader(), ShaderType.VertexShader);

            // Next, allocate the dictionary to hold the locations.
            _uniformLocations = new Dictionary<string, int>();
        }

        public void SetupStrids()
        {
            //Define Vertex specs
            //-------------------
            //since we are using vertex, opengl doesn't understand the specs of the vertex so we use vertexpointerattribute for this
            //now it is 5 instead of 3 for the texture coordinates
            var verloc = GL.GetAttribLocation(programId, "aPos");
            GL.VertexAttribPointer(verloc, Vertex3.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 0);
            GL.EnableVertexAttribArray(verloc); //now activate Vertexattrib

            //GetTexture coordinates
            //----------------------
            var texloc = GL.GetAttribLocation(programId, "aTexCoord");
            GL.VertexAttribPointer(texloc, Vertex2.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(texloc);

            //vertex Color
            //------------
            var vercolloc = GL.GetAttribLocation(programId, "aVerColor");
            GL.VertexAttribPointer(vercolloc, Vertex4.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(vercolloc);

            //Vertex Normal Color
            //------------
            var vNormal = GL.GetAttribLocation(programId, "aNormal");
            GL.VertexAttribPointer(vNormal, Vertex3.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 9 * sizeof(float));
            GL.EnableVertexAttribArray(vNormal);
        }

        string ReadVerShader()
        {
            return File.ReadAllText("Shaders\\shader.ver");
        }

        public void SetUniformMatrix(string name, ref Matrix4 value)
        {
            GL.UseProgram(programId);
            GL.UniformMatrix4(_uniformLocations[name], true, ref value);
        }

        public void SetUniformV3(string name, Vector3 value)
        {
            GL.UseProgram(programId);
            GL.Uniform3(_uniformLocations[name], value);

        }

        public void SetFloat(string name, float value)
        {
            GL.UseProgram(programId);

            GL.Uniform1(_uniformLocations[name], value);
        }

        public void SetInt(string name, int value)
        {
            GL.UseProgram(programId);

            GL.Uniform1(_uniformLocations[name], value);
        }


        //create shaders
        public int CreateShader(string source, ShaderType shadtype)
        {
            int shadid = GL.CreateShader(shadtype);
            GL.ShaderSource(shadid, source);
            GL.CompileShader(shadid);

            //test if the compilation is correct
            var result = GL.GetShaderInfoLog(shadid);
            if (!string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine(result);

            }
            return shadid;
        }


        public int LinkShader(int vershad, int fragshad)
        {
            GL.AttachShader(programId, vershad);
            GL.AttachShader(programId, fragshad);
            GL.LinkProgram(programId);

            //test if the prog is fine
            var result = GL.GetProgramInfoLog(programId);
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine(result);
            }

            GL.UseProgram(programId);

            //after linking there is no need to keep/attach the shaders and should be cleared from memory
            GL.DetachShader(programId, vershad);
            GL.DetachShader(programId, fragshad);
            GL.DeleteShader(vershad);
            GL.DeleteShader(fragshad);

            return programId;
        }

        public virtual void Use()
        {
            GL.UseProgram(programId); //we must link and use program first before applying light effects
                                      //orient Camera, MatrixTransformation
        }

        public virtual void Dispose()
        {
            GL.DeleteShader(vershad);
            GL.DeleteShader(programId);

        }
    }
}
