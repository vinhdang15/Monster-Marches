using Newtonsoft.Json;
using UnityEngine;

public class Vector2Converter : JsonConverter<Vector2>
{
    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WriteEndObject();
    }

    public override Vector2 ReadJson(JsonReader reader, System.Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        float x = 0f, y = 0f;
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propName = reader.Value.ToString();
                reader.Read();
                if (propName == "x") x = (float)(double)reader.Value;
                else if (propName == "y") y = (float)(double)reader.Value;
            }
        }
        return new Vector2(x, y);
    }
}
