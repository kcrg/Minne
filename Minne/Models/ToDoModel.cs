using Newtonsoft.Json;
using System;

namespace Minne.Models
{
    public class ToDoModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }
    }
}
