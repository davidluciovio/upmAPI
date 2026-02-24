using Entity.Dtos.ProductionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.AplicationDtos.ComponentAlert
{
    public class ComponentAlertResponseDto
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public string? CompleteBy { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string? ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? CancelBy { get; set; }
        public DateTime? CancelDate { get; set; }
        public string? CriticalBy { get; set; }
        public DateTime? CriticalDate { get; set; }

        public string Status { get; set; } = string.Empty;
        public virtual PartNumberLogisticsResponseDto? PartNumberLogistics { get; set; }
        public string User { get; set; } = string.Empty;
    }
}
