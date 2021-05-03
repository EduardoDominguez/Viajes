import { AccesoUsuario } from 'src/app/classes/AccesoUsuario';

export class User {
    public IdPersona: number;
    public Nombre: string;
    public Sexo: string;
    public Edad: number;
    public Telefono: string;
    public Fotografia: string;
    public Estatus: number;
    public TipoUsuario: number;
    public Acceso: AccesoUsuario;
    constructor() {

    }
}
