import { Local } from './Local';
import { ExtrasProducto } from './ExtrasProducto';
export class Producto {
    IdProducto: number;
    Nombre: string;
    Descripcion: string;
    Precio: number;
    Fotografia: string;
    Estatus: number;
    IdPersonaAlta : number;
    IdPersonaModifica: number;
    IdLocal: number;
    Local: Local;
    Extras: ExtrasProducto[];

    constructor(){
        this.Local = new Local();

    }
}
