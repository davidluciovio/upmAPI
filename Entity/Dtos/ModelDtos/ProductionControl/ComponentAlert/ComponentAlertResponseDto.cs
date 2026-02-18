using Entity.Dtos.Auth;
using Entity.Dtos.ProductionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.ModelDtos.ProductionControl.ComponentAlert
{
    public class ComponentAlertResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
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

        public string StatusDescrition { get; set; } = string.Empty;
        public PartNumberLogisticsResponseDto PartNumberLogisticsResponseDto { get; set; } = new PartNumberLogisticsResponseDto();
        public Guid PartNumberLogisticsId { get; set; }
        public Guid UserId { get; set; }
        public AuthUserDto UserData { get; set; }
    }
}
