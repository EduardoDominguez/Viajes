import { Injectable } from '@angular/core';
import { CanActivate, Route, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

import { StorageService } from './storage.service'
import { Observable } from 'rxjs';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private _storageService: StorageService, private _router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    let isloggedIn = this._storageService.isAuthenticated();
    // this._authService.logMessage('AuthGuardService->canActivate().router.url ' + this._router.url);
    // this._authService.logMessage('AuthGuardService->canActivate().authService.isLoggedIn() ' + isloggedIn);


    if (!isloggedIn) {
      this._router.navigate(['']);
      return false;
    }

    return isloggedIn;

    // if (!isloggedIn) {
    //   this._router.navigate(['']);
    //   return false;
    // }

    // //this._router.navigate(['/unauthorized']);
    // //this.authService.startAuthentication();
    // return true;
    // }, 500);
  }
}
