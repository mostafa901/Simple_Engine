using System;
using System.Collections.Generic;
using System.Drawing;
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
        
        public Tex2Frag()
        {

        }

        public Tex2Frag(Vector3 oc)
        {
            var temp = Path.GetTempFileName() + ".jpg";
            Bitmap bm = new Bitmap(100, 100);
            Graphics.FromImage(bm).FillRectangle(new SolidBrush(Color.FromArgb(1, (int)(oc.X * 255), (int)(oc.Y * 255), (int)(oc.Z * 255))), new Rectangle(0, 0, 100, 100));
            bm.Save(temp);
            bm.Dispose();

            Loadtx2Fragment(temp, temp);
        }

        public Tex2Frag(string v1, string v2)
        {
            Loadtx2Fragment(v1, v2);
        }

        public string LightFrag2Source()
        {
            return File.ReadAllText("shaders/TexturedLight.frag");
        }

        public void Loadtx2Fragment(string diffuse, string specular)
        {
            //setup shaders
            //load vertix/Fragment shader
            //---------------------------
            lightshad = CreateShader(LightFrag2Source(), ShaderType.FragmentShader);

            //create program, link shaders and test the results
            LinkShader(vershad, lightshad);

            SetupStrids();

            //load Textures
            if (!string.IsNullOrEmpty(diffuse)) diffuseid = Textures.Textures.AddTexture(TextureUnit.Texture0, diffuse);
            if (!string.IsNullOrEmpty(specular)) specularid = Textures.Textures.AddTexture(TextureUnit.Texture1, specular);

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

            SetFloat("ambientcoff", 0.15f);//Set Camera Position to Shader to create Specular
            SetInt("material.diffuse", 0); //because this variable is of type sample2d, we need to specify which texture numberis used
            SetInt("material.specular", 1);
            SetInt("TotalLightNumber", LightSources.Count);
            SetInt("IsBlin", Convert.ToInt32(IsBlin));


            for (int i = 0; i < LightSources.Count; i++)
            {
                //setuplight effect
                //-----------------
                SetUniformV3($"Lights[{i}].ambient", LightSources[i].ambient);
                SetUniformV3($"Lights[{i}].diffuse", LightSources[i].diffuse);
                SetUniformV3($"Lights[{i}].specular", LightSources[i].specular);
                SetUniformV3($"Lights[{i}].position", LightSources[i].lightPosition);
                SetUniformV3($"Lights[{i}].Direction", LightSources[i].Direction);
                SetFloat($"Lights[{i}].InnerAngle", LightSources[i].InnerAngle);
                SetFloat($"Lights[{i}].OuterAngle", LightSources[i].OuterAngle);
                SetFloat($"Lights[{i}].Constant", LightSources[i].Constance);
                SetFloat($"Lights[{i}].Linear", LightSources[i].Linear);
                SetFloat($"Lights[{i}].Quaderic", LightSources[i].Quaderic);
                SetInt($"Lights[{i}].LightType", LightSources[i].LightType);
            }

        }

        private void Setbool(string v, bool isBlin)
        {
            throw new NotImplementedException();
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
