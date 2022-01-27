var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
var QuerybuilderComponent = /** @class */ (function () {
    function QuerybuilderComponent() {
    }
    QuerybuilderComponent.prototype.ngOnInit = function () {
        var qb = document.getElementById('qb');
        var treeview = document.getElementById('treeview');
        var navbar = document.getElementById('navbar');
        var canvas = document.getElementById('canvas');
        var statusbar = document.getElementById('statusbar');
        var grid = document.getElementById('grid');
        var sql = document.getElementById('sql');
        // Specify a handler for pre-processing of requests to Active Query Builder server handlers.
        AQB.Web.beforeSend = beforeSend;
        // Instance identifier string to bind to the QueryBiulder component on the server side.
        var instanceId = 'Angular_IoC';
        function beforeSend(xhr) {
            // Add token the request header to identify the client and find the right QueryBuilder instance on the server.
            xhr.setRequestHeader('query-builder-token', getToken());
        }
        function createQueryBuilder(onSuccess, onError) {
            checkToken(function () {
                createQbOnServer(onSuccess, onError);
            });
        }
        function checkToken(callback) {
            // Send a request to check for the token validity.
            $.ajax({
                type: 'GET',
                url: '/api/QueryBuilder/CheckToken?token=' + getToken(),
                success: function (token) {
                    // Save new token to the local storage to use in further requests.
                    if (token)
                        saveToken(token);
                    callback();
                }
            });
        }
        function createQbOnServer(onSuccess, onError) {
            $.ajax({
                url: '/api/QueryBuilder/CreateQueryBuilder',
                data: { name: instanceId },
                beforeSend: beforeSend,
                success: onSuccess,
                error: onError
            });
        }
        function getToken() {
            return localStorage.getItem('queryBuilderToken');
        }
        function saveToken(token) {
            localStorage.setItem('queryBuilderToken', token);
        }
        AQB.Web.UI.QueryBuilder(instanceId, qb, { theme: 'jqueryui' });
        AQB.Web.UI.ObjectTreeView(instanceId, treeview);
        AQB.Web.UI.SubQueryNavigationBar(instanceId, navbar);
        AQB.Web.UI.Canvas(instanceId, canvas);
        AQB.Web.UI.StatusBar(instanceId, statusbar);
        AQB.Web.UI.Grid(instanceId, grid, { orColumnCount: 0 });
        AQB.Web.UI.SqlEditor(instanceId, sql);
        AQB.Web.UI.startApplication(instanceId, createQueryBuilder);
    };
    QuerybuilderComponent = __decorate([
        Component({
            selector: 'app-querybuilder',
            templateUrl: './querybuilder.component.html'
        }),
        __metadata("design:paramtypes", [])
    ], QuerybuilderComponent);
    return QuerybuilderComponent;
}());
export { QuerybuilderComponent };
//# sourceMappingURL=querybuilder.component.js.map