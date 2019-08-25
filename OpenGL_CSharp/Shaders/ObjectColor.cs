using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_CSharp.Geometery;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Utility;

namespace OpenGL_CSharp.Shaders
{
    public class ObjectColor : BaseShader
    {
        public int fragshad, diffuseid, specularid = -1;
      

        public ObjectColor()
        {
            LoadFrag();
        } 
          
          string ObjectColorFragSource()
        {
            return File.ReadAllText(Constants.conpaths(paths.BundlePath)+ "\\shaders\\ObjectColor.frag");
            
        }
        
        public void LoadFrag()
        {
            //setup shaders
            //load vertix/Fragment shader
            //---------------------------
            fragshad = CreateShader(ObjectColorFragSource(), ShaderType.FragmentShader);

            //create program, link shaders and test the results
            LinkShader(vershad, fragshad);

            SetupStrids(); 
             
            GetVariables();
        }


        public override void Use()
        {            
            base.Use();
            
        }

        public override void Dispose()
        {
             
            base.Dispose();
        }
    }
}
