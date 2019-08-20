using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_CSharp.Shaders
{
    class LampFrag : BaseShader
    {
        public int lampfragId = -1;
        public int lightshad = -1;
        Vector3 lightColor;
        public LampFrag()
        {

        }

        string LampPoint()
        {
            return File.ReadAllText("shaders/Lamp-Point.frag");
        }
        
        public void LoadLampPointFragment(Vector3 lightColor)
        {

            this.lightColor = lightColor;

            //setup shaders
            //load vertix/Fragment shader
            //---------------------------
            lightshad = CreateShader(LampPoint(), ShaderType.FragmentShader);
             
            SetupStrids();

            GetVariables();
        }

        public override void Use()
        {
            //we must use program before lighting
            base.Use();
            //create program, link shaders and test the results
            LinkShader(vershad, lightshad);
            SetUniformV3("material.diffuse", lightColor); //because this variable is of type sample2d, we need to specify which texture numberis used
            SetUniformV3("material.specular", lightColor);
            SetUniformV3("ViewPos", Program.cam.Position);//Set Camera Position to Shader to create Specular
        }

        public override void Dispose()
        {
            GL.DeleteShader(lightshad);

            base.Dispose();

        }

    }
}
