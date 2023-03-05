﻿using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace StuartsBookstores.Models
{
    public class Bookstore
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public int Zip { get; set; }
        public string Country { get; set; } = default!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Website { get; set; }
        [JsonPropertyName("img")]
        public string Image { get; set; } = default!;
        [JsonPropertyName("date_visited")]
        public string? DateVisited { get; set; }


        public override string ToString() => JsonSerializer.Serialize<Bookstore>(this);
    }
}
