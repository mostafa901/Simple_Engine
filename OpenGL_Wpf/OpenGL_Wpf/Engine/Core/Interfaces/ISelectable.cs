using InSitU.Views.ThreeD.Engine.Geometry;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Engine.Geometry.InputControls;
using InSitU.Views.ThreeD.Engine.Render;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Core.Interfaces
{
    public interface ISelectable : IDrawable
    { 
        void Set_Selected(bool value);
        bool GetSelected();

    }
}