using upmDomain.Lider;
using upmDomain.LineDomain;

namespace upmDomain.ProductionReport
{
    public class ProductionReportDto
    {
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
        public string? Shift { get; set; }
        public LiderDto? Lider { get; set; }
        public LineDto? Line { get; set; }

    }
}
