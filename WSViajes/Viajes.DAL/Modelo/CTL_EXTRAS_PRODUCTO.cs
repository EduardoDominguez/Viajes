//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Viajes.DAL.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class CTL_EXTRAS_PRODUCTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CTL_EXTRAS_PRODUCTO()
        {
            this.R_DETALLE_PEDIDO_EXTRAS = new HashSet<R_DETALLE_PEDIDO_EXTRAS>();
        }
    
        public System.Guid id_extra { get; set; }
        public int id_producto { get; set; }
        public string nombre { get; set; }
        public decimal precio { get; set; }
        public byte estatus { get; set; }
        public int id_persona_alta { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public Nullable<int> id_persona_mod { get; set; }
        public Nullable<System.DateTime> fecha_mod { get; set; }
    
        public virtual CTL_PERSONA CTL_PERSONA { get; set; }
        public virtual CTL_PERSONA CTL_PERSONA1 { get; set; }
        public virtual CTL_PERSONA CTL_PERSONA2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_DETALLE_PEDIDO_EXTRAS> R_DETALLE_PEDIDO_EXTRAS { get; set; }
    }
}