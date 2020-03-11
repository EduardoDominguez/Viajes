export class User {
    public idpersona: number;
    public nombre: string;
    public apepaterno: string;
    public apematerno: string;
    public full_name: string;
    public nombreusuario: string;
    public correo: string;
    constructor() {
        this.full_name = `${this.nombre} ${this.apepaterno} ${this.apematerno}`
    }
}