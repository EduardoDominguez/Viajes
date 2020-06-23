import { User } from './User';
import { Direcion } from './Direccion';
import { EstatusPedido } from './EstatusPedido';
import { DetallePedido } from './DetallePedido';

export class Pedido {
  public IdPedido: string;
  public PersonaPide: User;
  public DireccionEntrega: Direcion;
  public PersonaEntrega: User;
  public IdEncuesta: number;
  public Estatus: EstatusPedido;
  public IdMetodoPago: number;
  public Calificacion: number;
  public Observaciones: string;
  public FechaPedido: Date;
  public HoraPedido: Date;
  public FechaEntrega: Date;
  public HoraEntrega: Date;
  public Folio: string;
  public ReferenciaPago: string;
  public CostoEnvio: number;
  public Detalle: DetallePedido;
  public TipoPedido: number;

}
