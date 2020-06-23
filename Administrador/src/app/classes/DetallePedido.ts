import { Local } from 'protractor/built/driverProviders';
import { ExtrasProducto } from './ExtrasProducto';

export class DetallePedido {
  public IdPedido: string;
  public IdDetallePedido: string;
  public Local: Local;
  public IdLocal: number;
  public IdProducto: number;
  public NombreProducto: string;
  public Cantidad: number;
  public Precio: number;
  public Observaciones: string;
  public Extras : ExtrasProducto[];
}
