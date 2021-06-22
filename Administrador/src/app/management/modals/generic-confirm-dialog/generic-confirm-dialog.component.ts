import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Validators, FormBuilder, FormControl } from '@angular/forms';
import { AlertService } from 'src/app/core/services/alert.service';
import { Subscription } from 'rxjs';
// import { GenericConfirmDialogComponent } from 'src/app/management/modals/generic-confirm-dialog/generic-confirm-dialog.component';
// import { Candidato } from 'src/app/classes/Catalogos/Candidato';
// import { CandidatoLocalService } from 'src/app/core/services/catalogos/candidato-local.service';

@Component({
  selector: 'app-generic-confirm-dialog',
  templateUrl: './generic-confirm-dialog.component.html',
  styleUrls: ['./generic-confirm-dialog.component.scss']
})
export class GenericConfirmDialogComponent implements OnInit, OnDestroy {



  //Variables
  public strIdSeleccionado: string = "";
  // public objCandidatoSeleccionado: Candidato = null;
  public blnNoExistenCandidatos: boolean = true;

  constructor(
    public dialogRef: MatDialogRef<GenericConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public mensajeModal: string,

    // private _mensajesService: AlertService,
  ) {
  }

  ngOnInit() {
  }

  ngOnDestroy() {

  }

  /**
    * Cierra modal
    */
  cancelar(): void {
    this.dialogRef.close(false);
  }

  /**
   * Cierra y selecciona la opcion de aceptar.
   */
   aceptar(): void {

    this.dialogRef.close(true);

  }

}


