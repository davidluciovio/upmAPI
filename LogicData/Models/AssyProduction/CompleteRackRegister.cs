using Entity.Models.AssyProduction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicData.Models.AssyProduction
{
    public class CompleteRackRegister
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public string NoRack { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;

        public Guid ProductionStationId { get; set; }
        public virtual ProductionStation ProductionStation { get; set; } = new ProductionStation();
    }
}
