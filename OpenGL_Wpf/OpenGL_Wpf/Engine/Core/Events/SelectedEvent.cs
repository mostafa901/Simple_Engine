using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Core.Events
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
