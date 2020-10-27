using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using Shared_Lib.Extention.Serialize_Ex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Core.Serialize
{
    public static class Import
    {
        public enum filter
        {
            InsituModel
        }
        public static string GetFilter(filter filterType)
        {
            switch (filterType)
            {
                case filter.InsituModel:
                    {
                        return "InsituModel files|*.ssd";
                    }
                    break;
                default:
                    return "";
            }
        }
        public static T LoadModel<T>(string path) where T : IRenderable
        {

            return File.ReadAllText(path).JDeserialize<T>(JsonTools.GetSettings());

        }
    }
}
