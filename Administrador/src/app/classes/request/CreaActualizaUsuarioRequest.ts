export class CreaActualizaUsuarioRequest {
  public IdPersona: number;
  public Nombre: string;
  public Sexo: string;
  public Telefono: string;
  public Fotografia: string;
  public TipoUsuario: number; // 1 Cliente, 2 Conductor, 3 = Admin, 4 = Local
  public IdPersonaAlta: number;
  public Email: string;
  public IdPersonaModifica: number;

  constructor() {

  }
}
