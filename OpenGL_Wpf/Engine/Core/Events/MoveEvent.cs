using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Core.Events
{
    public class MoveingEvent : EventArgs
    {
        public Matrix4 Transform { get; set; }

        public MoveingEvent(Matrix4 _Transform)
        {
            Transform = _Transform;
        }
    }
}
