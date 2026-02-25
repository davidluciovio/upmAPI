using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.AplicationDtos.ComponentAlert
{
    public class ComponentAlertFiltersDto
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public Guid? AreaId { get; set; }
        public string? UserId { get; set; }
        public Guid? PartNumberLogisticId { get; set; }
    }
}
