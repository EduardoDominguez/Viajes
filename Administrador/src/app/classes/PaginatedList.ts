// Respuesta para consulta múltiple
export class PaginatedList<TList> {
  PageIndex: number = 0;
  PageSize: number = 0;
  TotalRows: number = 0;
  Rows: TList[] = new Array<TList>();
}
