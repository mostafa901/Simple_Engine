using OpenTK;
using System;

namespace Simple_Engine.Engine.Core.Events
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