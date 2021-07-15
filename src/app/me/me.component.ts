import { Component, OnInit } from '@angular/core';
import { UserInfo } from '../../models/user-info';

@Component({
  selector: 'app-me',
  templateUrl: './me.component.html',
  styleUrls: ['./me.component.sass']
})
export class MeComponent implements OnInit {

  redirect: string = window.location.pathname;
  userInfo: UserInfo | undefined = undefined;

  async ngOnInit() {
    this.userInfo = await this.getUserInfo();
  }

  async getUserInfo() {
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
