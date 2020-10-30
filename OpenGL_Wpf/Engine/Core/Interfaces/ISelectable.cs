using Simple_Engine.Engine.Geometry;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Render;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface ISelectable : IDrawable
    { 
        void Set_Selected(bool value);
        bool GetSelected();

    }
}