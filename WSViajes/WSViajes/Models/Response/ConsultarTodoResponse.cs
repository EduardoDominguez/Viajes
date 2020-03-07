using System.Collections.Generic;

namespace WSViajes.Models.Response
{
    public class ConsultarTodoResponse<T> : Respuesta
    {
        public List<T> Data { get; set; }

    }
}