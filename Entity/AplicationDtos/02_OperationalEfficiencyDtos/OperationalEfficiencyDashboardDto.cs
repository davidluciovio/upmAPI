namespace Entity.AplicationDtos._02_OperationalEfficiencyDtos
{
    public class OperationalEfficiencyDashboardDto
    {
        public StatsPayload Stats { get; set; }
        public List<string> Dates { get; set; }
        public List<SupervisorHierarchyNode> Hierarchy { get; set; }
        public List<PartKpi> Parts { get; set; }
    }

    public class StatsPayload
    {
        public DashboardKpis Kpis { get; set; }
        public Dictionary<string, DateMapAccumulator> DateMap { get; set; }
        public Dictionary<string, Dictionary<string, DateMapAccumulator>> AreaDateMap { get; set; }
        public Dictionary<string, Dictionary<string, DateMapAccumulator>> LeaderDateMap { get; set; }
        public Dictionary<string, Dictionary<string, DateMapAccumulator>> SupervisorDateMap { get; set; }
        public Dictionary<string, DateMapAccumulator> ShiftMap { get; set; }
        public Dictionary<string, float> LeaderDownMap { get; set; }
    }

    public class DashboardKpis
    {
        public float RealWorkingTime { get; set; }
        public float TotalTime { get; set; }
    }

    public class DateMapAccumulator
    {
        public float Work { get; set; }
        public float Total { get; set; }
        public int TotalRecs { get; set; }
        public int SuccessRecs { get; set; }
        public float OperSum { get; set; }
        public int Count { get; set; }
    }

    public class SupervisorHierarchyNode
    {
        public string Name { get; set; }
        public float Operativity { get; set; }
        public List<LeaderHierarchyNode> Leaders { get; set; }
    }

    public class LeaderHierarchyNode
    {
        public string Name { get; set; }
        public float Operativity { get; set; }
    }

    public class PartKpi
    {
        public string Name { get; set; }
        public float Operativity { get; set; }
    }
}
