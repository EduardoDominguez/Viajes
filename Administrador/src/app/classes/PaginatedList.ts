// Respuesta para consulta múltiple
export class PaginatedList<TList> {
  pageIndex: number = 0;
  pageSize: number = 0;
  totalRows: number = 0;
  rows: TList[] = new Array<TList>();
}
