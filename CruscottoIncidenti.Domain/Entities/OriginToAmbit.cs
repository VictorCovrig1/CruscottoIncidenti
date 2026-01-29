namespace CruscottoIncidenti.Domain.Entities
{
    public class OriginToAmbit
    {
        public int AmbitId { get; set; }
        public Ambit Ambit { get; set; }

        public int OriginId { get; set; }

        public Origin Origin { get; set; }
    }
}
