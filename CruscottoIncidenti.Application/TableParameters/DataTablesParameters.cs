using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CruscottoIncidenti.Application.TableParameters
{
    [Serializable]
    [DataContract]
    public class DataTablesParameters
    {
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }

        [DataMember(Name = "draw")]
        public int Draw { get; set; }

        [DataMember(Name = "start")]
        public int Start { get; set; }

        [DataMember(Name = "length")]
        public int Length { get; set; }

        [DataMember(Name = "columns")]
        public List<DataTablesColumn> Columns { get; set; }

        [DataMember(Name = "search")]
        public DataTablesSearch Search { get; set; }

        [DataMember(Name = "order")]
        public List<DataTablesOrder> Order { get; set; }
    }
}
