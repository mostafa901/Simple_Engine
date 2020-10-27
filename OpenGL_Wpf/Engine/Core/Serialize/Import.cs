using Simple_Engine.Engine.Core.Interfaces;
using Shared_Lib.Extention.Serialize_Ex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Core.Serialize
{
    public static class Import
    {
        public enum filter
        {
            Simple_EngineModel
        }
        public static string GetFilter(filter filterType)
        {
            switch (filterType)
            {
                case filter.Simple_EngineModel:
                    {
                        return "Simple_EngineModel files|*.ssd";
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
