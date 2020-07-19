import { Component, OnInit, ViewChild, ElementRef, OnDestroy, AfterViewInit, NgZone } from '@angular/core';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Router, ActivatedRoute, GuardsCheckStart } from "@angular/router";
import { AlertService } from "../../../core/services/alert.service";
import { StorageService } from 'src/app/core/services/storage.service';
import { globals } from '../../../core/globals/globals';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { FileValidator } from 'ngx-material-file-input';
import { MatSelect } from '@angular/material/select';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil, take } from 'rxjs/operators';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";
import { Location } from '@angular/common';
import { TipoSexo } from 'src/app/classes/TipoSexo';
import { TipoRepartidor } from 'src/app/classes/TipoRepartidor';
import { Repartidor } from 'src/app/classes/Repartidor';
import { PersonaService } from 'src/app/core/services/persona.service';

@AutoUnsubscribe()
@Component({
  selector: 'app-repartidor-add',
  templateUrl: './repartidor-add.component.html',
  styleUrls: ['./repartidor-add.component.scss']
})
export class RepartidorAddComponent implements OnInit, OnDestroy {

  /** Título de la operacion que esta ejecutando */
  tituloTipoOperacion: string;

  @ViewChild('removableInput', { static: false }) removableInput: any;
  @ViewChild('imgPreview', { static: true }) imgPreview: ElementRef;

  form: FormGroup;
  imagen: string;
  maxSize: number = 10000000;
  tipoOperacion: string = "n";//n = nuevo, e= editar, c= consultar
  isDisabled: boolean = false;
  idRepartidor: number;

  comboSexo: TipoSexo[] = new Array<TipoSexo>();
  comboTipoRepartidor: TipoRepartidor[] = new Array<TipoRepartidor>();

  constructor(
    private _storageService: StorageService,
    private _alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    public globales: globals,
    private fb: FormBuilder,
    private _location: Location,
    private _repartidorService: PersonaService,
  ) {
    this.buildForm();
    this.procesaRutas();
  }

  ngOnInit() {
    this.comboSexo.push(new TipoSexo("F", "Masculino"));
    this.comboSexo.push(new TipoSexo("M", "Femenino"));

    this.comboTipoRepartidor.push(new TipoRepartidor("1", "FASTER"));
    this.comboTipoRepartidor.push(new TipoSexo("2", "RUNNER"));
  }

  ngOnDestroy() {
    // this._onDestroy.next();
    // this._onDestroy.complete();
  }

  goBack(): void {
    this._location.back();
  }

  procesaRutas() {
    this.idRepartidor = +this.route.snapshot.paramMap.get('id');
    this.route.queryParamMap.subscribe(queryParams => {
      this.tipoOperacion = (queryParams.get("to") == null) ? "n" : queryParams.get("to").toLocaleLowerCase();
      this.isDisabled = this.tipoOperacion == ('c' || 'n');
      this.detectaTipoOperacion(this.tipoOperacion);
    });
  }

  buildForm() {
    this.form = this.fb.group({
      file: [
        //{ value: undefined, disabled: this.isDisabled },
        undefined,
        [Validators.required, FileValidator.maxContentSize(this.maxSize),],

      ],
      frmNombre: [
        null,
        [Validators.required, Validators.maxLength(250)]
      ],
      frmCalle: [
        null,
        [Validators.required, Validators.maxLength(400)]
      ],
      frmNoExt: [
        null,
        [Validators.required, Validators.maxLength(10)]
      ],
      frmNoInt: [
        null,
        [Validators.maxLength(50), Validators.nullValidator]
      ],
      frmColonia: [
        null,
        [Validators.required, Validators.maxLength(400)]
      ],
      cmbTipoRepartidor: [
        '',
        [Validators.required]
      ],
      frmNoPlacas: [
        '',
        [Validators.required]
      ],
      frmNoLicencia: [
        '',
        [Validators.required]
      ],
      frmTelefono: [
        '',
        [Validators.required]
      ],
      cmbSexo: [
        '',
        [Validators.required]
      ],
      frmEmail: [
        '',
        [Validators.required, Validators.email]
      ],
    });
  }

  detectaTipoOperacion(pTipoOperacion: string): void {
    switch (pTipoOperacion.toLocaleLowerCase()) {
      case "n":
        this.tituloTipoOperacion = "Nuevo";
        break;
      case "e":
        this.tituloTipoOperacion = "Editar";
        break;
      case "c":
        this.tituloTipoOperacion = "Consultar";
        break;
      default:
        this.tituloTipoOperacion = "";
        break;
    }
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
        let request = new Repartidor();
        request.Nombre = this.form.controls['frmNombre'].value.trim();
        request.Sexo = this.form.controls['cmbSexo'].value;
        request.Telefono = this.form.controls['frmTelefono'].value;
        request.Tipo = this.form.controls['cmbTipoRepartidor'].value;

        request.Colonia = this.form.controls['frmColonia'].value;
        request.Calle = this.form.controls['frmCalle'].value;
        request.NoExt = this.form.controls['frmNoExt'].value;
        request.NoInt = this.form.controls['frmNoInt'].value;
        request.Telefono = this.form.controls['frmTelefono'].value;
        request.Email = this.form.controls['frmEmail'].value;


        console.log(request);
        //if (request.tipo_operacion == 'n') {
        if (this.tipoOperacion == 'n') {
          // console.log(this.form.get("file").value);
          request.Fotografia = (this.form.get("file").value != null) ? this.imgPreview.nativeElement.src : null;
          request.IdPersonaAlta = this._storageService.getCurrentUser().IdPersona;

          this._repartidorService.agregarRepartidor(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {
                this.goBack();
                this._alertService.showSuccess(respuesta.Mensaje);
              } else {
                this._alertService.showWarning(respuesta.Mensaje);
              }

            }, error => {
              this._alertService.showError(error.message);
            });

        } else {
          request.IdPersonaModifica = this._storageService.getCurrentUser().IdPersona;//this._storageService.getCurrentSession().user.idpersona;
          request.IdPersona = this.idRepartidor;

          if (this.removableInput.value != null) {
            request.Fotografia = this.imgPreview.nativeElement.src;
          }

          this._repartidorService.editarRepartidor(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {
                // this.router.navigate(['/admin-dashboard/administracion/productos']);
                this.goBack();
                this._alertService.showSuccess(respuesta.Mensaje);
                //this.limpiaCamposFormulario();

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
}
