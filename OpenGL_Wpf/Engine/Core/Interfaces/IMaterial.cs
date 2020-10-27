using Simple_Engine.Views.ThreeD.Engine.Opticals;
using Simple_Engine.Views.ThreeD.Engine.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Core.Interfaces
{
    public interface IMaterial : IRenderable
    {
        public Gloss Glossiness { get; set; }


    }
}
