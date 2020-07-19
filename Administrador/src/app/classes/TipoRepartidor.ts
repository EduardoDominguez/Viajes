export class TipoRepartidor {
  public Nombre : string;
  public Clave : string;

  constructor(pClave: string, pNombre: string){
    this.Clave = pClave;
    this.Nombre = pNombre;
  }
}
