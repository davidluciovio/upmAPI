using LogicData.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain
{
    public class DataPartNumberService
    { 
        private readonly DataContext _dataContext;
        public DataPartNumberService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

    }
}
