using System;

namespace Simple_Engine.Engine.Core.AnimationSystem.Events

{
    public class AnimationUpdate_Event : EventArgs
    {
        public AnimationUpdate_Event(float etime)
        {
            Etime = etime;
        }

        public float Etime { get; }
    }
}