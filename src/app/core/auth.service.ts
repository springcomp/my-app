import { DOCUMENT } from '@angular/common';
import { Injectable, OnInit, OnDestroy, Inject } from '@angular/core';
import { resolve } from 'dns';
import { UserInfo } from '../models/userInfo';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnInit, OnDestroy {

  public clientPrincipal: UserInfo | undefined;

  constructor(
    @Inject(DOCUMENT) private document: Document
  ) { }

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
  public getIsAuthorized(): boolean {
    return this.getIsInRole('registered');
  }
  public getIsInRole(roleName: string): boolean {
    return (this.clientPrincipal != undefined && this.clientPrincipal.userRoles.indexOf(roleName) != -1); 
  }

  public navigate(url: string): void {
    this.document.location.href = url;
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
