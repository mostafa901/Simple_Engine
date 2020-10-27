using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space;
using Simple_Engine.ToolBox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared_Lib.Extention;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Water.Render
{
    public class Water_FBOReflection : FBO
    {
        public Vector4 ClipPlan { get; set; }

        private CameraModel camera;

        private Shader stensilShader;

        public Water_FBOReflection(int _width, int _height) : base(_width, _height)
        {
            Name = FboName.WorldReflection;
            Setup_Defaults(true);
            camera = new CameraModel(Game.Context.ActiveScene,false);
            stensilShader = new Shader(ShaderMapType.LightnColor, ShaderPath.Color);
            WrapeTo(TextureDepthId, TextureWrapMode.ClampToBorder);
            WrapeTo(TextureId, TextureWrapMode.ClampToBorder);
        }

        public override void PreRender(Shader ShaderModel)
        {
            base.PreRender(ShaderModel);
            //should be same distance below water Height, but since Water height is 0 then we just inverted Y

            ShaderModel.SetVector4(ShaderModel.Location_ClipPlanY, ClipPlan);
        }

        public override void RenderFrame(List<IDrawable> models)
        {
            GL.Enable(EnableCap.ClipDistance1);
            RenderStencil();
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            GL.StencilFunc(StencilFunction.Equal, 1, 1);

            foreach (var model in models)
            {
                if (!model.AllowReflect) continue;
                if (model == StenciledModel) continue;

                if (Name == FboName.WorldReflection)
                {
                    negate(model, 1);
                    model.CullMode = CullFaceMode.Front;
                }

                RenderFrame(model);

                if (Name == FboName.WorldReflection)
                {
                    negate(model, -1);
                    if (!(model is SkyBox))
                    {
                        model.CullMode = CullFaceMode.Back;
                    }
                }
            }

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.ClipDistance1);
        }

        //todo: there is bug here: when a dragon is moving the the reflection is based to 0 level, and correctly realigned when the dragon stops.
        private static void negate(IDrawable model, int sign = -1)
        {
            Vector3 scalarVector = new Vector3(1, -1, 1);
           
            var geo = model as Base_Geo;

            if (model.ShaderModel.EnableInstancing && model is Base_Geo)
            {
                foreach (var mesh in geo.Meshes)
                {
                    var pos = mesh.LocalTransform.ExtractTranslation();
                    mesh.Scale(scalarVector);
                    mesh.LocalTransform = eMath.MoveLocal(mesh.LocalTransform, new Vector3(0, sign * 2 * pos.Y, 0));
                }
            }
            else
            {
                var pos = model.LocalTransform.ExtractTranslation();
                model.Scale(scalarVector);
                model.LocalTransform = eMath.MoveLocal(model.LocalTransform, new Vector3(0, sign * 2 * pos.Y, 0));
            }
        }

        public override void RenderFrame(IDrawable model)
        {
            model.PrepareForRender(model.ShaderModel);
            PreRender(model.ShaderModel);

            model.ShaderModel.SetMatrix4(model.ShaderModel.Location_LocalTransform, model.LocalTransform);

            if (Name == FboName.WorldReflection)
            {
                Vector4 offclip = new Vector4(0, -1, 0, 0);
                model.ShaderModel.SetVector4(model.ShaderModel.Location_ClipPlanY, offclip);
            }

            model.ShaderModel.SetBool(model.ShaderModel.Location_isReflection, Name == FboName.WorldReflection);

            model.Renderer.Draw();

            PostRender(model.ShaderModel);
            model.ShaderModel.Stop();
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
            StenciledModel.ShaderModel.Stop();

            GL.DepthMask(true);
            GL.ColorMask(true, true, true, true);

            return true;
        }

        public override void PostRender(Shader ShaderModel)
        {
        }

        public override void Live_Update(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }
    }
}