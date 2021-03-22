using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{    
    public class E_ACCESO_PERSONA
    {
        public int IdPersona { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte TipoUsuario { get; set; }
        public string TokenFirebase { get; set; }
        public string ClavePassword { get; set; }
        public DateTime? FechaClavePassword { get; set; }

    }
}
