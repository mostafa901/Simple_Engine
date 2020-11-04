using OpenTK;

namespace Simple_Engine.Engine.Fonts.Core
{
    public class FontInfo
    {
        public string Name { get; set; }
        public float Size { get; set; }
        public Vector4 Padding { get; set; }
        public int LineHeight { get; set; }
        public int baseValue { get; set; }
        public float ImgWidth { get; set; }
        public float ImgHeight { get; set; }
        public string ImgPath { get; set; }
    }
}