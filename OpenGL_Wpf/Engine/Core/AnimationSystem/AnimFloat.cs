using Simple_Engine.Engine.Core.AnimationSystem;
using Simple_Engine.Engine.Core.AnimationSystem.Events;
using Simple_Engine.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Engine.Engine.Core
{
    public class AnimFloat
    {
        private float diffVector = new float();
        private double keyFramDuration = 0;
        private List<Float_KeyFrame> KeyFrames = new List<Float_KeyFrame>();

        private double Timeelapsed = 0;

        public Action<float> AnimationAction { get; }
        public float End { get; }
        public IRenderable Model { get; }
        public float Start { get; }
        public bool Completed { get; set; }

        public event EventHandler<AnimationFinished_Event> OnFinish;

        public AnimFloat(IRenderable model, double duration, float start, float end, Action<float> animationAction)
        {
            Model = model;

            Start = start;
            End = end;
            AnimationAction = animationAction;
            GenrateKeyFrames(duration, end);
            AnimationMaster.OnUpdate += AnimFloat_OnUpdate;
            OnFinish += AnimFloat_OnFinish;
        }

        private void AnimFloat_OnFinish(object sender, AnimationFinished_Event e)
        {
            AnimationMaster.OnUpdate -= AnimFloat_OnUpdate;
            OnFinish -= AnimFloat_OnFinish;
        }

        private void AnimFloat_OnUpdate(object sender, AnimationUpdate_Event e)
        {
            Timeelapsed += e.Etime;
            var keys = CurrentandPreviousFrame();
            if (Completed)
            {
                OnFinish?.Invoke(null, null);
                return;
            }

            Timeelapsed = Math.Min(Timeelapsed, keys[1].timeStamp);

            var perc = Timeelapsed / keyFramDuration;
            var moveValue = keys[0].Position + diffVector * (float)perc;

            AnimationAction(moveValue);
        }

        private Float_KeyFrame[] CurrentandPreviousFrame()
        {
            Float_KeyFrame[] keysets = new Float_KeyFrame[2];
            for (int i = 1; i < KeyFrames.Count; i++)
            {
                var key = KeyFrames.ElementAt(i);

                if (key.timeStamp > Timeelapsed)
                {
                    var key0 = KeyFrames.ElementAt(i - 1);
                    var key1 = key;

                    diffVector = key1.Position - key0.Position;
                    keyFramDuration = key1.timeStamp - key0.timeStamp;
                    keysets[0] = key0;
                    keysets[1] = key1;
                    return keysets;
                }
                else
                {
                    continue;
                }
            }

            Completed = true;
            return null;
        }

        private void GenrateKeyFrames(double duration, float end)
        {
            var keyinitial = new Float_KeyFrame()
            {
                Position = Start,
                timeStamp = 0
            };

            var keyEnd = new Float_KeyFrame()
            {
                Position = end,
                timeStamp = duration
            };
            KeyFrames.Add(keyinitial);
            KeyFrames.Add(keyEnd);
        }
    }
}