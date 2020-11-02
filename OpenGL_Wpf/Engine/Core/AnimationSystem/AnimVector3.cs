using OpenTK;
using Simple_Engine.Engine.Core.AnimationSystem.Events;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Engine.Engine.Core.AnimationSystem
{
    public class AnimVector3
    {
        public bool Completed = false;
        private Vector3 diffVector = new Vector3();
        private double keyFramDuration = 0;
        private List<Vector3_KeyFrame> KeyFrames = new List<Vector3_KeyFrame>();

        private double Timeelapsed = 0;

        public AnimVector3(IRenderable model, double duration, Vector3 start, Vector3 end, Action<Vector3> animationAction)
        {
            Model = model;

            Start = start;
            End = end;
            AnimationAction = animationAction;
            GenrateKeyFrames(duration, end);
            AnimationMaster.OnUpdate += AnimFloat_OnUpdate;
            OnFinish += AnimFloat_OnFinish;
        }

        public event EventHandler<AnimationFinished_Event> OnFinish;
        public Action<Vector3> AnimationAction { get; }
        public Vector3 End { get; }
        public IRenderable Model { get; }
        public Vector3 Start { get; }

        private void AnimFloat_OnFinish(object sender, AnimationFinished_Event e)
        {
            AnimationMaster.OnUpdate -= AnimFloat_OnUpdate;
            OnFinish -= AnimFloat_OnFinish;
        }

        private void AnimFloat_OnUpdate(object sender, AnimationUpdate_Event e)
        {
            Timeelapsed += DisplayManager.UpdatePeriod;
            var keys = CurrentandPreviousFrame();
            if (Completed)
            {
                OnFinish?.Invoke(null,null);
                return;
            }

            Timeelapsed = Math.Min(Timeelapsed, keys[1].timeStamp);

            var perc = Timeelapsed / keyFramDuration;
            var moveValue = ((Vector3_KeyFrame)keys[0]).Position + diffVector * (float)perc;

            AnimationAction(moveValue);
        }
        private KeyFrame[] CurrentandPreviousFrame()
        {
            KeyFrame[] keysets = new KeyFrame[2];
            for (int i = 1; i < KeyFrames.Count; i++)
            {
                var key = KeyFrames.ElementAt(i);

                if (key.timeStamp > Timeelapsed)
                {
                    var key0 = KeyFrames.ElementAt(i - 1) as Vector3_KeyFrame;
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

        private void GenrateKeyFrames(double duration, Vector3 end)
        {
            var keyinitial = new Vector3_KeyFrame()
            {
                Position = Start,
                timeStamp = 0
            };

            var keyEnd = new Vector3_KeyFrame()
            {
                Position = end,
                timeStamp = duration
            };
            KeyFrames.Add(keyinitial);
            KeyFrames.Add(keyEnd);
        }
    }
}