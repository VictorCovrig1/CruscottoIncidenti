using System;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Domain.Interfaces
{
    public interface IAuditableEntity : IBaseEntity
    {
        int? CreatedBy { get; set; }

        DateTime? Created { get; set; }

        int? LastModifiedBy { get; set; }

        DateTime? LastModified { get; set; }
    }
}
