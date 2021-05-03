export class TipoRepartidor {
  public Nombre : string;
  public Clave : number;

  constructor(pClave: number, pNombre: string){
    this.Clave = pClave;
    this.Nombre = pNombre;
  }
}
