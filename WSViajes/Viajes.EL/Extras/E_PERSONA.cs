using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_PERSONA
    {
    
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        /*public string APE_PATERNO { get; set; }
        public string APE_MATERNO { get; set; }*/
        public string Sexo { get; set; }
        public int Edad { get; set; }
        public string Telefono { get; set; }
        public string Fotografia { get; set; }
        public byte Estatus { get; set; }

        /*public string NOMBRE_COMPLETO {
            get {
                return $"{ this.NOMBRE} { this.APE_PATERNO} { this.APE_MATERNO} ";
            }
        }*/
    }
}
