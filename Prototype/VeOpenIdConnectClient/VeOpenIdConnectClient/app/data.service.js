"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const http_1 = require("@angular/http");
require("rxjs/add/operator/toPromise");
require("rxjs/Rx");
let DataService = class DataService {
    constructor(http) {
        this.http = http;
        this.getIdToken = () => {
            this.http.get('http://localhost:3276/api/account/GetIdToken').subscribe(response => {
                if (response.status === 200) {
                    sessionStorage.setItem("token", JSON.stringify(response.json()));
                    window.location.reload();
                }
            });
        };
        this.login = () => {
            this.http.get('http://localhost:3276/api/account/login').subscribe(response => {
                if (response.status === 200 || response.status === 302) {
                    sessionStorage.setItem("session", JSON.stringify({ isAuthenticated: true }));
                    window.location.href = response.url;
                }
            });
        };
        this.logout = () => {
            this.http.get('http://localhost:3276/api/account/logout').subscribe(response => {
                if (response.status === 200 || response.status === 302) {
                    sessionStorage.clear();
                    window.location.href = response.url;
                }
            });
        };
        this.getContract = (contractId) => {
            this.http.get('http://localhost:3276/api/mortgage/GetContract?contractId=' + contractId).subscribe(response => {
                if (response.status === 200) {
                    sessionStorage.setItem("contract", JSON.stringify(response.json()));
                    window.location.reload();
                }
                else
                    alert(response.status);
            });
        };
    }
};
DataService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], DataService);
exports.DataService = DataService;
//# sourceMappingURL=data.service.js.map