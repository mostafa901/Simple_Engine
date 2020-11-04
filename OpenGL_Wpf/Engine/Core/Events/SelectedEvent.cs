using Simple_Engine.Engine.Core.Interfaces;
using System;

namespace Simple_Engine.Engine.Core.Events
{
    public class SelectedEvent : EventArgs
    {
        public ISelectable Model { get; set; }

        public SelectedEvent(ISelectable model)
        {
            Model = model;
        }
    }
}