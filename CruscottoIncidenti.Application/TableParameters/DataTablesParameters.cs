using System.Collections.Generic;
using System.Linq;

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
        /// <summary>
        /// Used for sorting
        /// </summary>
        public void SetColumnName()
        {
            foreach (var item in Order)
            {
                item.Name = Columns[item.Column].Data;
            }
        }
        /// <summary>
        /// Gets the <see cref="DataTableColumn"/> with the specified column name.
        /// </summary>
        /// <value>
        /// The <see cref="DataTableColumn"/>.
        /// </value>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        public DataTablesColumn this[string columnName] => Columns.FirstOrDefault(x => x.Data == columnName);
    }
}
