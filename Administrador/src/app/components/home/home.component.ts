import { Component, OnInit } from '@angular/core';
import { Sesion } from 'src/app/classes/Sesion';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {


  constructor(
    private _sesionService : StorageService,
  ) {
    
  }

  ngOnInit() {
  }

}
