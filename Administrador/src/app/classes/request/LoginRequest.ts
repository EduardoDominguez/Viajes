export class LoginRequest {
  public Email: string;
  public Password: string;
  public TipoUsuario: number;
  constructor( object: any){
    this.Email = (object.Email) ? object.Email : null;
    this.Password = (object.Password) ? object.Password : null;
    this.TipoUsuario = (object.TipoUsuario) ? object.TipoUsuario : null;
  }
}
