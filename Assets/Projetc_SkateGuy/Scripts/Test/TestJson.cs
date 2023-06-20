using UnityEngine;
using Newtonsoft.Json;
using SkateGuy.GameElements.EnemyLogicData;
using System;

namespace SkateGuy.Test
{
    public class TestJson : MonoBehaviour
    {

    }

    public class Person
    {
        public string name { get; set; }
        public int? age { get; set; }
        public string city { get; set; }
    }

    public class Vec2Conv : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Vector2))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = serializer.Deserialize(reader);
            var iv = JsonConvert.DeserializeObject<Vector2>(t.ToString());
            Debug.Log(iv);
            return iv;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector2 v = (Vector2)value;

            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(v.x);
            writer.WritePropertyName("y");
            writer.WriteValue(v.y);
            writer.WriteEndObject();
        }
    }
}
