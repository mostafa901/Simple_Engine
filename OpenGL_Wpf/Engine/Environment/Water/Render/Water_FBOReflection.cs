using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.ToolBox;
using System;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Water.Render
{
    public class Water_FBOReflection : FBO
    {
        public Vector4 ClipPlan { get; set; }

        private readonly Base_Shader stensilShader;

        public Water_FBOReflection(int _width, int _height) : base(_width, _height)
        {
            Name = FboName.WorldReflection;
            Setup_Defaults(true);
            stensilShader = new Vertex_Shader(ShaderPath.Color);
            WrapeTo(TextureDepthId, TextureWrapMode.ClampToBorder);
            WrapeTo(TextureId, TextureWrapMode.ClampToBorder);
        }

        public override void PreRender(Base_Shader ShaderModel)
        {
            base.PreRender(ShaderModel);
            //should be same distance below water Height, but since Water height is 0 then we just inverted Y

            ShaderModel.SetVector4(ShaderModel.Location_ClipPlanY, ClipPlan);
        }

        //todo: Issue no #4 Water reflection not reflecting correctly
        public override void RenderFrame(List<IDrawable> models)
        {
            RenderStencil();
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            var val = Name == FboName.WorldReflection ? 0 : 1;
            GL.StencilFunc(StencilFunction.Equal, 1, 1);

            GL.Enable(EnableCap.ClipDistance1);

            foreach (var model in models)
            {
                if (!model.AllowReflect) continue;
                if (model == StenciledModel) continue;
                if (Name == FboName.WorldReflection)
                {
                    negate(model);
                    model.CullMode = CullFaceMode.Front;
                }
                RenderFrame(model);
                if (Name == FboName.WorldReflection)
                {
                    if (model is SkyBox)
                    {
                    }
                    else
                    {
                        model.CullMode = CullFaceMode.Back;
                    }

                    negate(model, -1);
                }
            }

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.ClipDistance1);
        }

        //todo: there is bug here: when a dragon is moving the reflection is based to 0 level, and correctly realigned when the dragon stops.
        private static void negate(IDrawable model, int sign = -1)
        {
            Vector3 scalarVector = new Vector3(1, -1, 1);

            var geo = model as Base_Geo;

            if (model.GetShaderModel().EnableInstancing && model is Base_Geo)
            {
                foreach (var mesh in geo.Meshes)
                {
                    var pos = mesh.LocalTransform.ExtractTranslation();
                    mesh.Scale(scalarVector);
                    mesh.LocalTransform = eMath.MoveWorld(mesh.LocalTransform, new Vector3(0, sign * 2 * pos.Y, 0));
                }
            }
            else
            {
                var pos = model.LocalTransform.ExtractTranslation();
                model.Scale(scalarVector);
                model.LocalTransform = eMath.MoveWorld(model.LocalTransform, new Vector3(0, sign * 2 * pos.Y, 0));
            }
        }

        public override void RenderFrame(IDrawable model)
        {
            model.PrepareForRender(model.GetShaderModel());
            PreRender(model.GetShaderModel());

            model.GetShaderModel().SetMatrix4(model.GetShaderModel().Location_LocalTransform, model.LocalTransform);

            //invertnormals since the model is scaled y= -1
            model.GetShaderModel().SetBool(model.GetShaderModel().Location_InvertNormal, Name == FboName.WorldReflection);

            model.Renderer.Draw();

            PostRender(model.GetShaderModel());
            model.GetShaderModel().Stop();
        }

        private bool RenderStencil()
        {
            //rendered = true;
            GL.ColorMask(false, false, false, false);
            GL.DepthMask(false);

            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilFunc(StencilFunction.Always, 1, 1);
            GL.StencilMask(1);

            StenciledModel.PrepareForRender(stensilShader);
            StenciledModel.Renderer.Draw();
            StenciledModel.GetShaderModel().Stop();

            GL.DepthMask(true);
            GL.ColorMask(true, true, true, true);

            return true;
        }

        public override void PostRender(Base_Shader ShaderModel)
        {
        }

        public override void Live_Update(Base_Shader ShaderModel)
        {
            throw new NotImplementedException();
        }
    }
}