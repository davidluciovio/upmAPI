using Entity.Models.DataUPM;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.ModelServices.DataProduction
{
    public class DataActiveEmployeesService
    {
        private readonly DataContext _contextData;

        public DataActiveEmployeesService(DataContext contextData)
        {
            _contextData = contextData;
        }

            public async Task<List<DataActiveEmployees>> GetActiveEmployees()
            {
                var activeEmployees = await _contextData.ActiveEmployees
                    .ToListAsync();
    
                return activeEmployees;
            }
    }
}
