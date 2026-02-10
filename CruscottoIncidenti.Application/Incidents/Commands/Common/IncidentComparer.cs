using System;
using System.Collections.Generic;
using CruscottoIncidenti.Application.Incidents.ViewModels;

namespace CruscottoIncidenti.Application.Incidents.Commands.Common
{
    public class IncidentComparer : IEqualityComparer<CreateIncidentViewModel>
    {
        public bool Equals(CreateIncidentViewModel x, CreateIncidentViewModel y)
        {
            if (ReferenceEquals(x, y)) 
                return true;

            if (x is null || y is null)
                return false;

            return x.RequestNr == y.RequestNr;
        }

        public int GetHashCode(CreateIncidentViewModel obj)
        {
            if (obj is null)
                return 0;

            return HashCode.Combine(obj.RequestNr);
        }
    }
}
