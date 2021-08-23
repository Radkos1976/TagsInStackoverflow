using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TagsInStackoverflow
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class ExternalLink
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }
    }

    public class Collective
    {
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("external_links")]
        public List<ExternalLink> ExternalLinks { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }
    }

    public class Item : IEquatable<Item>, IComparable<Item>
    {
        [JsonPropertyName("has_synonyms")]
        public bool HasSynonyms { get; set; }

        [JsonPropertyName("is_moderator_only")]
        public bool IsModeratorOnly { get; set; }

        [JsonPropertyName("is_required")]
        public bool IsRequired { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("collectives")]
        public List<Collective> Collectives { get; set; }

        public double Percentage { get; set; }

        public int CompareTo(Item other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                return this.Count.CompareTo(other.Count);
            }
        }
        

        public bool Equals(Item other)
        {
            if (other == null) return false;
            return (this.Count.Equals(other.Count));
        }
    }

    public class Root
    {
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("backoff")]
        public int Backoff { get; set; }

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }

        [JsonPropertyName("quota_max")]
        public int QuotaMax { get; set; }

        [JsonPropertyName("quota_remaining")]
        public int QuotaRemaining { get; set; }
    }
} 
