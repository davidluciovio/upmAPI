using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class ProductionControlComponentAlert
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string Component { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string SNP { get; set; } = string.Empty;

        public string? CompleteBy { get; set; }
        public string? ProcessedBy { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? CancelDate { get; set; }

        public virtual ProductionControlStatus Status { get; set; } = null!;
        public Guid StatusId { get; set; }
        public virtual DataProductionPartNumber ProductionPartNumber { get; set; } = null!;
        public Guid ProductionPartNumberId { get; set; }
    }
}
