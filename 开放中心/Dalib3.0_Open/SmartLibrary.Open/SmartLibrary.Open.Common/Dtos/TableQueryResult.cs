using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Common.Dtos
{
    public class TableQueryResult<T> where T : class
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}
