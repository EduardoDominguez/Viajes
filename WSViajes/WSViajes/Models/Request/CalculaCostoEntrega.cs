
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class CalculaCostoEntrega
    {
        public int IdDireccionEntrega { get; set; }
        public List<int> DireccionesRecoge { get; set; }
    }
}