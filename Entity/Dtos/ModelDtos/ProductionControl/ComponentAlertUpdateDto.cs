using System;

namespace Entity.ModelDtos
{
    public class ComponentAlertUpdateDto
    {
        public bool? Active { get; set; }
        public Guid? StatusId { get; set; }
    }
}
