using System;
using System.Collections.Generic;

namespace CruscottoIncidenti.Application.Incidents.Commands.Common
{
    public class IncidentStringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (x.ToLower().Contains(y.ToLower()))
                return true;

            if (x is null || y is null)
                return false;

            return x == y;
        }

        public int GetHashCode(string obj)
        {
            if (obj is null)
                return 0;

            return HashCode.Combine(obj);
        }
    }
}
