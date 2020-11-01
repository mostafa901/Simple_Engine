using OpenTK;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Engine.Engine.Core
{
    public class AnimFloat
    {
        public bool Completed = false;
        private float diffVector = new float();
        private double keyFramDuration = 0;
        private List<KeyFrame> KeyFrames = new List<KeyFrame>();

        private double Timeelapsed = 0;

        public Action<float> AnimationAction { get; }
        public float End { get; }
        public IRenderable Model { get; }
        public float Start { get; }

        public AnimFloat(IRenderable model, double duration, float start, float end, Action<float> animationAction)
        {
            Model = model;

            Start = start;
            End = end;
            AnimationAction = animationAction;
            GenrateKeyFrames(duration, end);
        }

        public void Update()
        {
            Timeelapsed += DisplayManager.UpdatePeriod;
            var keys = CurrentandPreviousFrame();
            if (Completed) return;

            Timeelapsed = Math.Min(Timeelapsed, keys[1].timeStamp);

            var perc = Timeelapsed / keyFramDuration;
            var moveValue = ((Float_KeyFrame)keys[0]).Position + diffVector * (float)perc;

            AnimationAction(moveValue);
        }

        private KeyFrame[] CurrentandPreviousFrame()
        {
            KeyFrame[] keysets = new KeyFrame[2];
            for (int i = 1; i < KeyFrames.Count; i++)
            {
                var key = KeyFrames.ElementAt(i) as Float_KeyFrame;

                if (key.timeStamp > Timeelapsed)
                {
                    var key0 = KeyFrames.ElementAt(i - 1) as Float_KeyFrame;
                    var key1 = key;

                    diffVector = key1.Position - key0.Position;
                    keyFramDuration = key1.timeStamp - key0.timeStamp;
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