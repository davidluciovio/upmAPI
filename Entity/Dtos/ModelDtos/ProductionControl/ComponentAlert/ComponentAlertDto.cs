using System;

namespace Entity.Dtos.ModelDtos.ProductionControl.ComponentAlert
{
    public class ComponentAlertDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string? CompleteBy { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string? ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? CancelBy { get; set; }
        public DateTime? CancelDate { get; set; }
        public string? CriticalBy { get; set; }
        public DateTime? CriticalDate { get; set; }

        public Guid StatusId { get; set; }
        public Guid PartNumberLogisticsId { get; set; }
    }
}
