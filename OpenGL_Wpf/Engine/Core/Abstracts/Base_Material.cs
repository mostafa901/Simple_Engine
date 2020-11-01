using OpenTK;
using Simple_Engine.Engine.Core.AnimationSystem;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Opticals;
using Simple_Engine.Engine.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Core.Abstracts
{
    public class Base_Material : IMaterial
    {
        public  Gloss Glossiness { get; set; }
        public string Name { get ; set ; }
        public int Id { get ; set ; }
        public IRenderable.BoundingBox BBX { get ; set ; }
        public ImgUI_Controls Ui_Controls { get ; set ; }
        public Vector4 DefaultColor { get  ; set  ; }
        public bool CastShadow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void BuildModel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Live_Update()
        {
           
        }

        public void Live_Update(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public IRenderable Load(string path)
        {
            throw new NotImplementedException();
        }

        public void PostRender(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public void PrepareForRender(Shader shaderModel)
        {
            throw new NotImplementedException();
        }

        public void RenderModel()
        {
            throw new NotImplementedException();
        }

        public string Save()
        {
            throw new NotImplementedException();
        }

        public void Render_UIControls()
        {
            throw new NotImplementedException();
        }

        public void UpdateBoundingBox()
        {
            throw new NotImplementedException();
        }

        public void Create_UIControls()
        {
            throw new NotImplementedException();
        }

        public virtual void UploadDefaults(Shader ShaderModel)
        {
            Glossiness?.UploadDefaults(ShaderModel);    
        }

         
    }
}
