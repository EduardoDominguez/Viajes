import { Component, OnInit, OnDestroy, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Validators, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { AlertService } from 'src/app/core/services/alert.service';
import { Subscription } from 'rxjs';
import { BannerService } from 'src/app/core/services/banner.service';
import { StorageService } from 'src/app/core/services/storage.service';
import { ActivatedRoute, Router } from '@angular/router';
import { globals } from 'src/app/core/globals/globals';
import { Banner } from 'src/app/classes/Banner';
import { Location } from '@angular/common';
import { FileValidator } from 'ngx-material-file-input';
import { CreaActualizaBannerRequest } from 'src/app/classes/request/CreaActualizaBannerRequest';
import { Producto } from 'src/app/classes/Producto';
import { ProductoService } from 'src/app/core/services/producto.service';

@Component({
  selector: 'app-form-banner',
  templateUrl: './form-banner.component.html',
  styleUrls: ['./form-banner.component.scss']
})
export class FormBannerComponent implements OnInit, OnDestroy {

  tituloTipoOperacion: string;

  @ViewChild('removableInput', { static: false }) removableInput: any;
  @ViewChild('imgPreview', { static: true }) imgPreview: ElementRef;

  form: FormGroup;
  imagen: string;
  maxSize: number = 10000000;
  tipoOperacion: string = "n";//n = nuevo, e= editar, c= consultar
  isDisabled: boolean = false;
  idBanner: string;

  comboProductos: Producto[];

  //Variables
  public strIdSeleccionado: string = "";
  // public objCandidatoSeleccionado: Candidato = null;
  public blnNoExistenCandidatos: boolean = true;

  constructor(
    public dialogRef: MatDialogRef<FormBannerComponent>,
    @Inject(MAT_DIALOG_DATA) public mensajeModal: string,
    private _storageService: StorageService,
    private _alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    public globales: globals,
    private fb: FormBuilder,
    private _location: Location,
    private _bannerService: BannerService,
    private _productoService: ProductoService,
    // private _mensajesService: AlertService,
  ) {
    this.procesaRutas();
  }

  ngOnInit() {
  }

  ngOnDestroy() {

  }

  procesaRutas() {
    // this.idBanner = this.route.snapshot.paramMap.get('id');
    this.route.queryParamMap.subscribe(queryParams => {
      this.tipoOperacion = (queryParams.get("to") == null) ? "n" : queryParams.get("to").toLocaleLowerCase();
      //this.isDisabled = this.tipoOperacion == 'c';
      this.idBanner = queryParams.get("id");
      this.detectaTipoOperacion(this.tipoOperacion);
    });
  }

  buildForm() {
    this.form = this.fb.group({
      file: [
        { value: undefined, disabled: this.isDisabled },
        [Validators.required, FileValidator.maxContentSize(this.maxSize),],
      ],
      frmNombre: [
        { value: null, disabled: this.isDisabled },
        [Validators.required, Validators.maxLength(250)]
      ],
      cmbProductos: [
        { value: '', disabled: this.isDisabled },
        [Validators.required]
      ]
    });

    setTimeout(() => {
      this.getProductos();
    });
  }

  detectaTipoOperacion(pTipoOperacion: string): void {
    switch (pTipoOperacion.toLocaleLowerCase()) {
      case "n":
        this.tituloTipoOperacion = "Nuevo";
        break;
      case "e":
        // this.comboTipoUsuario.push(new TipoUsuario(1, "Cliente"));
        this.tituloTipoOperacion = "Editar";
        this.getBannerById(this.idBanner);
        break;
      case "c":
        // this.comboTipoUsuario.push(new TipoUsuario(1, "Cliente"));
        this.isDisabled = true;
        this.tituloTipoOperacion = "Consultar";
        this.getBannerById(this.idBanner);

        break;
      default:
        this.tituloTipoOperacion = "";
        break;
    }
    this.buildForm();
  }

  /**
    * Cierra modal
    */
  cancelar(): void {
    this.dialogRef.close(false);
  }

  /**
   * Carga los datos consultados en pantalla
   *  @param pPersona - Objeto tipo persona con datos a cargar
   */
  private setDataOnForm(pBanner: Banner): void {
    this.form.get('frmNombre').setValue(pBanner.Nombre);
    this.form.get('cmbProductos').setValue(pBanner.IdProducto);
    // this.form.get('frmTelefono').setValue(pPersona.Telefono);

    if (!this.isDisabled) {
      this.form.get('file').clearValidators();
      this.form.get('file').updateValueAndValidity();
    }

    this.imgPreview.nativeElement.src = pBanner.Fotografia;
  }

  /**
   * Servicio para consultar productosl.
   */
  private getProductos(): void {
    this._productoService.getProductos(true).subscribe(
      respuesta => {
        // console.log(respuesta);
        if (respuesta.Exito) {
          this.comboProductos = respuesta.Data;
        } else
          this._alertService.showWarning(respuesta.Mensaje);
      }, error => {
        this._alertService.showError(error.message);
      }, () => {

      });
  }

  validar(): boolean {
    if (this.form.invalid) {
      this._alertService.showWarning('Ingrese todos los datos obligatorios');
      return false;
    }

    return true;
  }

  guardar(): void {
    try {
      if (this.validar()) {
        let request = new CreaActualizaBannerRequest();
        request.Nombre = this.form.controls['frmNombre'].value.trim();
        request.IdProducto = this.form.controls['cmbProductos'].value;

        if (this.tipoOperacion == 'n') {
          // console.log(this.form.get("file").value);
          request.Fotografia = (this.form.get("file").value != null) ? this.imgPreview.nativeElement.src : null;
          request.IdPersonaMovimiento = this._storageService.getCurrentUser().IdPersona;

          this._bannerService.agregar(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {
                this.dialogRef.close(true);
                this._alertService.showSuccess(respuesta.Mensaje);
              } else {
                this._alertService.showWarning(respuesta.Mensaje);
              }

            }, error => {
              this._alertService.showError(error.message);
            });

        } else {
          request.IdPersonaMovimiento = this._storageService.getCurrentUser().IdPersona;//this._storageService.getCurrentSession().user.idpersona;
          request.IdBanner = this.idBanner;

          if (this.removableInput.value != null) {
            request.Fotografia = this.imgPreview.nativeElement.src;
          }

          this._bannerService.editar(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {
                this.dialogRef.close(true);
                this._alertService.showSuccess(respuesta.Mensaje);

              } else {
                this._alertService.showWarning(respuesta.Mensaje);
              }
            }, error => {
              this._alertService.showError(error.message);
            });
        }
      }
    } catch (ex) {
      console.log(ex);
      this._alertService.showError(ex.message);
    }
  }

  cambiaImagen() {
    var preview = this.imgPreview.nativeElement;
    var file = (this.removableInput.value == null) ? null : this.removableInput.value.files[0];
    var reader = new FileReader();
    var contenedor = this.removableInput;
    reader.onloadend = function () {
      preview.src = reader.result;
    }

    var img = new Image();
    img.src = window.URL.createObjectURL(file);
    img.onload = function () {
      var width = img.naturalWidth,
        height = img.naturalHeight;

      /* if (!file.type.includes("svg") && (width != 200 || height != 200)) {
         preview.src = "";
         contenedor.clear();
         alert("La imagen debe tener una resolución de 200 x 200");
       } else {*/
      if (file) {
        reader.readAsDataURL(file);
      } else {
        preview.src = "";
      }
      //}
    };
    //console.log(file)
  }


  goBack(): void {
    this._location.back();
  }

  getBannerById(pIdBanner: string): void {
    this._bannerService.getBannersById(pIdBanner).subscribe(
      respuesta => {
        if (respuesta.Exito) {
          // this.dialogRef.close(true);
          this.setDataOnForm(respuesta.Data);
        } else {
          this._alertService.showWarning(respuesta.Mensaje);
        }

      }, error => {
        this._alertService.showError(error.message);
      });
  }
}


