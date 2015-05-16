angular.module('mainApp.controllers', [])
    .controller('clientController', function ($scope, clientService) {

        $scope.currencies = ['GBP', 'USD', 'EURO'];
        $scope.inputCurrency = 'GBP';
        $scope.clientsData = null;

        clientService.getClients().then(function (data) {
            $scope.clientsData = data;
        });
    })
    .controller('invoiceController', function ($scope, invoiceService) {
        invoiceService.getInvoices().then(function (data) {
            $scope.invoicesData = data;
        });
    })
    .controller('paymentController', function ($scope) { })