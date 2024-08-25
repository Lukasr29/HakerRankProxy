using HackerRankProxy.App.Models;
using System.Text.Json;

namespace HackerRankProxy.Tests
{
    public class StoryJsonConverterTests
    {
        [Fact]
        public void ConverterMapsJsonToObject()
        {
            var anonObject = new
            {
                id = 123,
                title = "title",
                url = "url",
                by = "by",
                score = 100,
                descendants = 445,
                time = 1724581081
            };

            string json = JsonSerializer.Serialize(anonObject);

            var story = JsonSerializer.Deserialize<Story>(json);

            Assert.NotNull(story);
            Assert.Equal(anonObject.id, story.Id);
            Assert.Equal(anonObject.by, story.PostedBy);
            Assert.Equal(anonObject.descendants, story.CommentCount);
            Assert.Equal(anonObject.score, story.Score);
            Assert.Equal(anonObject.title, story.Title);
            Assert.Equal(anonObject.url, story.Uri);
            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(anonObject.time).UtcDateTime, story.Timestamp);
        }

        [Fact]
        public void ConverterMapsStoryToExpectedJson()
        {
            var anonObject = new
            {
                id = 123,
                title = "title",
                url = "url",
                by = "by",
                score = 100,
                descendants = 445,
                time = 1724581081
            };

            string json = JsonSerializer.Serialize(anonObject);

            var story = JsonSerializer.Deserialize<Story>(json);

            Assert.NotNull(story);

            var storyAsJson = JsonSerializer.Serialize(story);


            Assert.Equal("{\"title\":\"title\",\"uri\":\"url\",\"postedBy\":\"by\",\"score\":100,\"commentsCount\":445,\"time\":\"2024-08-25T10:18:01.0000000Z\"}", storyAsJson);
        }
    }
}
