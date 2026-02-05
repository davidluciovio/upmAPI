using System;

namespace Entity.AplicationDtos._02_OperationalEfficiencyDtos
{
    public class OperationalEfficiencyRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Leader { get; set; }
        public string? PartNumberName { get; set; }
        public string? Area { get; set; }
        public string? Supervisor { get; set; }
        public string? Shift { get; set; }
    }
}