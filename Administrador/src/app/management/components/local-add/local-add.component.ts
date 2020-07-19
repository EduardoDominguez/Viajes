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
import { Local } from 'src/app/classes/Local';
import { LocalService } from 'src/app/core/services/local.service';
import { TipoLocal } from 'src/app/classes/TipoLocal';
import { Costo } from 'src/app/classes/Costo';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";

@AutoUnsubscribe()
@Component({
  selector: 'app-local-add',
  templateUrl: './local-add.component.html',
  styleUrls: ['./local-add.component.scss']
})
export class LocalAddComponent implements OnInit, AfterViewInit, OnDestroy {
  protected comboNodos: Local[];

  comboTipoLocal: TipoLocal[];

  comboCostos: Costo[];

  costoSelectedValue: Costo;
  tipoLocalSelectedValue: TipoLocal;

  //Para el mapa
  latitude: number;
  longitude: number;
  zoom: number;
  address: string;
  private geoCoder;


  @ViewChild('search', { static: false })
  public searchElementRef: ElementRef;

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
    private _localService: LocalService,
    private _mapsAPILoader: MapsAPILoader,
    private _ngZone: NgZone) {
    this.procesaRutas();
  }

  form: FormGroup;
  imagen: string;
  maxSize: number = 10000000;
  tipoOperacion: string = "n";//n = nuevo, e= editar, c= consultar
  isDisabled: boolean = false;
  idLocal: number = 0;

  ngOnInit() {

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
      frmReferencias:
        [
          null,
          [Validators.required, Validators.maxLength(500)]
        ],
      cmbTipoLocal: [
        '',
        [Validators.required]
      ],
      cmbCostoLocal: [
        '',
        [Validators.required]
      ],
    });

    this.validaPermisosPantalla();

    setTimeout(() => {
      this.initialLoadMap();
      this.getTiposLocal();
      this.getCostos();
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

  cargaDatosLocal(pLocal: Local) {
    //console.log(pProducto);
    this.form.controls['frmNombre'].setValue(pLocal.Nombre);
    this.form.controls['frmReferencias'].setValue(pLocal.Referencias);
    this.form.controls['cmbTipoLocal'].setValue(pLocal.TipoLocal.IdTipoLocal);
    this.form.controls['cmbCostoLocal'].setValue(pLocal.Costo.IdCosto);
    this.form.controls['frmCalle'].setValue(pLocal.Calle);
    this.form.controls['frmColonia'].setValue(pLocal.Colonia);
    this.form.controls['frmNoExt'].setValue(pLocal.NoExt);
    this.form.controls['frmNoInt'].setValue(pLocal.NoInt);
    this.latitude = pLocal.Latitud;
    this.longitude = pLocal.Longitud
    this.getAddress(this.latitude, this.longitude);
    this.imgPreview.nativeElement.src = pLocal.Fotografia;

    if (!this.isDisabled) {
      this.form.get('file').clearValidators();
      this.form.get('file').updateValueAndValidity();
    }
  }

  procesaRutas() {
    this.idLocal = +this.route.snapshot.paramMap.get('id');
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

  private getLocal(pIdLocal: number) {
    this._localService.getLocalByID(pIdLocal).subscribe(
      respuesta => {
        if (respuesta.Exito) {
          this.cargaDatosLocal(respuesta.Data)
        } else
          this._alertService.showWarning(respuesta.Mensaje);

      }, error => {
        this._alertService.showError(error.message);
      });
  }

  validaPermisosPantalla() {

    setTimeout(() => {

      if (this.idLocal > 0)
        this.getLocal(this.idLocal);

    });
  }

  validar(): boolean {
    //console.log(this.form)
    if (this.form.invalid) {
      this._alertService.showWarning('Ingrese todos los datos obligatorios');
      return false;
    }

    return true;
  }

  guardar(): void {
    try {
      if (this.validar()) {
        let request = new Local();
        request.Nombre = this.form.controls['frmNombre'].value;
        request.Referencias = this.form.controls['frmReferencias'].value;
        request.TipoLocal.IdTipoLocal = this.form.controls['cmbTipoLocal'].value;
        request.Costo.IdCosto = this.form.controls['cmbCostoLocal'].value;
        request.Calle = this.form.controls['frmCalle'].value;
        request.Colonia = this.form.controls['frmColonia'].value;
        request.NoExt = this.form.controls['frmNoExt'].value;
        request.NoInt = this.form.controls['frmNoInt'].value;
        request.Latitud = this.latitude;
        request.Longitud = this.longitude;
        //1;//this._storageService.getCurrentSession().user.idpersona;


        // console.log(request);

        if (this.tipoOperacion == 'n') {
          request.Fotografia = this.imgPreview.nativeElement.src;
          request.IdPersonaAlta = this._storageService.getCurrentUser().IdPersona;

          this._localService.agregar(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {

                this.router.navigate(['/admin-dashboard/administracion/locales']);
                this._alertService.showSuccess(respuesta.Mensaje);
                //this.limpiaCamposFormulario();

              } else {
                this._alertService.showWarning(respuesta.Mensaje);
              }

            }, error => {
              this._alertService.showError(error.message);
            });

        } else {
          request.IdLocal = this.idLocal;
          request.IdPersonaModifica = this._storageService.getCurrentUser().IdPersona;
          if (this.removableInput.value != null) {
            request.Fotografia = this.imgPreview.nativeElement.src;
            //request.nodo.nombre_imagen = this.removableInput.value._fileNames;
          }
          this._localService.editar(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {

                this.router.navigate(['/admin-dashboard/administracion/locales']);
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

  limpiaCamposFormulario(): void {
    this.form.clearValidators();
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
  }


  /**
   * Consume servicio para consultar tipos de locales.
   */
  private getTiposLocal(pTipoLocal?: TipoLocal): void {
    this.comboTipoLocal = new Array<TipoLocal>();
    this._localService.getTiposLocal().subscribe(
      respuesta => {
        if (respuesta.Exito) {
          this.comboTipoLocal = respuesta.Data;
          /*if (pTipoLocal)
            this.comboTipoLocal = pTipoLocal;*/
        } else
          this._alertService.showWarning(respuesta.Mensaje);
      }, error => {
        this._alertService.showError(error.message);
      }, () => {
      }
    );
  }


  /**
   * Consume servicio para consultar costos de locales.
   */
  private getCostos(pCosto?: Costo): void {
    this.comboCostos = new Array<Costo>();
    this._localService.getCostos().subscribe(
      respuesta => {
        //console.log(respuesta);
        if (respuesta.Exito) {
          this.comboCostos = respuesta.Data;
          /*if (pCosto)
            this.comboCostos = pCosto;*/
        } else
          this._alertService.showWarning(respuesta.Mensaje);
      }, error => {
        this._alertService.showError(error.message);
      }, () => {

      });
  }

  //Operaciones para mapa
  private initialLoadMap() {
    this._mapsAPILoader.load().then(() => {
      this.setCurrentLocation();
      this.geoCoder = new google.maps.Geocoder;

      if (!this.isDisabled) {
        let autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement, {
          types: ["address"]
        });
        autocomplete.addListener("place_changed", () => {
          this._ngZone.run(() => {
            //get the place result
            let place: google.maps.places.PlaceResult = autocomplete.getPlace();

            //verify result
            if (place.geometry === undefined || place.geometry === null) {
              return;
            }

            //set latitude, longitude and zoom
            this.latitude = place.geometry.location.lat();
            this.longitude = place.geometry.location.lng();
            this.zoom = 12;
          });
        });
      }

    });
  }
  /**
   * Obtiene posicion actual para el mapa
   */
  private setCurrentLocation() {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition((position) => {
        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;
        this.zoom = 8;
        this.getAddress(this.latitude, this.longitude);
      });
    }
  }

  markerDragEnd($event: MouseEvent) {
    //console.log($event);
    this.latitude = $event.coords.lat;
    this.longitude = $event.coords.lng;
    this.getAddress(this.latitude, this.longitude);
  }

  getAddress(latitude, longitude) {
    this.geoCoder.geocode({ 'location': { lat: latitude, lng: longitude } }, (results, status) => {
      //console.log(results);
      //console.log(status);
      if (status === 'OK') {
        if (results[0]) {
          this.zoom = 12;
          this.address = results[0].formatted_address;
          this.form.controls['frmCalle'].setValue(results[0].address_components[1].long_name);
          this.form.controls['frmColonia'].setValue(results[0].address_components[2].long_name);
          this.form.controls['frmNoExt'].setValue(results[0].address_components[0].long_name);

        } else {
          window.alert('No se encontraron resultados');
        }
      } else {
        window.alert('Geocoder falló debido : ' + status);
      }
    });
  }
}
