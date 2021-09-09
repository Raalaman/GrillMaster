using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class GrillRoundStripModel
    {
        public GrillRoundStripModel()
        {
            GrillMenuItems = new List<GrillMenuItemModel>();
        }
        public List<GrillMenuItemModel> GrillMenuItems { get; set; }
        /// <summary>
        /// Height of the current strip, it's always the height of the first grilled item, max size is <see cref="GrillModel.HEIGHT"/>
        /// </summary>
        public int Height { get { return GrillMenuItems.FirstOrDefault()?.Height ?? 0; } }
        /// <summary>
        /// Current Width of the strip, max size is <see cref="GrillModel.WIDTH"/>
        /// </summary>
        public int Width { get { return GrillMenuItems.Sum(x => x.Width); } }
    }
}
