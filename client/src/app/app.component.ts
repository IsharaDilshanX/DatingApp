import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'The Dating App'; 
  users: any; //users could be any type

  constructor(private accountService: AccountService){}

  //this life cycle even must always come after the constructor
  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser(){ //bc of this method user can refresh/restart the page/browser since we can verify username n the token from the localStorage!! WOW!
    const user: User = JSON.parse(localStorage.getItem('user')); 
    this.accountService.setCurrentUser(user);
  }
}
