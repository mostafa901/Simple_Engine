using Simple_Engine.Views.ThreeD.Engine.Core;
using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Simple_Engine.Views.ThreeD.Engine.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine
{
    public class AnimationComponent
    {
        public List<AnimVector3> AnimPositions = new List<AnimVector3>();
        private IRenderable Model;

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
