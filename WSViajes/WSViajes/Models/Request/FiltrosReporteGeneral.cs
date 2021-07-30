using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WSViajes.Models;

namespace WSViajes.Models.Request
{
    public class FiltrosReporteGeneral : FiltrosReporte
    {
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
    }
}