using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklad.Web.Models
{
    public class OrderSave
    {
        public int MaterialId { get; set; }
        public int Kgs { get; set; }
        public int Bags { get; set; }
        public int WorkerId { get; set; }
        public DateTime ActionedAt { get; set; }
        public int StageFromId { get; set; }
        public int StageToId { get; set; }
    }

    public class OrderGet : OrderSave
    {
        public Guid Id { get; set; }
        public string MaterialName { get; set; }
        public string WorkerName { get; set; }
        public string StageFromName { get; set; }
        public string StageToName { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
