'use strict';

import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor(private toastr: ToastrService) { }

  showSuccess(pMmensaje: string, pTitulo?: string) {
    let titulo = "¡Éxito!";
    if (pTitulo != null)
      titulo = pTitulo;
    this.toastr.success(pMmensaje, titulo);
  }

  showWarning(pMmensaje: string, pTitulo?: string) {
    let titulo = "¡Advertencia!";
    if (pTitulo != null)
      titulo = pTitulo;
    this.toastr.warning(pMmensaje, titulo);
  }

  showError(pMmensaje: string, pTitulo?: string) {
    let titulo = "!Error!";
    if (pTitulo != null)
      titulo = pTitulo;
    this.toastr.error(pMmensaje, titulo);
  }

}
