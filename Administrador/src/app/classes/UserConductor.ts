import { AccesoUsuario } from 'src/app/classes/AccesoUsuario';
import { Conductor } from './Conductor';

export class UserConductor {
    public IdPersona: number;
    public Nombre: string;
    public Sexo: string;
    public Edad: number;
    public Telefono: string;
    public Fotografia: string;
    public Estatus: number;
    public TipoUsuario: number;
    public Acceso: AccesoUsuario;
    public Conductor: Conductor;
    constructor() {

    }
}