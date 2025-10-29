using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class DataProductionLocation
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string ProductionLocationName { get; set; } = string.Empty;

        public ICollection<DataProductionPartNumber> ProductionPartNumbers { get; set; } = new List<DataProductionPartNumber>();


    }
}
