import { Injectable, Output, EventEmitter } from '@angular/core';

@Injectable()
export class MenuService {

  constructor() { }

  private _open: boolean = false;

  @Output() change: EventEmitter<boolean> = new EventEmitter();

  /**
* CMuestra u oculta el panel loading
* @param pOpen - Indica si debe mostrar o  no el panel loading
*/
  toggle(pOpen?: boolean): void {
    if (pOpen != undefined && pOpen != null)
      this._open = pOpen;
    else
      this._open = !this._open;

    this.change.emit(this._open);
  }
}
