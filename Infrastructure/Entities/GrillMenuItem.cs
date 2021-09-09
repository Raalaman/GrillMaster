using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class GrillMenuItem : BaseGrillMenuObject
    {
        [JsonPropertyName("Name")] 
        public string Name { get; set; }
        [JsonPropertyName("Length")]
        public int Length { get; set; }
        [JsonPropertyName("Width")]
        public int Width { get; set; }
        [JsonPropertyName("Duration")]
        public string Duration { get; set; }
        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }

    }
}
