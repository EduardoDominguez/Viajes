import { Local } from './Local';

export class Producto {
    IdProducto: number;
    Nombre: string;
    Descripcion: string;
    Precio: number;
    Fotografia: string;
    Estatus: number;
    IdPersonaAlta : number;
    IdPersonaModifica: number;
    Local: Local;
    
    constructor(){
        this.Local = new Local();

    }
}