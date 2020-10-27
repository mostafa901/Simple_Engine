using InSitU.Views.ThreeD.Engine.Opticals;
using InSitU.Views.ThreeD.Engine.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Core.Interfaces
{
    public interface IMaterial : IRenderable
    {
        public Gloss Glossiness { get; set; }


    }
}
