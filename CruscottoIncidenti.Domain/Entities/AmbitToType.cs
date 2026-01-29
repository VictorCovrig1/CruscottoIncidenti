namespace CruscottoIncidenti.Domain.Entities
{
    public class AmbitToType
    {
        public int AmbitId { get; set; }
        public Ambit Ambit { get; set; }

        public int TypeId { get; set; }

        public IncidentType Type { get; set; }
    }
}
