using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class GrillSettings
    {
        public string BaseURL { get; set; }
        public bool IsDev { get; set; }

        public const string SECTION = "GrillAssessment";
    }
}
