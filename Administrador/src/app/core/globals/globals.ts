import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { StorageService } from "../services/storage.service";
import { TooltipPosition } from '@angular/material';

@Injectable()
export class globals {

  /**Arreglo con nombres de meses */
  NOMBRES_MESES = new Array();
  /**Configuraciones del sistema que no entran en enviroments */
  SYSTEM = {
    NAME: "Administrador FastRun",
    SHORT_NAME: "FastRun"
  };


  HTTP_OPTIONS = {
    //headers: new HttpHeaders({'Content-Type': 'application/json'}),
    params: null
  };

  /**Posiciones posibles para el Tooltip Angular */
  POSITION_OPTIONS: TooltipPosition[] = ['after', 'before', 'above', 'below', 'left', 'right'];
  
  constructor(
    private storageService: StorageService,
  ) {
    this.NOMBRES_MESES[0] = "Enero";
    this.NOMBRES_MESES[1] = "Febrero";
    this.NOMBRES_MESES[2] = "Marzo";
    this.NOMBRES_MESES[3] = "Abril";
    this.NOMBRES_MESES[4] = "Mayo";
    this.NOMBRES_MESES[5] = "Junio";
    this.NOMBRES_MESES[6] = "Julio";
    this.NOMBRES_MESES[7] = "Agosto";
    this.NOMBRES_MESES[8] = "Septiembre";
    this.NOMBRES_MESES[9] = "Octubre";
    this.NOMBRES_MESES[10] = "Noviembre";
    this.NOMBRES_MESES[11] = "Deciembre";
  }
}