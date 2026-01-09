namespace Entity.AplicationDtos.OperationalAnalysis
{
    public class OperationalAnalysisResponseDto
    {
        public List<OperationalAnalysisItemDto> AnalysisResults { get; set; } = new();
    }

    public class OperationalAnalysisItemDto
    {
        public required string Leader { get; set; }
        public required string PartNumberName { get; set; }
        public required string Area { get; set; }
        public required string Supervisor { get; set; }
        public required string Shift { get; set; }
        public float AverageOperativityPercent { get; set; }
        public float TotalDowntime { get; set; }
    }
}
