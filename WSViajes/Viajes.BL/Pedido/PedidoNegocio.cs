using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Pedido;

namespace Viajes.BL.Pedido
{
    public class PedidoNegocio
    {
        /// <summary>
        /// Método para consultar agregar pedido
        /// <param name="Entidad">Datos del pedido a agregar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Agregar(E_PEDIDO Entidad)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return pDatos.Agregar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar pedidos por id
        /// <param name="pIdPedido">Id del pedido a consultar</param>
        /// <returns> Objeto tipo E_PEDIDO con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_PEDIDO> ConsultarPorId(Guid pIdPedido)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                var pResultado = await pDatos.Consultar(pIdPedido);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar pedidos
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_PEDIDO con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO>> ConsultarTodo(byte? SoloActivos = null, int? idPersona = null)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return await pDatos.Consultar(pIdPersonaPide: idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar pedidos personalizados
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_PEDIDO_PERSONALIZADO con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO_PERSONALIZADO>> ConsultarPersonalizadosTodo(int? idPersona = null)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return await pDatos.ConsultarPersonalizadosTodo(pIdPersonaPide: idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar pedidos personalizados por id
        /// <param name="pIdPedido">Id del pedido a consultar</param>
        /// <returns> Objeto tipo E_PEDIDO_PERSONALIZADO con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_PEDIDO_PERSONALIZADO> ConsultarPersonalizadosPorId(Guid pIdPedido)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                var pResultado = await pDatos.ConsultarPersonalizadosTodo(pIdPedido);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para consultar historial de pedidos por conductor
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_PEDIDO con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO>> ConsultarHistorialConductor(int idPersona)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return await pDatos.ConsultarHistorialConductor(idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar historial de pedidos por local
        /// <param name="idLocal">Identificador del local</param>
        /// <param name="idEstatus">Identificador del estatus</param>
        /// <returns> Objeto tipo E_PEDIDO con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO>> ConsultarHistorialLocal(int idLocal, int? idEstatus = null)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return await pDatos.ConsultarHistorialLocal(idLocal, idEstatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public E_MENSAJE Editar(E_PEDIDO Entidad)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Método para cancelar pedido
        /// <param name="Entidad">Datos del pedido a cancelar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Cancelar(E_PEDIDO Entidad)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return pDatos.Cancelar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar el estatus del pedido
        /// <param name="Entidad">Datos del pedido a cancelar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE ActualizaEstatus(E_PEDIDO Entidad)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return pDatos.ActualizaEstatus(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para consultar pedido actual
        /// <param name="pIdPersonaPide">Id de la persona que pide</param>
        /// <param name="pIdPersonaEntrega">Id de la persona que entrega</param>
        /// <returns> Objeto tipo E_PEDIDO con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_PEDIDO> ConsultarViajeActual(int? pIdPersonaPide = null, int? pIdPersonaEntrega = null)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                var pResultado = await pDatos.ConsultarViajeActual(pIdPersonaPide, pIdPersonaEntrega);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar pedidos por asignar
        /// <returns> Objeto tipo List<E_PEDIDO> con los datos solicitados </returns>  
        /// </summary>       
        public async Task<List<E_PEDIDO>> ConsultaPorAsignar()
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return await  pDatos.ConsultaPorAsignar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para asignar un pedido a un conductor
        /// <param name="´pEntidad">Datos del pedido a cancelar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Asignar(E_PEDIDO pEntidad)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return pDatos.Asignar(pEntidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para consultar las preguntas pendientes por contestar de un pedido
        /// <param name="pIdPedido">Identificador del pedido</param>
        /// <param name="pTipoPregunta">Representa el tipo de preguntas a consultar 1=Cliente, 2=Conductor, 3=Local</param>
        /// <returns> Objeto tipo List<E_PREGUNTA_SERVICIO> con los datos solicitados </returns>  
        /// </summary>        
        public async Task<List<E_PREGUNTA_SERVICIO>> ConsultarPreguntasPendientesByIdPedido(Guid pIdPedido, byte pTipoPregunta)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                var pResultado = await pDatos.ConsultarPreguntasPendientesByIdPedido(pIdPedido, pTipoPregunta);
                return pResultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar todas las preguntas por tipo de pregunta
        /// <param name="pTipoPregunta">Representa el tipo de preguntas a consultar 1=Cliente, 2=Conductor, 3=Local</param>
        /// <returns> Objeto tipo List<E_PREGUNTA_SERVICIO> con los datos solicitados </returns>  
        /// </summary>        
        public async Task<List<E_PREGUNTA_SERVICIO>> ConsultarPreguntaByTipo(byte pTipoPregunta)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                var pResultado = await pDatos.ConsultarPreguntaByTipo(pTipoPregunta);
                return pResultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inserta una respuesta a las preguntas del servicio
        /// <param name="pIdPedido"></param>
        /// <param name="pIdPregunta"></param>
        /// <param name="pRespuesta"></param>
        /// <returns> Objeto tipo E_MENSAJE con los datos DE LA SOLICITUD </returns>  
        /// </summary>        
        public E_MENSAJE AgregarRespuestaPreguntaServicio(Guid pIdPedido, Guid pIdPregunta, byte pRespuesta)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return pDatos.AgregarRespuestaPreguntaServicio(pIdPedido, pIdPregunta, pRespuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Método para agregar pedido personalizado
        /// <param name="Entidad">Datos del pedido personalizado a agregar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE AgregarPersonalizado(E_PEDIDO_PERSONALIZADO Entidad)
        {
            try
            {
                PedidoOperaciones pDatos = new PedidoOperaciones();
                return pDatos.AgregarPersonalizado(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar en que rango entra el costo en base a un pedido
        /// <paramref name="pMetros"/>
        /// <returns> Objeto tipo E_TAFIRA_ENVIO con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_TAFIRA_ENVIO> ConsultaCotoEnvioByDistancia(int pMetros)
        {
            try
            {

                PedidoOperaciones pDatos = new PedidoOperaciones();
                var tarifa = await pDatos.ConsultaCotoEnvioByDistancia(pMetros);
                return tarifa.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

