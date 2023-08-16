using Newtonsoft.Json;

namespace GrazerCore.Tool
{
    public class JsonHelper
    {
        public static string SerializeObject(object targetObject)
        {
            return JsonConvert.SerializeObject(targetObject);
        }

        public static string SerializeObject(object targetObject, ReferenceLoopHandling loopHandling)
        {
            var jsonSetting = new JsonSerializerSettings();
            jsonSetting.ReferenceLoopHandling = loopHandling;
            
            return JsonConvert.SerializeObject(targetObject, jsonSetting);
        }

        public static T DeserializeObject<T>(string value)
        {
            var logicData = JsonConvert.DeserializeObject<T>(value);
            return logicData;
        }
    }
}
