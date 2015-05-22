var app = angular.module('mainApp', ['ngRoute', 'ui.bootstrap', 'smart-table', 'highcharts-ng', 'LocalStorageModule', 'mainApp.controllers', 'mainApp.services', 'mainApp.directives', 'mainApp.filters']);

app.config(function($routeProvider) {
    $routeProvider
        // route for the Clients page
        .when('/clients', {
            templateUrl: 'Client',
            controller: 'clientController'
        })
        .when('/clients/create/', {
            templateUrl: 'Client/CreateEdit',
            controller: 'clientCreateEditController'
        })
        .when('/clients/edit/:id', {
            templateUrl: function (params) { return 'Client/CreateEdit/' + params.id; },
            controller: 'clientCreateEditController'
        })
        .when('/clients/detail/:id', {
            templateUrl: function (params) { return 'Client/Detail/' + params.id; },
            controller: 'clientDetailController'
        })
        // route for the Invoices page
        .when('/invoices', {
            templateUrl: 'Invoice',
            controller: 'invoiceController'
        })
        // route for the Payments page
        .when('/payments', {
            templateUrl: 'Payment',
            controller: 'paymentController'
        })
        // route for the Payments page
        .when('/payments/create', {
            templateUrl: 'Payment/Create',
            controller: 'paymentController'
        })
        // route for the Payments page
        .when('/reports', {
            templateUrl: 'Report',
            controller: 'reportController'
        })
        .otherwise({
            redirectTo: '/clients'
        });
});