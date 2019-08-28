﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_CSharp.Geometery;
using OpenTK;
using OpenTK.Graphics.OpenGL;
#if RAAPSII
using Revit_Lib;
using Utility; 
#endif

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
#if RAAPSII
			return File.ReadAllText(Constants.conpaths(paths.BundlePath) + "\\" + UT_Rvt.RevitProduct + "\\shaders\\ObjectColor.frag"); 
#else
			return File.ReadAllText("shaders\\ObjectColor.frag");
#endif

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
