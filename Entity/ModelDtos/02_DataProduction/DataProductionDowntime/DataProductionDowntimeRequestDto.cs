using System.ComponentModel.DataAnnotations;

namespace Entity.ModelDtos._02_DataProduction.DataProductionDowntime
{
    public class DataProductionDowntimeRequestDto
    {
        [Required]
        public string DowntimeDescription { get; set; }

        [Required]
        public string InforCode { get; set; }

        [Required]
        public string PLCCode { get; set; }

        public bool IsDirectDowntime { get; set; }

        public bool Programable { get; set; }

        [Required]
        public string CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}