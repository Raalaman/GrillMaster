using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class GrillMenu : BaseGrillMenuObject
    {
        public GrillMenu()
        {
            Items = new List<GrillMenuItem>();
        }
        [JsonPropertyName("menu")]
        public string Menu { get; set; }
        [JsonPropertyName("items")]
        public List<GrillMenuItem> Items { get; set; }
    }
}
