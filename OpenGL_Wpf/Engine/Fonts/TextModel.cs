using OpenTK;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Fonts
{
    public class RawTextModel : Base_Geo2D
    {
        public Offset CharOffset { get; set; }

        public List<Kerning> Kerns = new List<Kerning>();
        public CharBox CharacterBox;
        public string Id { get; set; }

        public Vector2 StartPosition = new Vector2();

        public int TextureAtlas_ID = 0;
        public float TextScale = .25f;

        public float LinePositionX { get; set; } = -0.5f;

        public RawTextModel()
        {
            Positions = new List<Vector2>();
            LocalTransform = Matrix4.Identity;
        }

        public override void BuildModel()
        {
            Build_DefaultModel();
            Setup_TextureCoordinates(1 / 372f, 1 / 363f);
            UploadVAO();
        }

        public override void UploadVAO()
        {
            if (Renderer == null)
            {
                Renderer = new FontRender(this);
            }
            Renderer.RenderModel();
        }

        public Vector2 GetOffset(RawTextModel previouseChr)
        {
            Vector2 offset = new Vector2(0, CharOffset.Y);

            foreach (var k in Kerns)
            {
                if (k.Id.Equals(previouseChr.Id))
                {
                    offset += new Vector2(k.Advanced, 0);
                    break;
                }
            }
            return offset;
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            var t0 = new Vector2(CharacterBox.Left * xScale, CharacterBox.Top * yScale);
            var t1 = new Vector2(CharacterBox.Left * xScale, (CharacterBox.Top + CharacterBox.Height) * yScale);
            var t2 = new Vector2((CharacterBox.Left + CharacterBox.Width) * xScale, CharacterBox.Top * yScale);
            var t3 = new Vector2((CharacterBox.Left + CharacterBox.Width) * xScale, (CharacterBox.Top + CharacterBox.Height) * yScale);

            TextureCoordinates = new List<Vector2>();
            TextureCoordinates.Add(t0);
            TextureCoordinates.Add(t1);
            TextureCoordinates.Add(t2);
            TextureCoordinates.Add(t3);
        }

        public override void Setup_Normals()
        {
            //Already loaded from DEfaults
        }

        public override void Setup_Indeces()
        {
            //Already loaded from DEfaults
        }

        public override void Setup_Position()
        {
        }

        public override void Live_Update(Base_Shader ShaderModel)
        {
            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
        }

        public override List<face> generatefaces()
        {
            throw new System.NotImplementedException();
        }
    }

    public struct CharBox
    {
        public int Width;
        public int Height;
        public int Top;
        public int Left;
    }

    public struct Kerning
    {
        public int Advanced;
        public string Id;
    }

    public struct Offset
    {
        public int X;
        public int Y;
    }
}