using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.DataProduction
{
    public class DataProductionLine
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string LineDescription { get; set; } = string.Empty;

    }
}
