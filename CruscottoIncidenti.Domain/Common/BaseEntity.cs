using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Domain.Common
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}
