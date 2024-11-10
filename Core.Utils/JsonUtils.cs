using Newtonsoft.Json;

namespace Core.Utils
{
    public static class JsonUtils
    {
        public static string Serialize(this object obj, JsonSerializerSettings? jsonSerializerSettings = null) => JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        public static T? Deserialize<T>(this string obj, JsonSerializerSettings? jsonSerializerSettings = null) => JsonConvert.DeserializeObject<T>(obj);
    }
}
