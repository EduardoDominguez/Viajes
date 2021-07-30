import { RespuestaGeneric } from "../RespuestaGeneric";
import { PaginatedList } from '../PaginatedList';
import { RptGeneral } from '../../classes/RptGeneral';

export class GetRptGeneralListPaginatedResponse extends RespuestaGeneric<PaginatedList<RptGeneral>>{

    constructor()
    {
        super();
    }
}
