using System;
using System.Collections.Generic;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Pedido
{
    public class EstatusOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para consultar estatus del pedido
        /// <param name="pIdEstatus">Folio del pedido</param>
        /// <returns> Objeto tipo List<E_ESTATUS_PEDIDO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_ESTATUS_PEDIDO>> Consultar(int? pIdEstatus = null)
        {
            try
            {
                var listaEstatus = new List<E_ESTATUS_PEDIDO>();
                using (context = new ViajesEntities())
                {
                    var estatusQuery = await (from s in context.CTL_ESTATUS_PEDIDO
                                    where
                                    (pIdEstatus == null || (pIdEstatus != null && s.id_estatus == pIdEstatus))
                                    select s).ToListAsync<CTL_ESTATUS_PEDIDO>();

                    foreach (var estatus in estatusQuery)
                    {
                        listaEstatus.Add(new E_ESTATUS_PEDIDO
                        {
                            IdEstatus = estatus.id_estatus,
                            Nombre = estatus.nombre,
                            Descripcion = estatus.descripcion
                        });
                    }

                    return listaEstatus;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


