export class ActualizaEstatusGenericoRequest{
    public IdRegistro :number;
    public IdEstatus :number;
    public IdPersonaModifica:number;
    // public modulo : string;
    
    constructor(pId:number, pEstatus:number, pIdPersona:number){
        this.IdRegistro = pId;
        this.IdEstatus = pEstatus;
        this.IdPersonaModifica = pIdPersona;
        // this.modulo = pModulo;
    }
}