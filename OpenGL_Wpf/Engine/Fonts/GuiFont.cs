
using Simple_Engine.Engine.Core;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Fonts.Core;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared_Lib.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Engine.Engine.Core.AnimationSystem;

namespace Simple_Engine.Engine.Fonts
{
    public class GuiFont : IRenderable
    {
        private List<CharacterModel> characterModels;

        public string Text { get; }
        public float LineWidth { get; }
        public Shader ShaderModel { get; set; }
        public Base_Texture TextureModel { get; set; }
        public Vector2 TextPosition { get; set; }
        public string Name { get ; set ; }
        public int Id { get ; set ; }
        public IRenderable.BoundingBox BBX { get ; set ; }
        public ImgUI_Controls Ui_Controls { get ; set ; }
        public Vector4 DefaultColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CastShadow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public GuiFont(string text, float lineWidth)
        {
            Text = text;
            LineWidth = lineWidth;
        }

        public void BuildModel()
        {
            characterModels = new List<CharacterModel>();
            ShaderModel = new FontShader(ShaderMapType.LoadColor, ShaderPath.Font);
            TextureModel = new TextureSample2D(FontFactory.imgFontPath, TextureUnit.Texture0);
            ShaderModel.UploadDefaults(null);

            CharacterModel prev = null;
            float offsetx = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                var c = Text[i];
                var tmodel = FontFactory.GetCharacterModel(c);

                tmodel.ShaderModel = ShaderModel;
                tmodel.TextureModel = TextureModel;

                tmodel.BuildModel();
                tmodel.UploadVAO();
                if (offsetx > LineWidth)
                {
                    offsetx = 0;
                    TextPosition += new Vector2(0, -tmodel.scaledHeight - .035f);
                    prev = null;
                }

                tmodel.MoveTo(new Vector3(TextPosition));

                if (prev != null)
                {
                    offsetx += tmodel.GetWidth() + tmodel.XOffset + prev.Advance;
                    tmodel.MoveWorld(new Vector3(offsetx, 0, 0) * tmodel.scaleValue);
                }
                tmodel.MoveWorld(new Vector3(0, -tmodel.YOffset, 0) * tmodel.scaleValue);

                tmodel.Scale(tmodel.scaleValue);

                prev = tmodel;
                characterModels.Add(tmodel);
            }
        }

        

        public void UploadVAO()
        {
            PrepareForRender(ShaderModel);

            for (int i = 0; i < characterModels.Count; i++)
            {
                var rawModel = characterModels[i];
                rawModel.Live_Update(ShaderModel);

                rawModel.Renderer.Draw();
            }

            PostRender(ShaderModel);
        }

        

        public void CleanUp()
        {
        }

        public void Dispose()
        {
            foreach (var raw in characterModels)
            {
                raw.Renderer.Dispose();
            }
        }

        public void Live_Update(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public void PostRender(Shader ShaderModel)
        {
            ShaderModel.Stop();
            GL.Enable(EnableCap.DepthTest);
        }

        public void UploadDefaults(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public void PrepareForRender(Shader shaderModel)
        {

            GL.Disable(EnableCap.DepthTest);
            ShaderModel.Use();
            TextureModel.Live_Update(ShaderModel);
        }

        public string Save()
        {
            throw new NotImplementedException();
        }

        public IRenderable Load(string path)
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
    }
}