using System;
namespace WSViajes.Models.Request
{
    public class InsertaPedidoRequest
    {
        public int IdPersonaPide { get; set; }
        public int IdDireccionEntrega { get; set; }
        public int IdPersonaEntrega { get; set; }
        public int IdEstatus { get; set; }
        public int IdMetodoPago { get; set; }
        public int Calificacion { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaPedido { get; set; }
        public TimeSpan HoraPedido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public TimeSpan HoraEntrega { get; set; }
        public string Folio { get; set; }
        //public List<E_DETALLE_PEDIDO> Detalle { get; set; }
    }
}
