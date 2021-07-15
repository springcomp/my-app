import { Injectable, OnInit, OnDestroy } from '@angular/core';
import { UserInfo } from '../models/userInfo';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnInit, OnDestroy {

  public clientPrincipal: UserInfo | undefined;

  constructor() { }

  async ngOnInit(){
    console.log('Auth.Service::ngOnInit()')
    this.clientPrincipal = await this.getClientPrincipal();
  }

  ngOnDestroy(): void {
    console.log('Auth.Service::ngOnDestroy()')
  }

  public getIsAuthenticated(): boolean {
    return this.getIsInRole('authenticated');
  }
  public getIsRegistered(): boolean {
    return this.getIsInRole('registered');
  }
  public getIsInRole(roleName: string): boolean {
    return (this.clientPrincipal != undefined && this.clientPrincipal.userRoles.indexOf(roleName) != -1); 
  }

  private async getClientPrincipal() {
    try {
      const response = await fetch('/.auth/me');
      const payload = await response.json();
      const { clientPrincipal } = payload;
      return clientPrincipal;
    } catch (error) {
      console.error('No profile could be found');
      return undefined;
    }
  }
}
