using DocumentFormat.OpenXml.Drawing;
using Shared_Lib.Extention;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InSitU.Views.ThreeD.Engine.Fonts
{
    public class TextManager
    {
        public string ImgPath { get; }
        public float Scale { get; }

        private XmlDocument xmldoc;

        public TextManager(string xmlPath, string imgPath, float scale)
        {
            ImgPath = imgPath;
            Scale = scale;
            xmldoc = new XmlDocument();
            xmldoc.LoadXml(File.ReadAllText(xmlPath));
        }

        public RawTextModel GetModel(Char c)
        {
            var elements = xmldoc.DocumentElement.GetElementsByTagName("Char").Cast<XmlElement>();
            var xele = elements.FirstOrDefault(o => o.GetAttribute("code") == c.ToString());

            var model = new RawTextModel();
            model.Id = xele.GetAttribute("code");
            var offsetTxt = xele.GetAttribute("offset").Split(' ');
            model.CharOffset = new Offset()
            {
                X = int.Parse(offsetTxt[0]),
                Y = int.Parse(offsetTxt[1])
            };

            string[] rectxt = xele.GetAttribute("rect").Split(' '); ;

            model.CharacterBox = new CharBox()
            {
                Left = int.Parse(rectxt[0]),
                Top = int.Parse(rectxt[1]),
                Width = int.Parse(rectxt[2]),
                Height = int.Parse(rectxt[3])
            };
            if (xele.HasChildNodes)
            {
                foreach (XmlElement child in xele.ChildNodes)
                {
                    var kern = new Kerning();
                    kern.Advanced = int.Parse(child.GetAttribute("advance"));
                    kern.Id = child.GetAttribute("id");
                    model.Kerns.Add(kern);
                }
            }
            model.Width = int.Parse(xele.GetAttribute("width"));
            model.Height = model.CharacterBox.Height;
            model.TextScale = Scale;
            return model;
        }
    }
}