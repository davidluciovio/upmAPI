using Entity.Models.DataUPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models._05_Temporal
{
    public class ProductionAchievement
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public DateTime ProductionDate { get; set; }
        public string Area { get; set; } = string.Empty;
        public string Supervisor { get; set; } = string.Empty;
        public string Leader { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string PartNumberName { get; set; } = string.Empty;
        public Guid? PartNumberId { get; set; }

        public float WorkingTime { get; set; }
        public float ProductionObjetive { get; set; }
        public float ProductionReal { get; set; }
    }
}
