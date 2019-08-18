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
        private int lightshad;

        public void Init()
        {
            var vers = points.SelectMany(o => o.data()).ToArray();

            //Element buffer object
            if (vao == -1) //no need to recreate if already created
                vao = GL.GenBuffer();
            GL.BindVertexArray(vao);

            if (vbo == -1) //no need to recreate if already created
                vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); //define the type of buffer in gpu memory
                                                          //fill up the buffer with the data
                                                          //we need to define the type of data to be filled and the size in the memory
            GL.BufferData(BufferTarget.ArrayBuffer, vers.Length * sizeof(float), vers, BufferUsageHint.StaticDraw);

            //setup shaders
            //load vertix/Fragment shader
            //---------------------------
            if (texid1 == -1) //no need to recreate if already created
            {
                vershad = CreateShader(Shaders.VertexShaders.VShader(), ShaderType.VertexShader);
                //  fragshad = CreateShader(Shaders.FragmentShaders.TexFrag2Tex(), ShaderType.FragmentShader);
                lightshad = CreateShader(Shaders.FragmentShaders.LightFrag2(), ShaderType.FragmentShader);

                //create program, link shaders and test the results
                CreatePrognLinkShader(vershad, lightshad);
                GL.UseProgram(Program.pipe.programId);

                //load Textures
                texid1 = Textures.Textures.AddTexture(TextureUnit.Texture0, @"Textures\container.jpg");
                texid2 = Textures.Textures.AddTexture(TextureUnit.Texture1, @"Textures\container_specular.jpg");
            }

            //Use vertix shaders holder to the GPU memory
            //--------------
            Textures.Textures.Link(TextureUnit.Texture0, texid1);
            Textures.Textures.Link(TextureUnit.Texture1, texid2);


            //Define Vertex specs
            //-------------------
            //since we are using vertex, opengl doesn't understand the specs of the vertex so we use vertexpointerattribute for this
            //now it is 5 instead of 3 for the texture coordinates
            var verloc = GL.GetAttribLocation(Program.pipe.programId, "aPos");
            GL.VertexAttribPointer(verloc, Vertex3.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 0);
            GL.EnableVertexAttribArray(verloc); //now activate Vertexattrib

            //GetTexture coordinates
            //----------------------
            var texloc = GL.GetAttribLocation(Program.pipe.programId, "aTexCoord");
            GL.VertexAttribPointer(texloc, Vertex2.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(texloc);

            //vertex Color
            //------------
            var vercolloc = GL.GetAttribLocation(Program.pipe.programId, "aVerColor");
            GL.VertexAttribPointer(vercolloc, Vertex4.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(vercolloc);

            //Vertex Normal Color
            //------------
            var vNormal = GL.GetAttribLocation(Program.pipe.programId, "aNormal");
            GL.VertexAttribPointer(vNormal, Vertex3.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 9 * sizeof(float));
            GL.EnableVertexAttribArray(vNormal);


            //element buffer
            //--------------
            if (ebo == -1) ////no need to recreate if already created
                ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indeces.Length * sizeof(int), Indeces, BufferUsageHint.StaticDraw);



            //setup lighiting effect
            //----------------------
            GL.UseProgram(Program.pipe.programId); //we must link and use program first before applying light effects
                                                   //   FragmentShaders.SetUniformV3(Program.pipe.programId, "material.ambient", objectColor*.3f);            
            FragmentShaders.SetInt(Program.pipe.programId, "material.diffuse", 0); //because this variable is of type sample2d, we need to specify which texture numberis used
            FragmentShaders.SetInt(Program.pipe.programId, "material.specular", 1);
            FragmentShaders.SetFloat(Program.pipe.programId, "material.shininess", specintens);
            FragmentShaders.SetUniformV3(Program.pipe.programId, "light.ambient", new Vector3(.2f));
            FragmentShaders.SetUniformV3(Program.pipe.programId, "light.diffuse", new Vector3(.5f));
            FragmentShaders.SetUniformV3(Program.pipe.programId, "light.specular", lightColor);
            FragmentShaders.SetUniformV3(Program.pipe.programId, "light.position", new Vector3(0, 1, 4));

            //Set Camera Position to Shader to create Specular
            FragmentShaders.SetUniformV3(Program.pipe.programId, "ViewPos", Program.cam.Position);

            //orient Camera, MatrixTransformation
            Shaders.VertexShaders.SetUniformMatrix(Program.pipe.programId, nameof(BaseGeometry.model), ref model);




        }

        //create shaders
        static int CreateShader(string source, ShaderType shadtype)
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

        static int CreatePrognLinkShader(int vershad, int fragshad)
        {
            Program.pipe.programId = Program.pipe.programId == -1 ? GL.CreateProgram() : Program.pipe.programId;
            GL.AttachShader(Program.pipe.programId, vershad);
            GL.AttachShader(Program.pipe.programId, fragshad);
            GL.LinkProgram(Program.pipe.programId);

            //test if the prog is fine
            var result = GL.GetProgramInfoLog(Program.pipe.programId);
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine(result);
            }

            //after linking there is no need to keep/attach the shaders and should be cleared from memory
            GL.DetachShader(Program.pipe.programId, vershad);
            GL.DetachShader(Program.pipe.programId, fragshad);
            GL.DeleteShader(vershad);
            GL.DeleteShader(fragshad);

            return Program.pipe.programId;
        }
    }
}
