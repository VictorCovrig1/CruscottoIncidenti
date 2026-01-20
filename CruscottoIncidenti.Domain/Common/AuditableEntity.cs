using System;
using CruscottoIncidenti.Domain.Interfaces;

namespace CruscottoIncidenti.Domain.Common
{
    public class AuditableEntity : IAuditableEntity
    {
        public int Id { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? Created { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
