using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.MetodoPago;

namespace Viajes.BL.MetodoPago
{
    public class MetodoPagoNegocio : ICRUD<E_METODO_PAGO>
    {
        /// <summary>
        /// Método para consultar agregar metodos de pago
        /// <param name="Entidad">Datos a agregar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Agregar(E_METODO_PAGO Entidad)
        {
            throw new NotImplementedException();
            /*try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return pDatos.Agregar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }*/
        }

        /// <summary>
        /// Método para consultar por id
        /// <param name="pId">Id del registro a consultar</param>
        /// <returns> Objeto tipo E_METODO_PAGO con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_METODO_PAGO> ConsultarPorId(int pId)
        {
            try
            {
                MetodoPagoOperaciones pDatos = new MetodoPagoOperaciones();
                var pResultado = await pDatos.Consultar(pId);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar todo
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_METODO_PAGO con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_METODO_PAGO>> ConsultarTodo(byte? SoloActivos = null, int? IdGenerico = null)
        {
            try
            {
                MetodoPagoOperaciones pDatos = new MetodoPagoOperaciones();
                return await pDatos.Consultar(null, SoloActivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public E_MENSAJE Editar(E_METODO_PAGO Entidad)
        {
            throw new NotImplementedException();
        }

    }

}
