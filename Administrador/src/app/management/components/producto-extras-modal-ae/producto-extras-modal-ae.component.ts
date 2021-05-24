import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
// import { NivelJerarquicoCrearRequest } from 'src/app/classes/request/plazas/unidades-organizacionales/NivelJerarquicoCrearRequest';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ProductoService } from 'src/app/core/services/producto.service';
import { AlertService } from 'src/app/core/services/alert.service';
// import { AuthService } from 'src/app/core/services/auth.service';
import { Subscription } from 'rxjs';
import { ExtraProductoRequest } from 'src/app/classes/request/ExtraProductoRequest';
import { StorageService } from 'src/app/core/services/storage.service';

@Component({
  selector: 'app-producto-extras-modal-ae',
  templateUrl: './producto-extras-modal-ae.component.html',
  styleUrls: ['./producto-extras-modal-ae.component.scss']
})
export class ProductoExtrasModalAeComponent implements OnInit, OnDestroy {

  form: FormGroup;
  private subscription: Subscription;

  constructor(
    public dialogRef: MatDialogRef<ProductoExtrasModalAeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ExtraProductoRequest,
    private fb: FormBuilder,
    private _productoService: ProductoService,
    private _mensajesService: AlertService,
    private _storageService: StorageService,
  ) {

    this.buildForm();
  }

  ngOnInit() {

  }

  ngOnDestroy() {
    if (this.subscription)
      this.subscription.unsubscribe();
  }

  /**
   * Crea el formulario en pantalla
   *
   */
  private buildForm(): void {
    //Init form angular
    this.form = this.fb.group({
      frmNombre: [
        { value: this.data.Nombre, disabled: false },
        [Validators.required, Validators.maxLength(100)]
      ],
      frmPrecio: [
        { value: this.data.Precio, disabled: false },
        [Validators.required]
      ]
    });
  }

  /**
  * Valida el formulario antes de guardar datos.
  */
  validar(): boolean {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this._mensajesService.showWarning('Ingrese todos los datos obligatorios y valide que sean datos permitodos.');
      return false;
    }

    return true;
  }

  /**
    * Cierra modal
    */
  cancelar(): void {
    this.dialogRef.close(false);
  }

  /**
    * Ejecuta la opción guardar datos en la BD
    */
  guardar(): void {
    try {
      if (!this.validar())
        return;

      let request = new ExtraProductoRequest()
      request.Nombre = this.form.controls['frmNombre'].value.trim();
      request.IdProducto = this.data.IdProducto;
      request.Precio = this.form.controls['frmPrecio'].value;
      request.Estatus = 1;
      request.IdExtra = this.data.IdExtra;

      request.IdPersona = this._storageService.getCurrentUser().IdPersona;
      if (this.data.TipoOperacion != "u") {
        this.subscription = this._productoService.agregarExtras(request).subscribe(
          respuesta => {
            if (respuesta.Exito) {
              this._mensajesService.showSuccess('Se insertó elemento');
              this.dialogRef.close(true);
            } else {
              this._mensajesService.showWarning(respuesta.Mensaje);
            }
          }, error => {
            this._mensajesService.showWarning(error);
          }
        );
      } else {
        this.subscription = this._productoService.editarExtras(request).subscribe(
          respuesta => {
            if (respuesta.Exito) {
              this._mensajesService.showSuccess('Se actualizó elemento');
              this.dialogRef.close(true);
            } else {
              this._mensajesService.showWarning(respuesta.Mensaje);
            }
          }, error => {
            this._mensajesService.showWarning(error);
          }
        );
      }

    } catch (ex) {
      console.log("guardarExtra --> ", ex);
      this._mensajesService.showError(ex.message);
    }
  }
}
