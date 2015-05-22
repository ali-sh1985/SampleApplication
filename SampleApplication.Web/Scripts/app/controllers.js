angular.module('mainApp.controllers', [])
    .controller('clientController', function ($scope, $modal, clientService) {

        $scope.currencies = ['GBP', 'USD', 'EURO'];
        $scope.inputCurrency = 'GBP';

        $scope.clientCollection = null;

        clientService.getClients().then(function (data) {
            $scope.clientCollection = data;
        });

        $scope.filterClients = function () {
            clientService.filterClients($scope.filter).then(function (data) {
                $scope.clientCollection = data;
            });
        }

        $scope.resetForm = function () {
            $scope.filter = {
                inputFind: null,
                inputInvoicedFrom: null,
                inputInvoicedTo: null,
                inputTotalPaidFrom: null,
                inputTotalPaidTo: null,
                inputBalanceFrom: null,
                inputBalanceTo: null,
                inputCurrency: null,
            }
        }
        $scope.resetForm();
        $scope.removeClient = function (client) {
            var modalInstance = $modal.open({
                animation: true,
                controller: 'clientDeleteModalController',
                templateUrl: 'deleteModalContent.html',
                size: 'sm',
                resolve: {
                    client: function () {
                        return client;
                    }
                }
            });

            modalInstance.result.then(function (clientId) {
                clientService.removeClient(clientId);
                $scope.filterClients();
            }, function () {

            });
        }
    })
    .controller('clientDeleteModalController', function ($scope, $modalInstance, client) {
        $scope.client = client;
        $scope.ok = function () {
            $modalInstance.close($scope.client.clientId);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss(false);
        };
    })
    .controller('clientCreateEditController', function ($scope, $routeParams, $location, clientService) {
        if ($routeParams.id) {
            clientService.findClient($routeParams.id).then(function (data) {
                $scope.client = data;
            }, function (err) {
                
            });
        }
        $scope.cancel = function () {
            $location.path("/clients");
        }

        $scope.save = function () {
            $scope.$broadcast('show-errors-check-validity');
            if ($scope.clientForm.$valid) {
                if ($routeParams.id) {
                    clientService.editeClient($scope.client).then(function (data) {
                        $location.path("/clients");
                    }, function (err) {

                    });
                } else {
                    clientService.createClient($scope.client).then(function (data) {
                        $location.path("/clients");
                    }, function (err) {

                    });
                }
                
            }
        }
    })
    .controller('clientDetailController', function ($scope, $routeParams, $location, $modal, clientService, paymentService) {
        $scope.balanceCollection = null;
        $scope.getBalances = function () {
            clientService.getBalances($routeParams.id).then(function (data) {
                $scope.balanceCollection = data;
            });
        }
        $scope.getBalances();

        $scope.invoiceCollection = null;
        $scope.getInvoices = function () {
            clientService.getInvoices($routeParams.id).then(function (data) {
                $scope.invoiceCollection = data;
            });
        }

        $scope.paymentCollection = null;
        $scope.getPayments = function () {
            clientService.getPayments($routeParams.id).then(function (data) {
                $scope.paymentCollection = data;
            });
        }

        $scope.openInvoiceForm = function (invoice) {
            var client = clientService.findClient($routeParams.id);
            var modalInstance = $modal.open({
                animation: true,
                controller: 'invoiceModalController',
                templateUrl: 'addInvoiceModalContent.html',
                size: 'lg',
                resolve: {
                    client: function () {
                        return client;
                    },
                    invoice: function () {
                        return invoice;
                    }
                }
            });

            modalInstance.result.then(function (invoice) {
                $scope.getBalances();
                $scope.getInvoices();
            }, function () {

            });
        }

        $scope.openPaymentForm = function (payment) {
            var client = clientService.findClient($routeParams.id);
            var modalInstance = $modal.open({
                animation: true,
                controller: 'paymentModalController',
                templateUrl: 'addPaymentModalContent.html',
                resolve: {
                    client: function () {
                        return client;
                    },
                    payment: function () {
                        return payment;
                    }
                }
            });

            modalInstance.result.then(function (payment) {
                $scope.getBalances();
                $scope.getPayments();
            }, function () {

            });
        }
        $scope.removePayment = function (payment) {
            var modalInstance = $modal.open({
                animation: true,
                controller: 'paymentDeleteModalController',
                templateUrl: 'deletePaymentModalContent.html',
                size: 'sm',
                resolve: {
                    payment: function () {
                        return payment;
                    }
                }
            });

            modalInstance.result.then(function (paymentId) {
                paymentService.removePayment(paymentId).then(function (data) {
                    $scope.getPayments();
                });
            }, function () {

            });
        }

    })
    .controller('invoiceController', function ($scope, invoiceService) {
        invoiceService.getInvoices().then(function (data) {
            $scope.invoicesData = data;
        });
    })
    .controller('paymentController', function ($scope, paymentService) {
        $scope.paymentCollection = null;
        paymentService.getPayments().then(function (data) {
            $scope.paymentCollection = data;
        });
    })
    .controller('invoiceModalController', function ($scope, $modalInstance, $filter, client, invoice, invoiceService) {
        $scope.client = client;
        $scope.invoice = {
            InvoiceId: null,
            ClientId: client.ClientId,
            Date: null,
            ItemList: []
        }

        if (invoice) {
            $scope.isEditMode = true;
            angular.copy($scope.invoice, invoice);
            $scope.invoice.invoiceDate = $filter('jsonDate')($scope.invoice.invoiceDate);
        }

        $scope.today = function () {
            $scope.dt = new Date();
        };

        $scope.openDatePicker = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.addItem = function() {
            $scope.invoice.ItemList.push({
                ItemId: null,
                Description: null,
                Net: null,
                Tax: null,
                InvoiceId: $scope.invoice.InvoiceId
            });
        }

        $scope.removeItem = function (item) {
            var index = $scope.invoice.ItemList.indexOf(item);
            $scope.invoice.ItemList.splice(index, 1);
        }

        $scope.save = function () {
            $scope.$broadcast('show-errors-check-validity');
            if ($scope.invoiceForm.$valid) {
                if ($scope.isEditMode) {
                    invoiceService.editeInvoice($scope.invoice).then(function (data) {
                        $modalInstance.close($scope.invoice);
                    });
                } else {
                    invoiceService.addinvoice($scope.invoice).then(function (data) {
                        $modalInstance.close($scope.invoice);
                    });
                }

            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss(false);
        };
    })
    .controller('paymentModalController', function ($scope, $modalInstance, $filter, client, payment, paymentService) {
        $scope.client = client;
        $scope.payment = {
            paymentDate: null,
            description: null,
            method: null,
            total: null
        }

        if (payment) {
            $scope.isEditMode = true;
            angular.extend($scope.payment, payment);
            $scope.payment.paymentDate = $filter('jsonDate')($scope.payment.paymentDate);
        }

        $scope.today = function () {
            $scope.dt = new Date();
        };

        $scope.openDatePicker = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.save = function () {
            $scope.$broadcast('show-errors-check-validity');
            if ($scope.paymentForm.$valid) {
                if ($scope.isEditMode) {
                    paymentService.editePayment($scope.payment).then(function (data) {
                        $modalInstance.close($scope.payment);
                    });
                } else {
                    paymentService.addPayment($scope.payment).then(function (data) {
                        $modalInstance.close($scope.payment);
                    });
                }

            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss(false);
        };
    })
    .controller('paymentDeleteModalController', function ($scope, $modalInstance, payment) {
        $scope.payment = payment;
        $scope.ok = function () {
            $modalInstance.close($scope.payment.paymentId);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss(false);
        };
    })
    .controller('reportController', function ($scope) {
        $scope.chartConfig = {
            options: {
                chart: {
                    type: 'line',
                    zoomType: 'x'
                }
            },
            series: [{
                data: [10, 15, 12, 8, 7, 1, 1, 19, 15, 10]
            }],
            title: {
                text: 'Hello'
            },
            xAxis: { currentMin: 0, currentMax: 10, minRange: 1 },
            loading: false
        };
    })