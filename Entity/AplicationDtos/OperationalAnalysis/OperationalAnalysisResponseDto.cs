using static Entity.AplicationDtos.OperationalAnalysis.OperationalAnalysisResponseDto.AreaOperativityDayTrend;

namespace Entity.AplicationDtos.OperationalAnalysis
{
    public class OperationalAnalysisResponseDto
    {
        public List<KPICardsData> Cards { get; set; } = new List<KPICardsData>();
        public List<ManagmentOperativityData> Managments { get; set; } = new List<ManagmentOperativityData>();
        public List<PartNumberOperativityData> PartNumbers { get; set; } = new List<PartNumberOperativityData>();
        public List<AreaOperativityDayTrend> AreaOperativityDayTrends { get; set; } = new List<AreaOperativityDayTrend>();
        //public List<SupervisorOperativityDayHeatMap> SupervisorOperativityDayHeatMaps { get; set; } = new List<SupervisorOperativityDayHeatMap>();
        public List<AnnualAreaTrend> AnnualAreaTrends { get; set; } = new List<AnnualAreaTrend>();
        public class KPICardsData
        {
            public string Area { get; set; } = string.Empty;
            public double Operativity { get; set; }
        }

        public class ManagmentOperativityData
        {
            public string Managment { get; set; } = string.Empty;
            public string Area { get; set; } = string.Empty;
            public double Operativity { get; set; }

            public List<JefeOperativityData> Jefes { get; set; } = new List<JefeOperativityData>();

            public class JefeOperativityData
            {
                public string Jefe { get; set; } = string.Empty;
                public double Operativity { get; set; }
                public List<SupervisorOperativityData> Supervisors { get; set; } = new List<SupervisorOperativityData>();
                public class SupervisorOperativityData
                {
                    public string Supervisor { get; set; } = string.Empty;
                    public double Operativity { get; set; }

                    public List<LeaderOperativityData> Leaders { get; set; } = new List<LeaderOperativityData>();

                    public class LeaderOperativityData
                    {
                        public string Leader { get; set; } = string.Empty;
                        public double Operativity { get; set; }
                    }
                }
            }
        }

        public class PartNumberOperativityData
        {
            public string PartNumber { get; set; } = string.Empty;
            public string Area { get; set; } = string.Empty;
            public string Supervisor { get; set; } = string.Empty;
            public string Leader { get; set; } = string.Empty;
            public double Operativity { get; set; }
            public List<DayOperativity> DayOperativities { get; set; } = new List<DayOperativity>();

        }

        public class AreaOperativityDayTrend
        {
            public string Area { get; set; } = string.Empty;
            public List<DayOperativity> DayOperativities { get; set; } = new List<DayOperativity>();
        }
        //public class SupervisorOperativityDayHeatMap
        //{
        //    public string Supervisor { get; set; } = string.Empty;
        //    public List<DayOperativity> DayOperativities { get; set; } = new List<DayOperativity>();
        //    public List<LeaderOperativityData> Leaders { get; set; } = new List<LeaderOperativityData>();

        //    public class LeaderOperativityData
        //    {
        //        public string Leader { get; set; } = string.Empty;
        //        public List<DayOperativity> DayOperativities { get; set; } = new List<DayOperativity>();

        //    }
        //}

        public class AnnualAreaTrend
        {
            public string Area { get; set; } = string.Empty;
            public List<MonthOperativity> Months { get; set; } = new List<MonthOperativity>();
        }

        public class MonthOperativity
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public string MonthName { get; set; } = string.Empty; // Opcional, útil para gráficas
            public double Operativity { get; set; }
        }

        public class DayOperativity
        {
            public DateTime Day { get; set; }
            public double Operativity { get; set; }
        }
    }

}
