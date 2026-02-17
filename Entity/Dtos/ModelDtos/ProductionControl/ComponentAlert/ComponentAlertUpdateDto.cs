using System;

namespace Entity.Dtos.ModelDtos.ProductionControl.ComponentAlert
{
    public class ComponentAlertUpdateDto
    {
        public bool? Active { get; set; }
        public Guid? StatusId { get; set; }
    }
}
