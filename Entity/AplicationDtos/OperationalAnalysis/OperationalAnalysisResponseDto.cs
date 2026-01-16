namespace Entity.AplicationDtos.OperationalAnalysis
{
    public class OperationalAnalysisResponseDto
    {
        public List<KPICardsData> Cards { get; set; } = new List<KPICardsData>();
        public List<SupervisorOperativityData> Supervisors { get; set; } = new List<SupervisorOperativityData>();
        public List<PartNumberOperativityData> PartNumbers { get; set; } = new List<PartNumberOperativityData>();
        public List<AreaOperativityDayTrend> AreaOperativityDayTrends { get; set; } = new List<AreaOperativityDayTrend>();

        public class KPICardsData
        {
            public string Area { get; set; } = string.Empty;
            public double Operativity { get; set; }
        }

        public class SupervisorOperativityData
        {
            public string Supervisor { get; set; } = string.Empty;
            public string Area { get; set; } = string.Empty;
            public double Operativity { get; set; }

            public List<LeaderOperativityData> Leaders { get; set; } = new List<LeaderOperativityData>();

            public class LeaderOperativityData
            {
                public string Leader { get; set; } = string.Empty;
                public double Operativity { get; set; }
            }
        }

        public class PartNumberOperativityData
        {
            public string PartNumber { get; set; } = string.Empty;
            public string Area { get; set; } = string.Empty;
            public string Supervisor { get; set; } = string.Empty;
            public string Leader { get; set; } = string.Empty;
            public double Operativity { get; set; }
        }

        public class AreaOperativityDayTrend
        {
            public string Area { get; set; } = string.Empty;
            public List<DayOperativity> DayOperativities { get; set; } = new List<DayOperativity>();

            public class DayOperativity
            {
                public DateTime Day { get; set; }
                public double Operativity { get; set; }
            }
        }
    }

}
