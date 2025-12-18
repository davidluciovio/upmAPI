using System;

namespace Entity.ModelDtos._02_DataProduction.DataProductionDowntime
{
    public class DataProductionDowntimeResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string DowntimeDescription { get; set; }
        public string InforCode { get; set; }
        public string PLCCode { get; set; }
        public bool IsDirectDowntime { get; set; }
        public bool Programable { get; set; }
    }
}