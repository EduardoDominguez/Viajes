using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Viajes.EL.Extras
{
    public class E_FILTROS_REPORTE
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
        public string palabraClave { get; set; }

    }
}