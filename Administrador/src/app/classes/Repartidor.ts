export class Repartidor {
  public IdPersona: number;
  public Nombre: string;
  public Sexo: string;
  public Edad: number;
  public Telefono: string;
  public Fotografia: string;
  public Estatus: number;
  public Acceso: string;
  public Tipo: number; // 1 Faster, 2 Runner
  public TipoUsuario: number; // 1 Cliente, 2 Conductor, 3 = Admin, 4 = Local
  public IdPersonaAlta: number;
  public IdPersonaModifica: number;

  public Colonia: string;
  public Calle: string;
  public NoExt: string;
  public NoInt: string;
  public Email: string;
  public NoLicencia: string;
  public NoPlacas: string;

  constructor() {

  }
}
