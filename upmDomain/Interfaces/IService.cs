using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmDomain.UserTools;

namespace upmDomain.Interfaces
{
    internal interface IService<T> where T : class
    {
        public Task<List<T>> GetAllAsync();
    }
}
