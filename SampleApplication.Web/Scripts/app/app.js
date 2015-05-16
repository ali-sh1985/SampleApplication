var app = angular.module('mainApp', ['ngRoute', 'ui.grid','LocalStorageModule', 'mainApp.controllers', 'mainApp.services']);

app.config(function($routeProvider) {
    $routeProvider
        // route for the Clients page
        .when('/clients', {
            templateUrl: 'Client',
            controller: 'clientController'
        })
        // route for the Invoices page
        .when('/invoices', {
            templateUrl: 'Invoice',
            controller: 'invoiceController'
        })
        // route for the Payments page
        .when('/payments', {
            templateUrl: 'Templates/payments.html',
            controller: 'paymentController'
        })
        .otherwise({
            redirectTo: '/clients'
        });
});