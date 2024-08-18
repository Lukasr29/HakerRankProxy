using HakerRankProxy.App.Converters;
using System.Text.Json.Serialization;

namespace HakerRankProxy.App.Models
{
    [JsonConverter(typeof(StoryJsonConverter))]
    public class Story
    {
        public int Id { get; set; }

        public string? Title { get; set; } = string.Empty;

        public string? Uri { get; set; } = string.Empty;

        public string? PostedBy { get; set; } = string.Empty;

        public int Score { get; set; }

        public int CommentCount { get; set; }

        public DateTime Timetamp { get; set; }
    }
}
