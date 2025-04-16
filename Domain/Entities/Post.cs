using System.Text.Json.Serialization;

namespace BlogSiggaApp.Domain.Entities
{
    public class Post
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;

        public bool IsValid() => !string.IsNullOrWhiteSpace(Title);
    }
}
