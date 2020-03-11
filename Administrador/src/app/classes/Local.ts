import { Costo } from './Costo';
import { TipoLocal } from './TipoLocal';

export class Local {
    IdLocal: number;
    Nombre: string;
    Calle: string;
    Colonia: string;
    NoInt: string;
    NoExt: string;
    Referencias: string;
    Fotografia: string;
    Latitud: number;
    Longitud: number;
    Estatus: number;
    IdPersonaModifica: number;
    IdPersonaAlta: number;
    Costo: Costo;
    TipoLocal: TipoLocal;

    constructor(){
        this.Costo = new Costo();
        this.TipoLocal = new TipoLocal();
    }
}