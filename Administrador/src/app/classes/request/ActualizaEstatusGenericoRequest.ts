export class ActualizaEstatusGenericoRequest{
    public IdRegistro :number;
    public IdEstatus :number;
    public IdPersonaModifica:number;
    public IdPersonaMovimiento:number; //Usar este parametro o el anterior es lo mismo

    // public modulo : string;

    constructor(pId:number, pEstatus:number, pIdPersona:number, pIdPersonaMovimiento?: number){
        this.IdRegistro = pId;
        this.IdEstatus = pEstatus;
        this.IdPersonaModifica = pIdPersona;
        this.IdPersonaMovimiento = pIdPersonaMovimiento;
        // this.modulo = pModulo;
    }
}
