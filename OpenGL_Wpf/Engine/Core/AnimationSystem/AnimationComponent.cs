using Simple_Engine.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Engine.Engine.Core.AnimationSystem
{
    public class AnimationComponent
    {
        public event EventHandler<Animation_FinishedEvent> OnFinished;

        public List<AnimVector3> AnimPositions = new List<AnimVector3>();
        private IRenderable Model;

        private void onFinishedEvent(Animation_FinishedEvent e)
        {
            OnFinished?.Invoke(this, e);
        }

        public AnimationComponent(IRenderable cameraModel)
        {
            this.Model = cameraModel;
        }

        public void Update()
        {
            var lst = AnimPositions.ToList();
            foreach (var anim in lst)
            {
                if (anim.Completed)
                {
                    onFinishedEvent(new Animation_FinishedEvent());
                    AnimPositions.Remove(anim);
                    continue;
                }
                anim.Update();
            }
        }

        //todo: change this to an event triggered from the animation itself
        public void CleanCompleted()
        {
            var lst = AnimPositions.ToList();
            foreach (var item in lst)
            {
                if (item.Completed) AnimPositions.Remove(item);
            }
        }
    }
}