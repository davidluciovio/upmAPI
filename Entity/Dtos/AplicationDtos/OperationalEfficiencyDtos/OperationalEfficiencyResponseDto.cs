namespace Entity.AplicationDtos._02_OperationalEfficiencyDtos
{
    public class OperationalEfficiencyResponseDto
    {
        public record ProductionResponse(
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<ProductionDetailDto>>>>> Data
        );

        public record ProductionDetailDto(
            DateTime ProductionDate,
            MetricsDto Metrics,
            string Shift,
            bool Active
        );

        public record MetricsDto(
            float Hp,
            float Neck,
            float RealTime,
            float ProductionReal,
            float TotalTime,
            float RealWorkingTime,
            float OperativityPercent,
            float DowntimePercent,
            float TotalDowntime,
            float NoReportedTime
        );
    }
}