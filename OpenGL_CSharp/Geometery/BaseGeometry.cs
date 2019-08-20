using OpenGL_CSharp.Graphic;
using OpenGL_CSharp.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Geometery
{
    class BaseGeometry
    {
        public List<Vertex> points;
        public int[] Indeces;

        public Vector3 objectColor;
        public static Vector3 lightColor = new Vector3(1f);
        public static float specintens = 0.4f;
        public bool IsLight = false;

        public Matrix4 model = Matrix4.Identity;

        public int vbo = -1;
        public int ebo = -1;
        public int vao = -1;
        public int texid1 = -1;
        public int texid2 = -1;
        public int vershad;
        public int fragshad;
        public int lightshad;
        public int programId = -1;
        public float[] vers;

        public BaseGeometry()
        {
            programId = GL.CreateProgram();
            GL.UseProgram(programId);

        }

        public void render()
        {
             
            //upload vao to GPU buffer
            GL.BindVertexArray(vao); 
            
            //upload vertex data to GPU buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vers.Length * sizeof(float), vers, BufferUsageHint.StaticDraw);
             
            //update light effect
            FragmentShaders.SetUniformV3(programId, "light.ambient", new Vector3(.2f));
            FragmentShaders.SetUniformV3(programId, "light.diffuse", new Vector3(.5f));
            FragmentShaders.SetUniformV3(programId, "light.specular", lightColor);
            FragmentShaders.SetUniformV3(programId, "light.position", new Vector3(0, 3, 4));

            //update textures the GPU memory
            //--------------
            Textures.Textures.ActivateandLink(TextureUnit.Texture0, texid1);
            Textures.Textures.ActivateandLink(TextureUnit.Texture1, texid2); 
             
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
 
            //setup lighiting effect
            //----------------------
            GL.UseProgram(programId); //we must link and use program first before applying light effects

            FragmentShaders.SetFloat(programId, "material.shininess", specintens);

            //Set Camera Position to Shader to create Specular
            FragmentShaders.SetUniformV3(programId, "ViewPos", Program.cam.Position);

            //orient Camera, MatrixTransformation
            Shaders.VertexShaders.SetUniformMatrix(programId, nameof(BaseGeometry.model), ref model);
        }

        static bool img = false;
        public void setupgeo()
        {
            vers = points.SelectMany(o => o.data()).ToArray();

            //Element buffer object

            if (vao == -1) ////no need to recreate if already created
                vao = GL.GenBuffer();
            GL.BindVertexArray(vao);


            if (vbo == -1) ////no need to recreate if already created
                vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); //define the type of buffer in gpu memory
                                                          //fill up the buffer with the data
                                                          //we need to define the type of data to be filled and the size in the memory
            GL.BufferData(BufferTarget.ArrayBuffer, vers.Length * sizeof(float), vers, BufferUsageHint.StaticDraw);

            //load shaders
            FragmentShaders.LoadFragment(programId, new Vector3(1),
                                         img? @"Textures\container.jpg": @"Textures\car.jpg",
                                            @"Textures\container_specular.jpg",
                                            out vershad,
                                            out lightshad,
                                            out texid1,
                                            out texid2);
            img = true;
              
            //define the structure of VBO
            setupstrid();

            //element buffer, is required to render based on indeces
            //--------------
            if (ebo == -1) ////no need to recreate if already created
                ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indeces.Length * sizeof(int), Indeces, BufferUsageHint.StaticDraw);
             
        }

        //create shaders

        int CreateShader(string source, ShaderType shadtype)
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

        //Explain to GPU the structure of vbo... Load/Link Shaders first
        void setupstrid()
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
        int CreatePrognLinkShader(int vershad, int fragshad)
        {
            programId = programId == -1 ? GL.CreateProgram() : programId;
            GL.AttachShader(programId, vershad);
            GL.AttachShader(programId, fragshad);
            GL.LinkProgram(programId);

            //test if the prog is fine
            var result = GL.GetProgramInfoLog(programId);
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine(result);
            }

            //after linking there is no need to keep/attach the shaders and should be cleared from memory
            GL.DetachShader(programId, vershad);
            GL.DetachShader(programId, fragshad);
            GL.DeleteShader(vershad);
            GL.DeleteShader(fragshad);

            return programId;
        }
    }
}
