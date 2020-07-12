import { Component, OnInit, ViewChild, ElementRef, OnDestroy, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, GuardsCheckStart } from "@angular/router";
import { AlertService } from "../../../core/services/alert.service";
import { StorageService } from 'src/app/core/services/storage.service';
import { globals } from '../../../core/globals/globals';
import { ProductoService } from '../../../core/services/producto.service';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { FileValidator } from 'ngx-material-file-input';
import { MatSelect, MatDialog } from '@angular/material';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil, take } from 'rxjs/operators';
import { Local } from 'src/app/classes/Local';
import { LocalService } from 'src/app/core/services/local.service';
import { Producto } from 'src/app/classes/Producto';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";
import { Location } from '@angular/common';
import { ExtrasProducto } from 'src/app/classes/ExtrasProducto';
import { ProductoExtrasModalAeComponent } from '../producto-extras-modal-ae/producto-extras-modal-ae.component';
import { ExtraProductoRequest } from 'src/app/classes/request/ExtraProductoRequest';

@AutoUnsubscribe()
@Component({
  selector: 'app-producto-add',
  templateUrl: './producto-add.component.html',
  styleUrls: ['./producto-add.component.scss']
})
export class ProductoAddComponent implements OnInit, AfterViewInit, OnDestroy {
  protected comboNodos: Local[];
  /** control for the selected estrategia */
  public nodoCtrl: FormControl = new FormControl('', [Validators.required]);

  /** control for the MatSelect filter keyword */
  public nodoFilterCtrl: FormControl = new FormControl();

  /** list of banks filtered by search keyword */
  public filteredNodo: ReplaySubject<Local[]> = new ReplaySubject<Local[]>(1);

  @ViewChild('cmbNodo', { static: true }) cmbNodo: MatSelect;

  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  nodoSelectedValue: Local;

  @ViewChild('removableInput', { static: false }) removableInput: any;
  @ViewChild('imgPreview', { static: true }) imgPreview: ElementRef;

  /** Título de la operacion que esta ejecutando */
  tituloTipoOperacion: string;

  //Identifiador del local a agregar  productos
  idLocal: number = 0;

  listaExtras: ExtrasProducto[];

  constructor(
    private _storageService: StorageService,
    private _alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    public globales: globals,
    private fb: FormBuilder,
    private _productoService: ProductoService,
    private _localService: LocalService,
    private _location: Location,
    public _dialogService: MatDialog,
  ) {
    this.procesaRutas();
  }

  form: FormGroup;
  imagen: string;
  maxSize: number = 10000000;
  tipoOperacion: string = "n";//n = nuevo, e= editar, c= consultar
  isDisabled: boolean = false;
  idProducto: number = 0;

  ngOnInit() {
    this.validaPermisosPantalla();
    this.form = this.fb.group({
      file: [
        //{ value: undefined, disabled: this.isDisabled },
        undefined,
        [FileValidator.maxContentSize(this.maxSize),],

      ],
      frmNombre: [
        null,
        [Validators.required, Validators.maxLength(150)]
      ],
      frmDescripcion:
        [
          null,
          [Validators.required, Validators.maxLength(250)]
        ],
      frmPrecio: [
        null,
        [Validators.required,]
      ],
      nodoCtrl: [
        { value: null, disabled: true },
        [Validators.required]
      ],
    });

    setTimeout(() => {
      if (this.idLocal > 0)
        this.getLocalByID(this.idLocal);
      else
        this.getLocales();
    });
  }

  ngAfterViewInit() {
    this.setInitialValue();
  }

  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }

  goBack(): void {
    this._location.back();
  }

  /**
  * Sets the initial value after the filterEstrategia are loaded initially
  */
  protected setInitialValue() {
    this.filteredNodo
      .pipe(take(1), takeUntil(this._onDestroy))
      .subscribe(() => {
        this.cmbNodo.compareWith = (a: Local, b: Local) => a && b && a.IdLocal === b.IdLocal;
      });
  }


  /**
   * Consume servicio para consultar locales.
   */
  private getLocales(pLocal?: Local): void {
    this.comboNodos = new Array<Local>();
    this._localService.getLocales().subscribe(
      respuesta => {
        // console.log(respuesta);
        if (respuesta.Exito) {
          this.comboNodos = respuesta.Data;
          if (pLocal) {
            this.nodoSelectedValue = pLocal;
          }
          this.filteredNodo.next(this.comboNodos.slice());
          this.nodoFilterCtrl.valueChanges
            .pipe(takeUntil(this._onDestroy))
            .subscribe(() => {
              this.filterNodos();
            });
        } else
          this._alertService.showWarning(respuesta.Mensaje);
      }, error => {
        this._alertService.showError(error.message);
      });
  }

  private getLocalByID(pIdLocal: number): void {
    this._localService.getLocalByID(pIdLocal).subscribe(
      respuesta => {
        // console.log(respuesta);
        if (respuesta.Exito) {
          this.getLocales(respuesta.Data);
          this.form.controls['nodoCtrl'].setValue(respuesta.Data);
        } else
          this._alertService.showWarning(respuesta.Mensaje);
      }, error => {
        this._alertService.showError(error.message);
      });
  }


  protected filterNodos() {
    if (!this.comboNodos) {
      return;
    }
    // get the search keyword
    let search = this.nodoFilterCtrl.value;
    //console.log(search);
    if (!search) {
      this.filteredNodo.next(this.comboNodos.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    //let filtro = this.comboNodos.filter(nodo => nodo.nombre.toLowerCase().indexOf(search));
    //console.log(filtro);
    // filter the banks
    this.filteredNodo.next(
      this.comboNodos.filter(local => local.Nombre.toLowerCase().indexOf(search) > -1)
    );
  }

  cargaDatosProducto(pProducto: Producto) {
    //console.log(pProducto);
    this.form.controls['frmNombre'].setValue(pProducto.Nombre);
    this.form.controls['frmDescripcion'].setValue(pProducto.Descripcion);
    this.form.controls['frmPrecio'].setValue(pProducto.Precio);
    this.form.controls['nodoCtrl'].setValue(pProducto.Local);

    // this.txtNombreProducto = pProducto.Nombre;
    // this.nodoSelectedValue = pProducto.Local;
    // this.txtPrecio = pProducto.Precio;
    // this.txtDescripcion = pProducto.Descripcion;
    this.imgPreview.nativeElement.src = pProducto.Fotografia;
    this.listaExtras = pProducto.Extras;

    if (!this.isDisabled) {
      this.form.get('file').clearValidators();
      this.form.get('file').updateValueAndValidity();
    }
  }

  procesaRutas() {
    this.idProducto = +this.route.snapshot.paramMap.get('id');
    this.route.queryParamMap.subscribe(queryParams => {
      this.tipoOperacion = (queryParams.get("to") == null) ? "n" : queryParams.get("to").toLocaleLowerCase();
      this.idLocal = (queryParams.get("idLocal") == null) ? 0 : parseInt(queryParams.get("idLocal"));

      this.isDisabled = this.tipoOperacion == ('c' || 'n');
      this.detectaTipoOperacion(this.tipoOperacion);
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

  private getProducto(pIdProducto: number) {
    this._productoService.getProductoByID(pIdProducto).subscribe(
      respuesta => {
        console.log(respuesta)
        if (respuesta.Exito) {
          this.cargaDatosProducto(respuesta.Data)
        } else
          this._alertService.showWarning(respuesta.Mensaje);

      }, error => {
        this._alertService.showError(error.message);
      });
  }

  validaPermisosPantalla() {
    setTimeout(() => {
      if (this.idProducto > 0)
        this.getProducto(this.idProducto);

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
        let request = new Producto();
        request.Nombre = this.form.controls['frmNombre'].value.trim();
        request.Descripcion = this.form.controls['frmDescripcion'].value.trim();
        request.IdLocal = this.form.controls['nodoCtrl'].value.IdLocal;// this.nodoSelectedValue.IdLocal;
        request.Precio = this.form.controls['frmPrecio'].value;

        console.log(request);
        //if (request.tipo_operacion == 'n') {
        if (this.tipoOperacion == 'n') {
          request.Fotografia = (this.imgPreview.nativeElement.src) ? this.imgPreview.nativeElement.src : null;
          //request.etapa.nombre_imagen = this.removableInput.value._fileNames;
          request.IdPersonaAlta = this._storageService.getCurrentUser().IdPersona;//this._storageService.getCurrentSession().user.idpersona;

          this._productoService.agregar(request).subscribe(
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

        } else {
          request.IdPersonaModifica = this._storageService.getCurrentUser().IdPersona;//this._storageService.getCurrentSession().user.idpersona;
          request.IdProducto = this.idProducto;

          if (this.removableInput.value != null) {
            request.Fotografia = this.imgPreview.nativeElement.src;
            //request.nodo.nombre_imagen = this.removableInput.value._fileNames;
          }

          this._productoService.editar(request).subscribe(
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

  limpiaCamposFormulario(): void {
    // this.txtDescripcion = "";
    // this.txtNombreProducto = "";
    // this.txtPrecio = 0;
    this.imagen = null;
    this.nodoSelectedValue = new Local();
    this.removableInput.clear();
    //this.sele
    this.removableInput.clear();
    this.cambiaImagen();
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

  /**
   * Abre el panel para agregar una adscripción
   */
  openAgregarExtra(): void {
    let datos = new ExtraProductoRequest();
    datos.IdProducto = this.idProducto;
    const dialogRef = this._dialogService.open(ProductoExtrasModalAeComponent, {
      // width: '600px',
      // height: 'calc(100% - 100px)',
      panelClass: 'custom-dialog-container',
      data: datos,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // this.reLoadGridPage();
      }
    });
  }
}
