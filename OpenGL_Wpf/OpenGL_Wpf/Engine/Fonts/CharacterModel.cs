using com.sun.jdi;
using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Fonts.Core;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Engine.Core.Events;
using InSitU.Views.ThreeD.Engine.Particles;
using InSitU.Views.ThreeD.Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Fonts
{
    public class CharacterModel : Base_Geo2D
    {
        public FontInfo Finfo { get; set; }
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int Advance { get; set; }

        public float scaleValue { get; private set; }
        public float scaledHeight;

        public CharacterModel(FontInfo _finfo)
        {
            Finfo = _finfo;
            //Ratio of the Character Height size 18 on a screen 244 pixels
            //https://websemantics.uk/tools/font-size-conversion-pixel-point-em-rem-percent/
            scaledHeight = 48 / (float)Game.Context.Height;
            scaleValue = scaledHeight / (float)Finfo.LineHeight;
        }

        public override void CloneModel(Base_Geo sourceModel)
        {
            base.CloneModel(sourceModel);
            var model = sourceModel as CharacterModel;
            Finfo = model.Finfo;
            XOffset = model.XOffset;
            YOffset = model.YOffset;
            Advance = model.Advance;
            scaleValue = model.scaleValue;
            scaledHeight = model.scaledHeight;
        }

        public override void BuildModel()
        {
            Build_DefaultModel();
            Setup_Position();
            Setup_TextureCoordinates(1 / Finfo.ImgWidth, 1 / Finfo.ImgHeight);
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            TextureCoordinates = new List<Vector2>();
            float totalx = (X + Finfo.Padding.X);
            float totaly = (Y + Finfo.Padding.Y);
            var v0 = new Vector2(totalx, totaly);
            var v1 = new Vector2(totalx, totaly + GetHeight());
            var v2 = new Vector2(totalx + GetWidth(), totaly);
            var v3 = new Vector2(totalx + GetWidth(), totaly + GetHeight());

            TextureCoordinates.Add(v0 * new Vector2(xScale, yScale));
            TextureCoordinates.Add(v1 * new Vector2(xScale, yScale));
            TextureCoordinates.Add(v2 * new Vector2(xScale, yScale));
            TextureCoordinates.Add(v3 * new Vector2(xScale, yScale));
        }

        public override void Live_Update(Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
        }

        public override void RenderModel()
        {
            Renderer = new FontRender(this);
            Renderer.RenderModel();
        }

        public override void Setup_Normals()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Indeces()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Position()
        {
        }
    }
}