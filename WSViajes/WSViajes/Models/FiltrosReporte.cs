using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models
{
    public class FiltrosReporte
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
        public string palabraClave { get; set; }
            
    }
}