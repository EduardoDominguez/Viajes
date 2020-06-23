import { Component, OnInit } from '@angular/core';
import { Sesion } from 'src/app/classes/Sesion';
import { StorageService } from 'src/app/core/services/storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public nombrePersona: string;

  constructor(
    private _sesionService : StorageService,
  ) {

  }

  ngOnInit() {
    this.nombrePersona = this._sesionService.getCurrentSession().Persona.Nombre;

  }

}
