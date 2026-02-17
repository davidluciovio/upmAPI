using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.ModelDtos.ProductionControl.ForkliftArea
{
    public class ForkliftAreaResponseDto
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public List<AreasData> DataProductionAreaId { get; set; } = new List<AreasData>();

        public class AreasData
        {
            public Guid Id { get; set; }
            public string AreaName { get; set; } = string.Empty;
        }
    }
}
