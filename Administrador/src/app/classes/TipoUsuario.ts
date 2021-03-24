export class TipoUsuario {
  IdTipoUsuario: number;
  Nombre: string;
  Descripcion: string;
  Estatus: number;
  IdPersonaModifica: number;
  IdPersonaAlta: number;

  constructor(pIdTipoUsuario: number, pNombre: string){
    this.IdTipoUsuario = pIdTipoUsuario;
    this.Nombre = pNombre;
  }
}
