﻿using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.ToolBox;
using Microsoft.Win32;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Core
{
    public class AnimVector3
    {
        public bool Completed = false;
        private Vector3 diffVector = new Vector3();
        private double keyFramDuration = 0;
        private List<KeyFrame> KeyFrames = new List<KeyFrame>();

        private double Timeelapsed = 0;

        public AnimVector3(IRenderable model, double duration, Vector3 start, Vector3 end, Action<Vector3> animationAction)
        {
            Model = model;

            Start = start;
            End = end;
            AnimationAction = animationAction;
            GenrateKeyFrames(duration, end);
        }

        public Action<Vector3> AnimationAction { get; }
        public Vector3 End { get; }
        public IRenderable Model { get; }
        public Vector3 Start { get; }
        public void Update()
        {
            Timeelapsed += DisplayManager.UpdatePeriod;
            var keys = CurrentandPreviousFrame();
            if (Completed) return;

            Timeelapsed = Math.Min(Timeelapsed, keys[1].timeStamp);

            var perc = Timeelapsed / keyFramDuration;
            var moveValue = keys[0].Position + diffVector * (float)perc;

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
                    keysets[0] = KeyFrames.ElementAt(i - 1);

                    keysets[1] = key;
                    diffVector = keysets[1].Position - keysets[0].Position;
                    keyFramDuration = keysets[1].timeStamp - keysets[0].timeStamp;
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
            var keyinitial = new KeyFrame()
            {
                Position = Start,
                timeStamp = 0
            };

            var keyEnd = new KeyFrame()
            {
                Position = end,
                timeStamp = duration
            };
            KeyFrames.Add(keyinitial);
            KeyFrames.Add(keyEnd);
        }
    }

    public class KeyFrame
    {
        public int Id;
        public Vector3 Position;
        public double timeStamp;
    }
}