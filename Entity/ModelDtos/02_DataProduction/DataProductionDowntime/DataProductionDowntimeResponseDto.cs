using System;

namespace Entity.ModelDtos._02_DataProduction.DataProductionDowntime
{
    public class DataProductionDowntimeResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string DowntimeDescription { get; set; } = string.Empty;
        public string InforCode { get; set; } = string.Empty;
        public string PLCCode { get; set; } = string.Empty;
        public bool IsDirectDowntime { get; set; }
        public bool Programable { get; set; }
    }
}