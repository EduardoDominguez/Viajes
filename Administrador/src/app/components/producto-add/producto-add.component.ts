import { Component, OnInit, ViewChild, ElementRef, OnDestroy, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, GuardsCheckStart } from "@angular/router";
import { AlertService } from "../../services/alert.service";
import { StorageService } from 'src/app/services/storage.service';
import { globals } from '../../globals/globals';
import { ProductoService } from '../../services/producto.service';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { FileValidator } from 'ngx-material-file-input';
import { MatSelect } from '@angular/material';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil, take } from 'rxjs/operators';
import { Local } from 'src/app/classes/Local';
import { LocalService } from 'src/app/services/local.service';
import { Producto } from 'src/app/classes/Producto';

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

  constructor(
    private _storageService: StorageService,
    private _alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    public globales: globals,
    private fb: FormBuilder,
    private _productoService: ProductoService,
    private _localService: LocalService, ) {
    this.procesaRutas();
  }

  form: FormGroup;
  imagen: string;
  maxSize: number = 10000000;
  txtNombreProducto: string;
  txtDescripcion: string;
  txtPrecio: number;
  tipoOperacion: string = "n";//n = nuevo, e= editar, c= consultar
  isDisabled: boolean = false;
  idProducto: number = 0;

  ngOnInit() {
    this.validaPermisosPantalla();
    this.form = this.fb.group({
      file: [
        //{ value: undefined, disabled: this.isDisabled },
        undefined,
        [Validators.required, FileValidator.maxContentSize(this.maxSize),],

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
        null,
        [Validators.required]
      ],
    });

    setTimeout(() => {
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
        console.log(respuesta);
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
    this.txtNombreProducto = pProducto.Nombre;
    this.nodoSelectedValue = pProducto.Local;
    this.txtPrecio = pProducto.Precio;
    this.txtDescripcion = pProducto.Descripcion;
    this.imgPreview.nativeElement.src = pProducto.Fotografia;

    if (!this.isDisabled) {
      this.form.get('file').clearValidators();
      this.form.get('file').updateValueAndValidity();
    }
  }

  procesaRutas() {
    this.idProducto = +this.route.snapshot.paramMap.get('id');
    this.route.queryParamMap.subscribe(queryParams => {
      this.tipoOperacion = (queryParams.get("to") == null) ? "n" : queryParams.get("to").toLocaleLowerCase();
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
        //console.log(request, this.nodoSelectedValue)
        request.IdProducto = this.idProducto;
        request.Nombre = this.txtNombreProducto;
        request.Descripcion = this.txtDescripcion;
        request.Local.IdLocal = this.nodoSelectedValue.IdLocal;
        request.Precio = this.txtPrecio;
        request.IdPersonaAlta = 1;//this._storageService.getCurrentSession().user.idpersona;
        request.IdPersonaModifica = 1;//this._storageService.getCurrentSession().user.idpersona;

        //if (request.tipo_operacion == 'n') {
        if (this.tipoOperacion == 'n') {
          request.Fotografia = this.imgPreview.nativeElement.src;
          //request.etapa.nombre_imagen = this.removableInput.value._fileNames;

          this._productoService.agregar(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {

                this.router.navigate(['/administracion/productos']);
                this._alertService.showSuccess(respuesta.Mensaje);
                //this.limpiaCamposFormulario();

              } else {
                this._alertService.showWarning(respuesta.Mensaje);
              }

            }, error => {
              this._alertService.showError(error.message);
            });

        } else {
          if (this.removableInput.value != null) {
            request.Fotografia = this.imgPreview.nativeElement.src;
            //request.nodo.nombre_imagen = this.removableInput.value._fileNames;
          }
          this._productoService.editar(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {

                this.router.navigate(['/administracion/productos']);
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
    this.txtDescripcion = "";
    this.txtNombreProducto = "";
    this.txtPrecio = 0;
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

}