using System.ComponentModel.DataAnnotations;

namespace Entity.ModelDtos._02_DataProduction.DataProductionDowntime
{
    public class DataProductionDowntimeRequestDto
    {
        [Required]
        public string DowntimeDescription { get; set; } = string.Empty;

        [Required]
        public string InforCode { get; set; } = string.Empty;

        [Required]
        public string PLCCode { get; set; } = string.Empty;

        public bool IsDirectDowntime { get; set; }

        public bool Programable { get; set; }

        [Required]
        public string CreateBy { get; set; } = string.Empty;
        public string? UpdateBy { get; set; }
    }
}