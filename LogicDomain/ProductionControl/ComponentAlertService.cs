using Entity.Dtos;
using LogicData.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.ProductionControl
{
    public class ComponentAlertService
    {
        private  readonly ProductionControlContext _contextProductionControl;
        public ComponentAlertService(ProductionControlContext context)
        {
            _contextProductionControl = context;
        }

        //public async Task<ProductionControlActiveComponentAlertDto> GetAll(ProductionControlComponentAlertsRequest request)
        //{

        //}
    }
}
