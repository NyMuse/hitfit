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
const core_1 = require("@angular/core");
const person_service_1 = require("./person.service");
let AppComponent = class AppComponent extends core_1.OnInit {
    constructor(_service) {
        super();
        this._service = _service;
        this.persons = [];
    }
    ngOnInit() {
        this._service.loadData().then(data => {
            this.persons = data;
        });
    }
};
AppComponent = __decorate([
    core_1.Component({
        selector: 'my-app',
        template: `
    <h1>My First Angular 2 App</h1>
    <ul>
    <li *ngFor="let person of persons">
    <strong></strong><br>
    from: <br>
    date of birth: 
    </li>
    </ul>
    `,
        providers: [
            person_service_1.PersonService
        ]
    }),
    __metadata("design:paramtypes", [person_service_1.PersonService])
], AppComponent);
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map