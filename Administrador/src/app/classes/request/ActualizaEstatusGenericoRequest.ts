export class ActualizaEstatusGenericoRequest{
    public id :number;
    public estatus :boolean;
    public idpersona_modifica:number;
    public modulo : string;
    
    constructor(pId:number, pEstatus:boolean, pIdPersona:number, pModulo:string){
        this.id = pId;
        this.estatus = pEstatus;
        this.idpersona_modifica = pIdPersona;
        this.modulo = pModulo;
    }
}