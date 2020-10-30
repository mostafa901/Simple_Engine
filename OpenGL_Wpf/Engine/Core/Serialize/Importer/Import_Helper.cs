using Simple_Engine.Engine.Core.Interfaces;
using Shared_Lib.Extention.Serialize_Ex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Core.Serialize.Importer
{
    public static class Import_Helper
    {
        public enum filter
        {
            Simple_EngineModel,
            Json
        }
        public static string GetFilter(filter filterType)
        {
            switch (filterType)
            {
                case filter.Simple_EngineModel:
                        return "Simple_EngineModel files|*.ssd";
                case filter.Json:
                    return "Json files|*.json";

                default:
                    return "";
            }
        }
       
    }
}
