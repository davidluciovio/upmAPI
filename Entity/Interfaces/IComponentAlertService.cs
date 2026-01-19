using Entity.ModelDtos;
using System;
using System.Threading.Tasks;

namespace Entity.Interfaces
{
    public interface IComponentAlertService : IServiceCrud<ComponentAlertDto, ComponentAlertCreateDto, ComponentAlertUpdateDto>
    {
        Task<bool> Delete(Guid id);
    }
}
