using System;
using Newtonsoft.Json.Linq;

namespace ArcaneSurvivorsClient {
    public static class JsonUtility {
        public static void TryGetValue<T>(this JObject jObject, string fieldName, ref T value) {
            if (jObject.TryGetValue(fieldName, out JToken jToken))
                value = jToken.Value<T>();
        }

        public static T TryGetValue<T>(this JObject jObject, string fieldName, T defaultValue) {
            if (!jObject.ContainsKey(fieldName))
                return defaultValue;

            try {
                if (typeof(T).IsEnum)
                    return (T)Enum.Parse(typeof(T), jObject.Value<string>(fieldName));

                return jObject.Value<T>(fieldName);
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "JsonUtility.TryGetValue", $"Failed to get value from JObject.",
                    new[] { new LogElement("Exception", ex.ToString()), new LogElement("FieldName", fieldName) });

                return defaultValue;
            }
        }
    }
}