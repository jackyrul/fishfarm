using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklad.Web.Models
{
    public class SumsPerStage
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public int InKgs { get; set; }
        public int InBags { get; set; }
        public int OutKgs { get; set; }
        public int OutBags { get; set; }
    }
}
