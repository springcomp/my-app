import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor() { }

  public async getMessage(): Promise<string> {
    const response = await fetch("/api/message");
    console.log(response);
    const payload = await response.json();
    return JSON.stringify(payload);
    //const { message } = payload;
    //return message;
  }
}
