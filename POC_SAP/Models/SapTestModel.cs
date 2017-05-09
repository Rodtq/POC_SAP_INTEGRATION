using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POC_SAP.Models
{
    public class SapTestModel
    {
        public List<string> ColumnsNames { get; set; }
        public List<Row> Data { get; set; }
    }
    public class Column
    {
        public string PropertyValue { get; set; }
    }
    public class Row
    {
        public List<Column> Columns { get; set; }
    }

}
