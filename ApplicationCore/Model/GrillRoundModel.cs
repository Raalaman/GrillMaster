using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class GrillRoundModel
    {
        public GrillRoundModel()
        {
            GrillRoundStrips = new List<GrillRoundStripModel>();
        }
        public int Number { get; set; }
        public List<GrillRoundStripModel> GrillRoundStrips { get; set; }
        /// <summary>
        /// The max Height of the items grilled, it is the sum of the height of the first item in each strip
        /// </summary>
        public int CurrentMaxHeightAllStrips
        {
            get
            {
                return GrillRoundStrips.Sum(z => z.Height);
            }
        }
    }
}
