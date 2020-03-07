using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Xml.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using Viajes.DAL.Direccion;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.QR
{
    public class QROperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar un nuevo escaneo del código qr de descarga de app-
        /// <param name="pDatos">Objeto de tipo E_QR_TIENDA con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public async Task<E_MENSAJE> AgregarVisitaQR(E_QR_TIENDA pDatos)
        {
            try
            {
                E_MENSAJE vMensaje;
                using (context = new ViajesEntities())
                {

                    var coordenadas = context.Set<TBL_QR_TIENDA>();
                    coordenadas.Add(new TBL_QR_TIENDA { id_qr = Guid.NewGuid(), dispositivo = pDatos.Dispositivo, ip = pDatos.Ip, latitud = pDatos.Latitud, longitud = pDatos.Longitud, aplicacion = pDatos.Aplicacion, fecha_alta = DateTime.Now });

                    if (await context.SaveChangesAsync() > 0)
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Insertado correctamente", RET_VALORDEVUELTO = "Insertado correctamente" };
                    else
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -1000, RET_MENSAJEERROR = "No se pudo insertar la visita QR", RET_VALORDEVUELTO = "No se pudo insertar la visita QR" };

                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
