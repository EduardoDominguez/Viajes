using System.Collections.Generic;
using Viajes.EL.Extras;
using System.Threading.Tasks;

namespace Viajes.EL.Interfaz
{
    public interface ICRUD<T>
    {
        E_MENSAJE Agregar(T pEntidad);
        E_MENSAJE Editar(T pEntidad);
        Task<List<T>> ConsultarTodo(byte? pSoloActivos, int? pIdGenerico);
        Task<T> ConsultarPorId(int pId);

    }
}
