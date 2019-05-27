import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-querybuilder',
    templateUrl: './querybuilder.component.html'
})
export class QuerybuilderComponent implements OnInit {

    constructor() { }

    ngOnInit() {
        const qb = document.getElementById('qb');
        const treeview = document.getElementById('treeview');
        const navbar = document.getElementById('navbar');
        const canvas = document.getElementById('canvas');
        const statusbar = document.getElementById('statusbar');
        const grid = document.getElementById('grid');
        const sql = document.getElementById('sql');

        // Specify a handler for pre-processing of requests to Active Query Builder server handlers.
        AQB.Web.beforeSend = beforeSend;

        // Instance identifier string to bind to the QueryBiulder component on the server side.
        const instanceId = 'Angular_IoC';

        function beforeSend(xhr: any) {
            // Add token the request header to identify the client and find the right QueryBuilder instance on the server.
            xhr.setRequestHeader('query-builder-token', getToken());
        }

        function createQueryBuilder(onSuccess: () => void, onError: () => void) {
            checkToken(() => {
                createQbOnServer(onSuccess, onError);
            });
        }

        function checkToken(callback: () => void) {
            // Send a request to check for the token validity.
            $.ajax({
                type: 'GET',
                url: '/api/QueryBuilder/CheckToken?token=' + getToken(),
                success: (token: string) => {
                    // Save new token to the local storage to use in further requests.
                    if (token)
                        saveToken(token);
                    callback();
                }
            });
        }

        function createQbOnServer(onSuccess: () => void, onError: () => void) {
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

        function saveToken(token: string) {
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
    }
}
