using System;

namespace Entity.Dtos.AplicationDtos.DowntimeCapture
{
    public class LineOperatorsRegisterRequestDto
    {
        public string OperatorCode { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;

        public Guid LineId { get; set; }

        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
    }
}
