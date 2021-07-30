import { CollectionViewer, DataSource } from "@angular/cdk/collections";
import { Observable, BehaviorSubject, of } from "rxjs";
import { catchError, finalize, debounceTime } from "rxjs/operators";
import { GetRptGeneralListPaginatedResponse } from '../../classes/response/GetRptGeneralListPaginatedResponse';
import { ReportesService } from '../services/reportes.service';
import { RptGeneral } from '../../classes/RptGeneral';

export class RptGeneralDataSource implements DataSource<RptGeneral> {

    private rptGeneralSubject = new BehaviorSubject<RptGeneral[]>([]);

    private loadingSubject = new BehaviorSubject<boolean>(false);

    private totalRowsSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();

    public totalRows$ = this.totalRowsSubject.asObservable();

    constructor(private _reportesService: ReportesService) {

    }

    loadRptGeneral(
        pageIndex: number,
        pageSize: number,
        sortColumn: string,
        sortDirection: string,
        palabraClave: string,
        fechaInicial?: Date,
        fechaFinal?: Date
        ) {

        this.loadingSubject.next(true);

        this._reportesService.getListPaginatedGeneral(pageIndex, pageSize, sortColumn, sortDirection, palabraClave, fechaInicial, fechaFinal)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe(response => {
                console.log(response)
                this.rptGeneralSubject.next((response as GetRptGeneralListPaginatedResponse).Data.Rows)
                this.totalRowsSubject.next((response as GetRptGeneralListPaginatedResponse).Data.TotalRows)
            });
    }

    connect(collectionViewer: CollectionViewer): Observable<RptGeneral[]> {
        return this.rptGeneralSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.rptGeneralSubject.complete();
        this.loadingSubject.complete();
        this.totalRowsSubject.complete();
    }

}
