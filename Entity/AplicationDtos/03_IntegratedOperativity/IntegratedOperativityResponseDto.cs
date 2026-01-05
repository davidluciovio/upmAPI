namespace Entity.AplicationDtos._03_IntegratedOperativity
{
    public class IntegratedOperativityResponseDto
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public List<ShiftData> ShiftDataList { get; set; } = new List<ShiftData>();

        public class ShiftData
        {
            public string ShiftName { get; set; } = string.Empty;
            public SummaryData Summary { get; set; } = new SummaryData();
            public List<AreaData> AreaDataList { get; set; } = new List<AreaData>();
        }

        public class AreaData
        {
            public string AreaName { get; set; } = string.Empty;
            public SummaryData Summary { get; set; } = new SummaryData();
            public List<OperativityData> OperativityDataList { get; set; } = new List<OperativityData>();
            public List<SupervisorData> SupervisorDataList { get; set; } = new List<SupervisorData>();
        }

        public class SupervisorData {
            public string SupervisorName { get; set; } = string.Empty;
            public SummaryData Summary { get; set; } = new SummaryData();

            public List<OperativityData> OperativityDataList { get; set; } = new List<OperativityData>();
            public List<LeaderData> LeaderDataList { get; set; } = new List<LeaderData>();
        }

        public class LeaderData
        {
            public string LeaderName { get; set; } = string.Empty;
            public SummaryData Summary { get; set; } = new SummaryData();

            public List<OperativityData> OperativityDataList { get; set; } = new List<OperativityData>();
            public List<PartNumberData> PartNumberDataList { get; set; } = new List<PartNumberData>();

        }

        public class PartNumberData
        {

            public string PartNumberName { get; set; } = string.Empty;
            public string PartNumberDescription { get; set; } = string.Empty;
            public SummaryData Summary { get; set; } = new SummaryData();

            public List<OperativityData> OperativityDataList { get; set; } = new List<OperativityData>();
        }

        public class SummaryData
        {
            public TimeSpan TotalWorkingTime { get; set; }
            public float AverageAchievement { get; set; }
            public int TotalRealProduction { get; set; }
            public int TotalObjetiveProduction { get; set; }
        }

        public class OperativityData
        {
            public DateOnly Date { get; set; }
            public TimeSpan WorkingTime { get; set; }
            public float Achievement { get; set; }
            public int RealProduction { get; set; }
            public int ObjetiveProduction { get; set; }
            public float HPTime { get; set; }
            public float RealTime { get; set; }
            public float OperativityPercent { get; set; }
            public float TotalTime { get; set; }
            public float ProgramabeDowntimeTime { get; set; }
            public float RealWorkingTime { get; set; }
            public float TotalDowntime { get; set; }
            public float NoProgramabeDowntimeTime { get; set; }
            public float NoReportedTime { get; set; }
        }
    }


}
