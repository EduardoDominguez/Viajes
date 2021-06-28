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
            .subscribe(response => {
                console.log(response)
                this.rptGananciaSubject.next((response as GetRptGananciaListPaginatedResponse).Data.rows)
                this.totalRowsSubject.next((response as GetRptGananciaListPaginatedResponse).Data.totalRows)
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
