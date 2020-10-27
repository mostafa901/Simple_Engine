using Simple_Engine.Views.ThreeD.Engine.Geometry;
using Simple_Engine.Views.ThreeD.Engine.Geometry.Core;
using Simple_Engine.Views.ThreeD.Engine.Geometry.InputControls;
using Simple_Engine.Views.ThreeD.Engine.Render;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Core.Interfaces
{
    public interface ISelectable : IDrawable
    { 
        void Set_Selected(bool value);
        bool GetSelected();

    }
}