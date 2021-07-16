import { Injectable } from '@angular/core';
import { Router, Route, CanActivate, CanLoad, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthorizationGuard implements CanActivate, CanLoad {
  constructor(
    private router: Router,
    private authService: AuthService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.checkUser(route.url[0].path);
  }

  canLoad(state: Route): Observable<boolean> {
    return this.checkUser();
  }

  private checkUser(route?: string): Observable<boolean> {
    var promise = this.authService.ngOnInit();
    const observable = from(promise);
    return observable.pipe(map(() => {
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
    }));
  }
}