using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm.Web.Models
{
    public class PipelineRuleSave
    {
        public int StageFromId { get; set; }
        public int StageToId { get; set; }
    }

    public class PipelineRuleGet : PipelineRuleSave
    {
        public int Id { get; set; }
        public string StageFromName { get; set; }
        public string StageToName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
