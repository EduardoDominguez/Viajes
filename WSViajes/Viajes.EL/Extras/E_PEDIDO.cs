﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_PEDIDO
    {
        public Guid IdPedido { get; set; }
        //public int IdPersonaPide { get; set; }
        public E_PERSONA PersonaPide { get; set; }
        //public int IdDireccionEntrega { get; set; }
        public E_DIRECCION DireccionEntrega { get; set; }

        //public int IdPersonaEntrega { get; set; }
        public E_PERSONA PersonaEntrega { get; set; }

        public int? IdEncuesta { get; set; }
        public E_ESTATUS_PEDIDO Estatus { get; set; }
        public int IdMetodoPago { get; set; }
        public int Calificacion { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaPedido { get; set; }
        public TimeSpan HoraPedido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public TimeSpan HoraEntrega { get; set; }
        public string Folio { get; set; }
        public string ReferenciaPago { get; set; }
        public decimal CostoEnvio { get; set; }
        public List<E_DETALLE_PEDIDO> Detalle { get; set; }
        public byte TipoPedido { get; set; }
        public decimal Propina { get; set; }
        public byte IdEstatusFactura { get; set; }
        public decimal Iva { get; set; }
        public int TiempoEspera { get; set; }
        public E_PEDIDO()
        {
            
            this.DireccionEntrega = new E_DIRECCION();
            this.Estatus = new E_ESTATUS_PEDIDO();
            this.PersonaEntrega = new E_PERSONA();
            this.PersonaPide = new E_PERSONA();
            this.Iva = 0;
            this.IdEstatusFactura = 0;
            this.Propina = 0;
        }
    }
}
