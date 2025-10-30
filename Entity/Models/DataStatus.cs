using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class DataStatus
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public required DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string StatusDescription { get; set; } = string.Empty;

    }
}
