angular.module('mainApp.services', [])
    .service('clientService', function ($http, $q) {

        this.getClients = function () {
            var deferred = $q.defer();
            $http.get('/Client/List')
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).error(function (data, status, headers, config) {
                    deferred.reject();
                });
            return deferred.promise;
        }

        this.findClient = function (clientId) {
            var deferred = $q.defer();
            $http.get('Client/GetClient/' + clientId).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });
            return deferred.promise;
        }
        this.createClient = function (client) {
            var deferred = $q.defer();
            $http.post('Client/CreateEdit', {
                Name: client.Name,
                Company: client.Company,
                Town: client.Town,
                Country: client.Country,
                AddressLine1: client.AddressLine1,
                AddressLine2: client.AddressLine2,
                Postcode: client.Postcode,
                DefaultCurrency: client.DefaultCurrency
            }).success(function (data, status, headers, config) {
                deferred.resolve();
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });
            return deferred.promise;
        }

        this.editeClient = function (client) {
            var deferred = $q.defer();
            $http.post('Client/CreateEdit', {
                ClientId: client.ClientId,
                Name: client.Name,
                Company: client.Company,
                Town: client.Town,
                Country: client.Country,
                AddressLine1: client.AddressLine1,
                AddressLine2: client.AddressLine2,
                Postcode: client.Postcode,
                DefaultCurrency: client.DefaultCurrency
            }).success(function (data, status, headers, config) {
                deferred.resolve();
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });
            return deferred.promise;
        }

        this.getBalances = function (clientId) {
            var deferred = $q.defer();
            $http.get('Client/Balance/' + clientId).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });
            return deferred.promise;
        }

        this.getInvoices = function (clientId) {
            var deferred = $q.defer();
            $http.get('Invoice/List/' + clientId).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });
            return deferred.promise;
        }

        this.getPayments = function (clientId) {
            var deferred = $q.defer();
            $http.get('Payment/List/' + clientId).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });
            return deferred.promise;
        }

        this.filterClients = function (filter) {
            var deferred = $q.defer();
            $http.post('/Client/List', {
                Find: filter.inputFind,
                InvoiceFrom: filter.inputInvoicedFrom,
                InvoiceTo: filter.inputInvoicedTo,
                TotalPaidFrom: filter.inputTotalPaidFrom,
                TotalPaidTo: filter.inputTotalPaidTo,
                BalanceFrom: filter.inputBalanceFrom,
                BalanceTo: filter.inputBalanceTo,
                DisplayCurrency: filter.inputCurrency
            }).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });
            return deferred.promise;
        }

        this.removeClient = function (clientId) {
            var deferred = $q.defer();
            $http.post('/Client/Delete', { id: clientId })
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).error(function (data, status, headers, config) {
                    deferred.reject();
                });
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
        this.validateInvoice = function (invoiceId) {
            var deferred = $q.defer();
            $http.post('Invoice/Validate', { id: invoiceId })
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).error(function (data, status, headers, config) {
                    deferred.reject();
                });
            return deferred.promise;
        }

        this.addinvoice = function(invoice) {
            var deferred = $q.defer();
            $http.post('Invoice/CreateEdit', {
                InvoiceId: null,
                Date: invoice.Date,
                ClientId: invoice.ClientId,
                ItemList: invoice.ItemList
                })
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).error(function (data, status, headers, config) {
                    deferred.reject();
                });
            return deferred.promise;
        }

        this.editinvoice = function (invoice) {
            var deferred = $q.defer();
            $http.post('Invoice/CreateEdit', {
                InvoiceId: invoice.InvoiceId,
                Date: invoice.Date,
                ClientId: invoice.ClientId,
                ItemList: invoice.ItemList
            })
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).error(function (data, status, headers, config) {
                    deferred.reject();
                });
            return deferred.promise;
        }

    })
    .service('paymentService', function ($http, $q) {
        this.getPayments = function () {
            var deferred = $q.defer();
            $http.get('Payment/GetAll').success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });

            return deferred.promise;
        }

        this.addPayment = function (payment) {
            var deferred = $q.defer();
            $http.post('Payment/CreateEdit', {
                PaymentDate: payment.paymentDate,
                Description: payment.description,
                Method: payment.method,
                Total: payment.total,
                InvoiceId: payment.invoiceId,
                PaymentId: null
            }).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });

            return deferred.promise;
        }

        this.editePayment = function (payment) {
            var deferred = $q.defer();
            $http.post('Payment/CreateEdit', {
                PaymentDate: payment.paymentDate,
                Description: payment.description,
                Method: payment.method,
                Total: payment.total,
                InvoiceId: payment.invoiceId,
                PaymentId: payment.paymentId
            }).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject();
            });

            return deferred.promise;
        }

        this.removePayment = function (paymentId) {
            var deferred = $q.defer();
            $http.post('Payment/Delete', { id: paymentId })
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                }).error(function (data, status, headers, config) {
                    deferred.reject();
                });

            return deferred.promise;
        }

    });