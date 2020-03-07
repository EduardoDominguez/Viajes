namespace WSViajes.Models.Response
{
    public class ConsultaPorIdResponse<T> : Respuesta
    {
        public T Data { get; set; }

    }
}