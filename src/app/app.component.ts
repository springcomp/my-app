import { Component } from '@angular/core';
import { AuthService } from '../app/core/auth.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'my-app';

  constructor(
    public authService: AuthService
  ){
  }

  async ngOnInit() {
    await this.authService.ngOnInit();
  }
}
