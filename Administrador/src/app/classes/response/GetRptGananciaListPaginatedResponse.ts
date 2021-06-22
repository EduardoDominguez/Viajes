import { RespuestaGeneric } from "../RespuestaGeneric";
import { PaginatedList } from '../PaginatedList';
import { RptGanancia } from '../../classes/RptGanancia';

export class GetRptGananciaListPaginatedResponse extends RespuestaGeneric<PaginatedList<RptGanancia>>{

    constructor()
    {
        super(); // call to default constructor added implicitly
        // pageIndex = 0;
        // pageSize = 0;
        // totalRow= 0;
        // rows = new Array();
    }
}
