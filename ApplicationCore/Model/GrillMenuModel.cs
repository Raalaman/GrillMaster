using System.Collections.Generic;

namespace ApplicationCore
{
    public class GrillMenuModel
    {
        public GrillMenuModel()
        {
            Items = new List<GrillMenuItemModelWQuantity>();
            Rounds = new List<GrillRoundModel>();
        }
        public string Menu { get; set; }
        public List<GrillMenuItemModelWQuantity> Items { get; set; }
        public List<GrillRoundModel> Rounds { get; set; }
    }
}
