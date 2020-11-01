namespace Simple_Engine.Engine.Core.Serialize.Importer
{
    public static class Import_Helper
    {
        public enum filter
        {
            All,
            Simple_EngineModel,
            Json,
            OBJ
        }

        public static string GetFilter(filter filterType)
        {
            switch (filterType)
            {
                case filter.Simple_EngineModel:
                    return "Simple_EngineModel files|*.ssd";

                case filter.Json:
                    return "Json files|*.json";
                case filter.OBJ:
                    return "Obj files|*.obj";

                case filter.All:
                    return string.Join("|", GetFilter(filter.Json), GetFilter(filter.Simple_EngineModel));

                default:
                    return "";
            }
        }
    }
}