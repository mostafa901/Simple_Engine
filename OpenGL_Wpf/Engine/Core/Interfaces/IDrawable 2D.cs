﻿using Simple_Engine.Engine.Geometry.Core;


using Simple_Engine.Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface IDrawable2D : IDrawable
    {
        public List<Vector2> Positions { get; set; }

    }
}