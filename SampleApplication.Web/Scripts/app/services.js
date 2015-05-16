angular.module('mainApp.services', [])
    .service('clientService', function ($http, $q) {

        this.getClients = function () {
            var deferred = $q.defer();
            //$http.get({
            //    method: 'GET',
            //    url: '/api/clients',
            //}).success(function(data, status, headers, config) {
            //    deferred.resolve();
            //}).error(function(data, status, headers, config) {
            //    deferred.reject();
            //});
            deferred.resolve([{
                "Name": "Name One",
                "TotalInvoiced": 100,
                "TotalPaid": 50,
                "Balance": 50
            }, {
                "Name": "Name Two",
                "TotalInvoiced": 120,
                "TotalPaid": 20,
                "Balance": 100
            }, {
                "Name": "Name Three",
                "TotalInvoiced": 50,
                "TotalPaid": 20,
                "Balance": 30
            }]);
            return deferred.promise;
        }
    })
    .service('invoiceService', function ($http, $q) {
        this.getInvoices = function () {
            var deferred = $q.defer();
            //$http.get({
            //    method: 'GET',
            //    url: '/api/invoices',
            //}).success(function () {
            //    deferred.resolve();
            //}).error(function () {
            //    deferred.reject();
            //});

            deferred.resolve([{
                "Name": "Name One",
                "TotalInvoiced": 100,
                "TotalPaid": 50,
                "Balance": 50
            }, {
                "Name": "Name Two",
                "TotalInvoiced": 120,
                "TotalPaid": 20,
                "Balance": 100
            }, {
                "Name": "Name Three",
                "TotalInvoiced": 50,
                "TotalPaid": 20,
                "Balance": 30
            }]);
            return deferred.promise;
        }

    });