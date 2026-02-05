using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entity.AplicationDtos._01_ProductionAcvhievementDtos
{
    public class ProductionAchievementResponseDto
    {
        public class ProductionReportDto
        {
            [JsonPropertyName("partInfo")]
            public PartInfoDto? PartInfo { get; set; }

            [JsonPropertyName("dailyRecords")]
            public List<DailyRecordDto> DailyRecords { get; set; } = new();
        }

        public class PartInfoDto
        {
            [JsonPropertyName("number")]
            public string? Number { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("area")]
            public string? Area { get; set; }

            [JsonPropertyName("supervisor")]
            public string? Supervisor { get; set; }

            [JsonPropertyName("leader")]
            public string? Leader { get; set; }
        }

        public class DailyRecordDto
        {
            [JsonPropertyName("date")]
            public DateTime Date { get; set; }

            [JsonPropertyName("time")]
            public double Time { get; set; }

            [JsonPropertyName("obj")]
            public double Obj { get; set; }

            [JsonPropertyName("real")]
            public double Real { get; set; }
        }
    }
}
