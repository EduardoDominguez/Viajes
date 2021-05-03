import { Component, OnInit, ViewChild, ElementRef, OnDestroy, AfterViewInit, NgZone } from '@angular/core';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Router, ActivatedRoute, GuardsCheckStart } from "@angular/router";
import { AlertService } from "../../../core/services/alert.service";
import { StorageService } from 'src/app/core/services/storage.service';
import { globals } from '../../../core/globals/globals';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { FileValidator } from 'ngx-material-file-input';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";
import { Location } from '@angular/common';
import { TipoSexo } from 'src/app/classes/TipoSexo';
import { PersonaService } from 'src/app/core/services/persona.service';
import { TipoUsuario } from 'src/app/classes/TipoUsuario';
import { CreaActualizaUsuarioRequest } from 'src/app/classes/request/CreaActualizaUsuarioRequest';
import { User } from 'src/app/classes/User';
import { TipoUsuarioEnum } from 'src/app/classes/enums/TipoUsuarioEnum';

@AutoUnsubscribe()
@Component({
  selector: 'app-usuarios-add',
  templateUrl: './usuarios-add.component.html',
  styleUrls: ['./usuarios-add.component.scss']
})
export class UsuariosAddComponent implements OnInit, OnDestroy {

  /** Título de la operacion que esta ejecutando */
  tituloTipoOperacion: string;

  @ViewChild('removableInput', { static: false }) removableInput: any;
  @ViewChild('imgPreview', { static: true }) imgPreview: ElementRef;

  form: FormGroup;
  imagen: string;
  maxSize: number = 10000000;
  tipoOperacion: string = "n";//n = nuevo, e= editar, c= consultar
  isDisabled: boolean = false;
  idPersona: number;

  comboSexo: TipoSexo[] = new Array<TipoSexo>();
  comboTipoUsuario: TipoUsuario[] = new Array<TipoUsuario>();

  constructor(
    private _storageService: StorageService,
    private _alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    public globales: globals,
    private fb: FormBuilder,
    private _location: Location,
    private _personaService: PersonaService,
  ) {
    this.procesaRutas();
  }

  ngOnInit() {
    this.comboSexo.push(new TipoSexo("M", "Masculino"));
    this.comboSexo.push(new TipoSexo("F", "Femenino"));

    this.comboTipoUsuario.push(new TipoUsuario(4, "Administrador Local"));
    this.comboTipoUsuario.push(new TipoUsuario(3, "Administrador sistema FastRun"));
  }

  ngOnDestroy() {
    // this._onDestroy.next();
    // this._onDestroy.complete();
  }

  goBack(): void {
    this._location.back();
  }

  procesaRutas() {
    this.idPersona = +this.route.snapshot.paramMap.get('id');
    this.route.queryParamMap.subscribe(queryParams => {
      this.tipoOperacion = (queryParams.get("to") == null) ? "n" : queryParams.get("to").toLocaleLowerCase();
      //this.isDisabled = this.tipoOperacion == 'c';
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
        {value: null, disabled: this.isDisabled},
        [Validators.required, Validators.maxLength(250)]
      ],
      cmbTipoUsuario: [
        {value: '', disabled: this.isDisabled},
        [Validators.required]
      ],
      frmTelefono: [
        {value: '', disabled: this.isDisabled},
        [Validators.required]
      ],
      cmbSexo: [
        {value: '', disabled: this.isDisabled},
        [Validators.required]
      ],
      frmEmail: [
        {value: '', disabled: this.isDisabled},
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
        this.comboTipoUsuario.push(new TipoUsuario(1, "Cliente"));
        this.tituloTipoOperacion = "Editar";
        this.getUsurarioById(this.idPersona);
        break;
      case "c":
        this.comboTipoUsuario.push(new TipoUsuario(1, "Cliente"));
        this.isDisabled = true;
        this.tituloTipoOperacion = "Consultar";
        this.getUsurarioById(this.idPersona);
        
        break;
      default:
        this.tituloTipoOperacion = "";
        break;
    }
    this.buildForm();
  }


  /**
   * Consume servicio para consultar un usuario
   * @param pIdPersona - Identificador de la persona a consultar
   */
   public getUsurarioById(pIdPersona: number): void {
    this._personaService.getPersonaById(pIdPersona).subscribe(
      respuesta => {
        // console.log(respuesta);
        if (respuesta.Exito) {
          this.setDataOnForm(respuesta.Data);
        } else
          this._alertService.showWarning(respuesta.Mensaje);
      }, error => {
        this._alertService.showError(error.message);
      });
  }

  /**
   * Carga los datos consultados en pantalla
   *  @param pPersona - Objeto tipo persona con datos a cargar
   */
  private setDataOnForm(pPersona: User): void{
    this.form.get('frmNombre').setValue(pPersona.Nombre);
    this.form.get('cmbTipoUsuario').setValue(pPersona.Acceso.TipoUsuario);
    this.form.get('frmTelefono').setValue(pPersona.Telefono);
    this.form.get('cmbSexo').setValue(pPersona.Sexo);
    this.form.get('frmEmail').setValue(pPersona.Acceso.Email);

    if(pPersona.Acceso.TipoUsuario == TipoUsuarioEnum.CLIENTE){
      this.form.get('cmbTipoUsuario').disable();
    }

    if (!this.isDisabled) {
      this.form.get('file').clearValidators();
      this.form.get('file').updateValueAndValidity();
    }

    this.form.get('frmEmail').disable();

    this.imgPreview.nativeElement.src = pPersona.Fotografia;
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
        let request = new CreaActualizaUsuarioRequest();
        request.Nombre = this.form.controls['frmNombre'].value.trim();
        request.Sexo = this.form.controls['cmbSexo'].value;
        request.TipoUsuario = this.form.controls['cmbTipoUsuario'].value;
        request.Telefono = this.form.controls['frmTelefono'].value;
        request.Email = this.form.controls['frmEmail'].value;

        // console.log(request);
        //if (request.tipo_operacion == 'n') {
        if (this.tipoOperacion == 'n') {
          // console.log(this.form.get("file").value);
          request.Fotografia = (this.form.get("file").value != null) ? this.imgPreview.nativeElement.src : null;
          request.IdPersonaAlta = this._storageService.getCurrentUser().IdPersona;

          this._personaService.agregar(request).subscribe(
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
          request.IdPersona = this.idPersona;

          if (this.removableInput.value != null) {
            request.Fotografia = this.imgPreview.nativeElement.src;
          }

          this._personaService.editar(request).subscribe(
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
