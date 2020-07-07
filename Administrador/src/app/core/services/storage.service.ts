import { Injectable } from "@angular/core";
import { Router, ActivatedRoute } from '@angular/router';
import { Sesion } from "../../classes/Sesion";
import { User } from "../../classes/User";

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  private localStorageService;
  private currentSession: Sesion = null;

  constructor(private router: Router,
    private route: ActivatedRoute) {
    //this.localStorageService = localStorage;
    this.localStorageService = sessionStorage;

    this.currentSession = this.loadSessionData();
  }
  setCurrentSession(session: Sesion): void {
    this.currentSession = session;
    this.localStorageService.setItem('currentUser', JSON.stringify(session));
  }
  loadSessionData(): Sesion {
    var sessionStr = this.localStorageService.getItem('currentUser');
    return (sessionStr) ? <Sesion>JSON.parse(sessionStr) : null;
  }
  getCurrentSession(): Sesion {
    return this.currentSession;
  }
  removeCurrentSession(): void {
    this.localStorageService.removeItem('currentUser');
    this.currentSession = null;
  }
  getCurrentUser(): User {
    var session: Sesion = this.getCurrentSession();
    return (session && session.Persona) ? session.Persona : null;
  };
  isAuthenticated(): boolean {
    return (this.getCurrentToken() != null) ? true : false;
  };
  getCurrentToken(): string {
    var session = this.getCurrentSession();
    return (session && session.Token) ? session.Token : null;
  };
  logout(): void {
    this.removeCurrentSession();
    //this.router.navigate(['../../Login']);
    this.router.navigate(['/'], { relativeTo: this.route });
  };
}
