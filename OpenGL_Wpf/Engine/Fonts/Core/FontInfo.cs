using OpenTK;
using Shared_Lib.Extention.Serialize_Ex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Simple_Engine.Views.ThreeD.Engine.Fonts.Core
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