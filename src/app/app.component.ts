import { Component } from '@angular/core';
import { AuthService } from '../app/core/auth.service'
import { MessageService } from './services/message.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'my-app';

  public message: string | null = null;

  constructor(
    public authService: AuthService,
    public messageService: MessageService
  ){
  }

  async ngOnInit() {
    await this.authService.ngOnInit();
    this.message = await this.messageService.getMessage();
  }
}
