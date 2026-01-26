using System.Collections.Generic;

namespace CruscottoIncidenti.Application.TableParameters
{
    public class DataTablesParameters
    {
        public int TotalCount { get; set; }

        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public List<DataTablesColumn> Columns { get; set; }

        public DataTablesSearch Search { get; set; }

        public List<DataTablesOrder> Order { get; set; }
    }
}
