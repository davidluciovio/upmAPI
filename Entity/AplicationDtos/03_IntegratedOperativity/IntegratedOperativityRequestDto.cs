namespace Entity.AplicationDtos._03_IntegratedOperativity
{
    public class IntegratedOperativityRequestDto
    {
        public GlobalRangeFilter GlobalRange { get; set; } = new GlobalRangeFilter();
        public HierarchyFilter Hierarchy { get; set; } = new HierarchyFilter();
        public PartDataFilter PartData { get; set; } = new PartDataFilter();
        public OperativityFilter Operativity { get; set; } = new OperativityFilter();

        public class GlobalRangeFilter
        {
            // Usamos DateTime? (nullable) por si el usuario no selecciona fechas
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string Shift { get; set; } = "all";
        }

        public class HierarchyFilter
        {
            public string? AreaName { get; set; }
            public string? SupervisorName { get; set; }
            public string? LeaderName { get; set; }
        }

        public class PartDataFilter
        {
            public string? PartNumber { get; set; }
            public string? Description { get; set; }
        }

        public class OperativityFilter
        {
            /// <summary>
            /// Porcentaje mínimo de cumplimiento (0-100)
            /// </summary>
            public float? MinAchievement { get; set; } = 0;

            public float? MaxAchievement { get; set; } = 100;

            /// <summary>
            /// Valores esperados: "all", "real", "objective"
            /// </summary>
            public string ProductionType { get; set; } = "all";
        }
    }
}
