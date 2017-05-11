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
const data_service_1 = require("./data.service");
const http_1 = require("@angular/http");
let AppComponent = class AppComponent {
    constructor(dataService) {
        this.dataService = dataService;
        this.session = JSON.parse(sessionStorage.getItem("session"));
        this.token = JSON.parse(sessionStorage.getItem("token"));
        this.mortgageFile = JSON.parse(sessionStorage.getItem("mortgageFile"));
        this.getValueFromToken = (type) => {
            var result = "";
            if (!this.token)
                return result;
            this.token.forEach((obj) => {
                if (obj.type === type)
                    result = obj.value;
            });
            return result;
        };
        //if (!this.session) this.dataService.login();
    }
    ngOnInit() {
    }
    login() {
        this.dataService.login();
    }
    logout() {
        this.session.isAuthenticated = false;
        this.dataService.logout();
    }
    getMortgageFileForUser() {
        if (this.session != null) {
            var el = document.getElementById('dossierId');
            var dossierId = el.value;
            this.dataService.getMortgageFileForUser(dossierId);
        }
    }
    getIdToken() {
        if (this.session != null) {
            this.dataService.getIdToken();
        }
    }
};
AppComponent = __decorate([
    core_1.Component({
        selector: 'my-app',
        templateUrl: 'app.component.html',
        styleUrls: ['app.component.css'],
        moduleId: module.id,
        providers: [data_service_1.DataService, http_1.HttpModule]
    }),
    __metadata("design:paramtypes", [data_service_1.DataService])
], AppComponent);
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map