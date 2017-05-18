import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataService } from './data.service';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';

@Component({
    selector: 'my-app',
    templateUrl: 'app.component.html',
    styleUrls: [ 'app.component.css'],
    moduleId: module.id,
    providers: [DataService, HttpModule]
})

export class AppComponent implements  OnInit {
    session = JSON.parse(sessionStorage.getItem("session"));
    token = JSON.parse(sessionStorage.getItem("token"));
    contract = JSON.parse(sessionStorage.getItem("contract"));

    constructor(private dataService: DataService) {
        //if (!this.session) this.dataService.login();
    }

    ngOnInit(): void {
    }

    login(): void {
        this.dataService.login();
    }

    logout(): void {
        this.session.isAuthenticated = false;
        this.dataService.logout();
    }

    getContract() {
        if (this.session != null) {
//            var el: HTMLInputElement = <HTMLInputElement>document.getElementById('dossierId');
//            var dossierId = el.value;
            var contractId = "010101";
            this.dataService.getContract(contractId);
        }
    }

    getIdToken() {
        if (this.session != null) {
            this.dataService.getIdToken();

        }
    }

    getValueFromToken = (type: string): string => {
        var result = "";
        if (!this.token) return result;
        this.token.forEach((obj: any) => {
            if (obj.type === type) result = obj.value;
        });
        return result;
    }
}
