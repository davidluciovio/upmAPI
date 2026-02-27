using System;

namespace Entity.Dtos.AplicationDtos.DowntimeCapture
{
    public class LineOperatorsRegisterResponseDto
    {
        public Guid Id { get; set; }

        public string OperatorCode { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;

        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
    }
}
