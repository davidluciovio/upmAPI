using upmDomain.DomainDowntime;
using upmDomain.DomainPartNumber;
using upmDomain.Lider;
using upmDomain.LineDomain;

namespace upmDomain.ProductionReport
{
    public class ProductionReportDto
    {
        public PartNumberDto PartNumber { get; set; } = new PartNumberDto();
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }

        public List<TimeProduction> TimeProductions { get; set; } = new List<TimeProduction>();
    }


    public class TimeProduction
    {
        public DateTime Time { get; set; }
        public int Production { get; set; }
        public float Plan { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public float Efectivity
        {
            get
            {
                return Plan == 0 ? 0 : (float)Production / Plan;
            }
        }

        public List<TimeDowntime> Downtimes { get; set; } = new List<TimeDowntime>();
    }

    public class TimeDowntime
    {
        public TimeSpan Minutes { get; set; }
        public DowntimeDto? Downtime { get; set; } = new DowntimeDto();
    }

   
}
