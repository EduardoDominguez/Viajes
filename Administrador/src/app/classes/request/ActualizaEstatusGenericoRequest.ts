export class ActualizaEstatusGenericoRequest<T>{
    public IdRegistro :T;
    public IdEstatus :number;
    public IdPersonaModifica:number;
    

    // public modulo : string;

    constructor(pId:T, pEstatus:number, pIdPersona:number){
        this.IdRegistro = pId;
        this.IdEstatus = pEstatus;
        this.IdPersonaModifica = pIdPersona;
        // this.modulo = pModulo;
    }
}
