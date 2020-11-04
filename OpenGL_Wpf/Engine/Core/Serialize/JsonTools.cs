using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Simple_Engine.Engine.Core.Serialize
{
    internal class JsonTools
    {
        public static JsonSerializerSettings GetSettings()
        {
            var js = new JsonSerializerSettings();
            js.Converters.Add(new VectorConverter());
            // js.Formatting = Formatting.Indented;
#if false
            js.ContractResolver = new JsonTools.JsonResolver(
               new List<Type>
               {
                typeof(ImgUI_Controls),
                typeof(KeyControl),
                typeof(List<ImgUI_Controls>),
                typeof(List<ClipPlan>),
               });
#endif

            return js;
        }

        public class JsonResolver : DefaultContractResolver
        {
            private readonly HashSet<Type> ignoreProps;

            public JsonResolver(IEnumerable<Type> propNamesToIgnore)
            {
                this.ignoreProps = new HashSet<Type>(propNamesToIgnore);
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);

                if (this.ignoreProps.Contains(property.PropertyType))
                {
                    property.ShouldSerialize = _ => false;
                }
                return property;
            }
        }

        private class VectorConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return
                objectType == typeof(OpenTK.Vector2) ||
                objectType == typeof(OpenTK.Vector3) ||
                objectType == typeof(OpenTK.Vector4);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var temp = JObject.Load(reader);
                if (objectType == typeof(Vector2))
                {
                    return new OpenTK.Vector2(((float?)temp["X"]).GetValueOrDefault(), ((float?)temp["Y"]).GetValueOrDefault());
                }
                else if (objectType == typeof(Vector3))
                {
                    return new OpenTK.Vector3(((float?)temp["X"]).GetValueOrDefault(), ((float?)temp["Y"]).GetValueOrDefault(), ((float?)temp["Z"]).GetValueOrDefault());
                }

                return new OpenTK.Vector4(((float?)temp["X"]).GetValueOrDefault(), ((float?)temp["Y"]).GetValueOrDefault(), ((float?)temp["Z"]).GetValueOrDefault(), ((float?)temp["W"]).GetValueOrDefault());
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var type = value.GetType();
                if (type == typeof(Vector2))
                {
                    var vec = (OpenTK.Vector2)value;
                    serializer.Serialize(writer, new { X = vec.X, Y = vec.Y });
                }
                if (type == typeof(Vector3))
                {
                    var vec = (OpenTK.Vector3)value;
                    serializer.Serialize(writer, new { X = vec.X, Y = vec.Y, Z = vec.Z });
                }
                if (type == typeof(Vector4))
                {
                    var vec = (OpenTK.Vector4)value;
                    serializer.Serialize(writer, new { X = vec.X, Y = vec.Y, Z = vec.Z, W = vec.W });
                }
            }
        }
    }
}