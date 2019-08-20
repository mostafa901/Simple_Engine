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

        public LampFrag()
        {

        }

        string LampPointSource()
        {
            return File.ReadAllText("shaders/Lamp-Point.frag");
        }

        public void LoadLampPointFragment()
        {

            //setup shaders
            //load vertix/Fragment shader
            //---------------------------
            lightshad = CreateShader(LampPointSource(), ShaderType.FragmentShader);
            //create program, link shaders and test the results
            LinkShader(vershad, lightshad);

            SetupStrids();

            GetVariables();
        }

        public override void Use()
        {
            //we must use program before lighting
            base.Use();
          
            //create program, link shaders and test the results
            LinkShader(vershad, lightshad);
            SetUniformV3("material.diffuse", light.diffuse); //because this variable is of type sample2d, we need to specify which texture numberis used
            SetUniformV3("material.ambient", light.ambient);
            SetUniformV3("material.specular", light.specular);

        }

        public override void Dispose()
        {
            GL.DeleteShader(lightshad);

            base.Dispose();

        }

    }
}
