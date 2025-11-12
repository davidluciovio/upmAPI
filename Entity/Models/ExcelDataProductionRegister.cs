using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class ExcelDataProductionRegister
    {
        public string Supervisor { get; set; } = string.Empty;
        public string Leader { get; set; } = string.Empty;
        public string LineName { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string DataValue { get; set; } = string.Empty;
    }
}
