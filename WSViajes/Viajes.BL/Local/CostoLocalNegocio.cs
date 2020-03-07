using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Local;

namespace Viajes.BL.Local
{
    public class CostoLocalNegocio : ICRUD<E_COSTO>
    {
        public E_MENSAJE Agregar(E_COSTO Entidad)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método para consultar costo por id
        /// <param name="pIdCosto">Id del costo a consultar</param>
        /// <returns> Objeto tipo E_COSTO con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_COSTO> ConsultarPorId(int pIdlocal)
        {
            try
            {
                CostoLocalOperaciones pDatos = new CostoLocalOperaciones();
                var pRegistros = await pDatos.Consultar(pIdlocal);
                return pRegistros.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar costos de locales
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_COSTO con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_COSTO>> ConsultarTodo(byte? SoloActivos = null, int? IdGenerico = null)
        {
            try
            {
                CostoLocalOperaciones pDatos = new CostoLocalOperaciones();
                return await pDatos.Consultar(pSoloActivos: SoloActivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public E_MENSAJE Editar(E_COSTO Entidad)
        {
            throw new NotImplementedException();
        }
    }
}

