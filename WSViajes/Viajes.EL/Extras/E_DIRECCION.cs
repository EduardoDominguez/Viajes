using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_DIRECCION
    {
        public int IdDireccion { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string Descripcion { get; set; }
        public string NoInt { get; set; }
        public string NoExt { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int IdPersona { get; set; }
        public int IdPersonaAlta { get; set; }
        public int IdPersonaMod { get; set; }
        public byte Estatus { get; set; }
        public byte Predeterminada { get; set; }
    }
}
