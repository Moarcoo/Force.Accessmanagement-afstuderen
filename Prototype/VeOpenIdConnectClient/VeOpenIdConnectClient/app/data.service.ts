import { Component, Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/Rx';

@Injectable()
export class DataService {
    claims: any;
    idToken: Object;
    headers: Headers;

    constructor(private http: Http) {
    }

    getIdToken = (): void => {
        this.http.get('http://localhost:3276/api/account/GetIdToken').subscribe(response => {
            if (response.status === 200) {
                sessionStorage.setItem("token", JSON.stringify(response.json()));
                window.location.reload();
            }
        });
    }

    login = (): void => {
        this.http.get('http://localhost:3276/api/account/login').subscribe(response => {
            if (response.status === 200 || response.status === 302) {
                sessionStorage.setItem("session", JSON.stringify({ isAuthenticated: true }));
                window.location.href = response.url; 
            }    
        });
    }

    logout = (): void => {
        this.http.get('http://localhost:3276/api/account/logout').subscribe(response => {
            if (response.status === 200 || response.status === 302) {
                sessionStorage.clear();
                window.location.href = response.url;   
            }
        });
    }

    getMortgageFileForUser = (dossierId: string): void => {
        this.http.get('http://localhost:3276/api/mortgage/GetMortgageFileForUser?dossierId=' + dossierId).subscribe(response => {
            if (response.status === 200) {
                sessionStorage.setItem("mortgageFile", JSON.stringify(response.json()));
                window.location.reload();
            }
            else alert(response.status);
        });
    }
}






