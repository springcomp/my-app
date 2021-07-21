import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor() { }

  public async getMessage(): Promise<string> {
    const response = await fetch("/api/message");
    const payload = await response.json();
    const { message } = payload;
    return message;

    //return new Promise((resolve, reject) => {
    //  resolve("Hello, world!");
    //  reject("");
    //});
  }
}
