using System.Collections.Generic;

namespace CruscottoIncidenti.Application.TableParameters
{
    public class DataTablesSearch
    {
        public DataTablesSearch()
        {
            Values = new List<string>();
        }

        public string Value { get; set; }

        public ICollection<string> Values { get; set; }

        public string Regex { get; set; }
    }
}
