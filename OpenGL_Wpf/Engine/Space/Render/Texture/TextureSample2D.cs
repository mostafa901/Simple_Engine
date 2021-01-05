using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using System.Drawing;

namespace Simple_Engine.Engine.Render.Texture
{
    public class TextureSample2D : Base_Texture
    {
        public TextureSample2D(string imgPath, TextureUnit textureUnit) : base(TextureMode.Texture2D)
        {
            Setup_2DTexture(imgPath, textureUnit);
        }

        public TextureSample2D(TextureUnit textureUnit, Vector4 color = new Vector4()) : base(TextureMode.Texture2D)
        {
            //Bitmap img = Shared_Library.Extention.Graphics_Ex.Image_Ex.ImageFromColor(10, 10, (int)(color.X * 225), (int)(color.Y * 225), (int)(color.Z * 225), (int)(color.W * 225));

            Bitmap img = new Bitmap(1, 1);
            using (Graphics gfx = Graphics.FromImage(img))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(color.X * 225), (int)(color.Y * 225), (int)(color.Z * 225), (int)(color.W * 225))))
            {
                gfx.FillRectangle(brush, 0, 0, 1, 1);
            }
            var filename = System.IO.Path.GetTempFileName() + ".jpg";
            img.Save(filename);
            img.Dispose();

            Setup_2DTexture("", textureUnit);
        }
    }
}