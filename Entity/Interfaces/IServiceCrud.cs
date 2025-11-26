using Entity.Dtos.DataProduction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Interfaces
{
    public interface IServiceCrud<T, V, U> where T : class where V : class where U : class
    {
        Task<T> Create(V dtocreate);

        Task<List<T>> GetAlls();

        Task<T?> GetById(Guid id);

        Task<T> Update(Guid id, U areaDto);
    }
}
