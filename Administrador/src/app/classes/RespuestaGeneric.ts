// Respuesta para consulta individual
export class RespuestaGeneric<T> {
  public Exito: boolean;
  public CodigoError: number;
  public Mensaje: string;
  public Data: T;
}
