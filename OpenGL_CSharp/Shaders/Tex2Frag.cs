﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            SetUniformV3("light.specular", lightColor);
        }

      

        public override void Use()
        {
            base.Use();

            //Use vertix shaders holder to the GPU memory
            //--------------
            Textures.Textures.Link(TextureUnit.Texture0, diffuseid);
            Textures.Textures.Link(TextureUnit.Texture1, specularid);

            SetFloat("material.shininess", specintens);            
            SetUniformV3("ViewPos", Program.cam.Position);//Set Camera Position to Shader to create Specular
            SetInt("material.diffuse", 0); //because this variable is of type sample2d, we need to specify which texture numberis used
            SetInt("material.specular", 1);

            //setuplight effect
            SetUniformV3("light.ambient", new Vector3(.2f));
            SetUniformV3("light.diffuse", new Vector3(.5f));
            SetUniformV3("light.position", new Vector3(0, 3, 4));
        }

        public override void Dispose()
        {
            GL.DeleteShader(diffuseid);
            GL.DeleteShader(specularid);
            GL.DeleteShader(lightshad);
            base.Dispose();
        }
    }
}
