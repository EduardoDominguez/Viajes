import { CollectionViewer, DataSource } from "@angular/cdk/collections";
import { Observable, BehaviorSubject, of } from "rxjs";
import { catchError, finalize, debounceTime } from "rxjs/operators";
import { GetRptGananciaListPaginatedResponse } from '../../classes/response/GetRptGananciaListPaginatedResponse';
import { ReportesService } from '../services/reportes.service';
import { RptGanancia } from '../../classes/RptGanancia';

export class RptGananciaDataSource implements DataSource<RptGanancia> {

    private rptGananciaSubject = new BehaviorSubject<RptGanancia[]>([]);

    private loadingSubject = new BehaviorSubject<boolean>(false);

    private totalRowsSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();

    public totalRows$ = this.totalRowsSubject.asObservable();

    constructor(private _reportesService: ReportesService) {

    }

    loadRptGanancia(
        pageIndex: number,
        pageSize: number,
        sortColumn: string,
        sortDirection: string,
        palabraClave: string,
        fechaInicial?: Date,
        fechaFinal?: Date
        ) {

        this.loadingSubject.next(true);

        this._reportesService.getListPaginated(pageIndex, pageSize, sortColumn, sortDirection, palabraClave, fechaInicial, fechaFinal)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((response : GetRptGananciaListPaginatedResponse) => {
                if(response.Exito){
                  this.rptGananciaSubject.next(response.Data.Rows)
                  this.totalRowsSubject.next(response.Data.TotalRows)
                }else{
                  this.rptGananciaSubject.next(new Array<RptGanancia>())
                  this.totalRowsSubject.next(0)
                }

            });
    }

    connect(collectionViewer: CollectionViewer): Observable<RptGanancia[]> {
        return this.rptGananciaSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.rptGananciaSubject.complete();
        this.loadingSubject.complete();
        this.totalRowsSubject.complete();
    }

}
