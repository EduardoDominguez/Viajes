import { Injectable, Output, EventEmitter } from '@angular/core';

@Injectable()
export class LoadingService {

  constructor() { }

  private show: boolean = false;

  @Output() change: EventEmitter<boolean> = new EventEmitter();

  /**
* CMuestra u oculta el panel loading
* @param pShow - Indica si debe mostrar o  no el panel loading
*/
  toggle(pShow?: boolean): void {
    if (pShow != undefined && pShow != null)
      this.show = pShow;
    else
      this.show = !this.show;

    this.change.emit(this.show);
  }
}
