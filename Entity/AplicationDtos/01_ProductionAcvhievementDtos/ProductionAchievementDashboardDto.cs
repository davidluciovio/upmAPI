using System.Collections.Generic;

namespace Entity.AplicationDtos._01_ProductionAcvhievementDtos
{
    public class ProductionAchievementDashboardDto
    {
        public AchievementKpis? Kpis { get; set; }
        public List<SupervisorAchievementNode>? Hierarchy { get; set; }
        public List<PartAchievement>? Parts { get; set; }
        public ChartData? ChartData { get; set; }
    }

    public class AchievementKpis
    {
        public float? TotalObj { get; set; }
        public float? TotalReal { get; set; }
        public float? Ach { get; set; }
        public float? Diff { get; set; }
    }

    public class SupervisorAchievementNode
    {
        public string? Name { get; set; }
        public float? Obj { get; set; }
        public float? Real { get; set; }
        public float? Ach { get; set; }
        public List<LeaderAchievementNode>? Leaders { get; set; }
    }

    public class LeaderAchievementNode
    {
        public string? Name { get; set; }
        public float? Obj { get; set; }
        public float? Real { get; set; }
        public float? Ach { get; set; }
    }

    public class PartAchievement
    {
        public string? Name { get; set; }
        public float? Obj { get; set; }
        public float? Real { get; set; }
        public float? Ach { get; set; }
    }

    public class ChartData
    {
        public Dictionary<string, DateStats>? DateStats { get; set; }
        public Dictionary<string, Dictionary<string, AreaDateSubtotals>>? AreaDateMap { get; set; }
        public List<string>? ActiveAreas { get; set; }
        public List<string>? Dates { get; set; }
    }

    public class DateStats
    {
        public float? Obj { get; set; }
        public float? Real { get; set; }
        public float? TotalCount { get; set; }
        public float? SuccessCount { get; set; }
    }

    public class AreaDateSubtotals
    {
        public float? Obj { get; set; }
        public float? Real { get; set; }
    }
}
