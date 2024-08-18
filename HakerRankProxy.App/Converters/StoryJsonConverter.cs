using HakerRankProxy.App.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HakerRankProxy.App.Converters
{
    internal class StoryJsonConverter : JsonConverter<Story>
    {
        public override Story? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var story = new Story();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return story;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString()?.ToLowerInvariant() ?? string.Empty;
                    reader.Read();

                    switch (propertyName)
                    {
                        case "id":
                            story.Id = reader.GetInt32();
                            break;
                        case "title":
                            story.Title = reader.GetString();
                            break;
                        case "url":
                            story.Uri = reader.GetString();
                            break;
                        case "by":
                            story.PostedBy = reader.GetString();
                            break;
                        case "score":
                            story.Score = reader.GetInt32();
                            break;
                        case "descendants":
                            story.CommentCount = reader.GetInt32();
                            break;
                        case "time":
                            story.Timetamp = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()).UtcDateTime;
                            break;
                    }
                }
            }

            return story;
        }

        public override void Write(Utf8JsonWriter writer, Story value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("title", value.Title);
            writer.WriteString("uri", value.Uri);
            writer.WriteString("postedBy", value.PostedBy);
            writer.WriteNumber("score", value.Score);
            writer.WriteNumber("commentsCount", value.CommentCount);
            writer.WriteString("time", value.Timetamp.ToString("O"));

            writer.WriteEndObject();
        }
    }
}
