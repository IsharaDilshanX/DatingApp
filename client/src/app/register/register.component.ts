import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { error } from 'protractor';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {  
  @Output() cancelRegister = new EventEmitter();  //Auto import from '@angular/core'

  model: any ={};

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void { 
  }

  register(){
    this.accountService.register(this.model).subscribe(Response => {
      console.log(Response);
      this.cancel();
    }, error => {
      console.log(error)
      this.toastr.error(error.error);
    })
  }

  cancel(){
    this.cancelRegister.emit(false); //we can pass what ever in this emit methord
  }
}
