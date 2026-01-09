namespace Entity.AplicationDtos.OperationalAnalysis
{
    public class OperationalAnalysisFiltersDataDto
    {
        public List<string> Leaders { get; set; } = new();
        public List<string> PartNumbers { get; set; } = new();
        public List<string> Areas { get; set; } = new();
        public List<string> Supervisors { get; set; } = new();
        public List<string> Shifts { get; set; } = new();
    }
}
