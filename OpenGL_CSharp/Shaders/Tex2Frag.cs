using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_CSharp.Geometery;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_CSharp.Shaders
{
    public class Tex2Frag : BaseShader
    {
        public int lightshad, diffuseid, specularid = -1;
        public float specintens = 10;
  
        public Tex2Frag(BaseGeometry parent)
        {
            this.parent = parent;
        }

        public Tex2Frag(Vector3 vector3, string v1, string v2)
        {
            Loadtx2Fragment(vector3, v1, v2);
        }

        public string LightFrag2()
        {
            return File.ReadAllText("shaders/Light.frag");
        }



        public void Loadtx2Fragment(Vector3 lightColor, string diffuse, string specular)
        {
            //setup shaders
            //load vertix/Fragment shader
            //---------------------------
            lightshad = CreateShader(LightFrag2(), ShaderType.FragmentShader);

            //create program, link shaders and test the results
            LinkShader(vershad, lightshad);

            SetupStrids();

            //load Textures
            diffuseid = Textures.Textures.AddTexture(TextureUnit.Texture0, diffuse);
            specularid = Textures.Textures.AddTexture(TextureUnit.Texture1, specular);

            GetVariables();

        }

      

        public override void Use()
        {
            base.Use();

            //Use vertix shaders holder to the GPU memory
            //--------------
            Textures.Textures.Link(TextureUnit.Texture0, diffuseid);
            Textures.Textures.Link(TextureUnit.Texture1, specularid);

            //setuplight effect
            SetUniformV3("light.ambient", lightColor);
            SetUniformV3("light.diffuse", lightColor);
            SetUniformV3("light.position", lightPosition);
            SetUniformV3("light.specular", lightColor);

            SetFloat("material.shininess", specintens);            
            SetUniformV3("ViewPos", Program.cam.Position);//Set Camera Position to Shader to create Specular
            SetInt("material.diffuse", 0); //because this variable is of type sample2d, we need to specify which texture numberis used
            SetInt("material.specular", 1);

           

        }

        public override void Dispose()
        {
            GL.DeleteTexture(diffuseid);
            GL.DeleteTexture(specularid);
            GL.DeleteShader(lightshad);
            base.Dispose();
        }
    }
}
