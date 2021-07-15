import { Injectable } from '@angular/core';
import { Router, Route, CanActivate, CanLoad, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthorizationGuard implements CanActivate, CanLoad {
  constructor(
    private router: Router,
    private authService: AuthService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.checkUser(route.url[0].path);
  }

  canLoad(state: Route): boolean {
    return this.checkUser();
  }

  private checkUser(route?: string): boolean {
    console.log('AuthorizationGuard::checkUser()')
    console.log(route);
    const isAuthorized = this.authService.getIsAuthorized();
    console.log(isAuthorized);
    if (!isAuthorized) {
      console.log('navigate...');
      var redirect: string = '/.auth/login/github'
      if (route != null) {
        redirect += `?post_login_redirect_uri=/${route}`;
      }
      console.log(redirect);
      this.authService.navigate(redirect);
      console.log('navigated...');
      return false;
    }

    return true;
  }
}