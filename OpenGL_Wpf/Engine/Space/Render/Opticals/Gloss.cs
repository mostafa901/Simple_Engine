using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Opticals
{
    public class Gloss : Base_Material
    {
        public float ShiningDamp { get; set; } = 0; //how must it is reflective and shine to the perfect reflected ray
        public float ReflectionIndex { get; set; } = 0;

        public Gloss(float _ReflectionIndex, float _ShiningDamp)
        {
            ReflectionIndex = _ReflectionIndex;
            ShiningDamp = _ShiningDamp; //decrease shiny by distance squared
        }

        public override void UploadDefaults(Shader ShaderModel)
        {
            ShaderModel.SetFloat(ShaderModel.ReflectionIndexLocation, ReflectionIndex);
            ShaderModel.SetFloat(ShaderModel.ShiningDampLocation, ShiningDamp);
        }
    }
}