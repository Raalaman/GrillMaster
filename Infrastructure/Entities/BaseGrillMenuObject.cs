using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure
{
    public abstract class BaseGrillMenuObject
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; }
        [JsonPropertyName("Id")]
        public Guid Guid { get; set; }
    }
}
