using Simple_Engine.Engine.Core.AnimationSystem.Events;
using System;

namespace Simple_Engine.Engine.Core.AnimationSystem
{
    public static class AnimationMaster
    {
        public static event EventHandler<AnimationUpdate_Event> OnUpdate;

        public static void Render(float time)
        {
            OnUpdate?.Invoke(null, new AnimationUpdate_Event(time));
        }
    }
}