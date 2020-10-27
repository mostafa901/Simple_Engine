using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Geometry.ThreeDModels;
using Microsoft.Office.Interop.Word;
using Ookii.Dialogs.Wpf;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Core.Abstracts
{
    public class FloorModel : IHelper
    {
        public Terran Parent;

        public FloorModel(Terran _Parent)
        {
            Parent = _Parent;
        }

        public void OnMoving(Base_Geo model)
        {
            var position = model.LocalTransform.ExtractTranslation();
            float height = Parent.GetTerrainHeight(position.Z, position.X);

            if (position.Y != height)
            {
                model.MoveTo(new Vector3(position.X, height, position.Z));
            }
        }
    }
}