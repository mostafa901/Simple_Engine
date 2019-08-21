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

        public Tex2Frag()
        {

        }

        public Tex2Frag( string v1, string v2)
        {
            Loadtx2Fragment( v1, v2);
        }

        public string LightFrag2Source()
        {
            return File.ReadAllText("shaders/Text2DirectLight.frag");
        }
        
        public void Loadtx2Fragment( string diffuse, string specular)
        {
            //setup shaders
            //load vertix/Fragment shader
            //---------------------------
            lightshad = CreateShader(LightFrag2Source(), ShaderType.FragmentShader);

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
            //Use vertix shaders holder to the GPU memory
            //--------------

            Textures.Textures.ActivateandLink(TextureUnit.Texture0, diffuseid);
            Textures.Textures.ActivateandLink(TextureUnit.Texture1, specularid);

            base.Use();

            
            
            SetFloat("material.shininess", specintens);
            SetUniformV3("ViewPos", Program.cam.Position);//Set Camera Position to Shader to create Specular
            SetInt("material.diffuse", 0); //because this variable is of type sample2d, we need to specify which texture numberis used
            SetInt("material.specular", 1);
            
            //setuplight effect
            SetUniformV3("light.ambient", light.ambient );
            SetUniformV3("light.diffuse", light.diffuse);
            SetUniformV3("light.specular", light.specular);
            SetUniformV3("light.position", light.lightPosition);
            SetUniformV3("light.Direction", light.Direction);
            SetFloat("light.Constant", light.Constance);
            SetFloat("light.Linear", light.Linear);
            SetFloat("light.Quaderic", light.Quaderic);

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
