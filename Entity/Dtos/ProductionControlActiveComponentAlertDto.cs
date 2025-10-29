using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class ProductionControlActiveComponentAlertDto
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string SNP { get; set; } = string.Empty;
        public string Line { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public string? CompleteBy { get; set; }
        public string? ProcessedBy { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? CancelDate { get; set; }
    }
}
