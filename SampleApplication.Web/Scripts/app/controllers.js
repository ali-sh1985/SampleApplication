angular.module('mainApp.controllers', [])
    .controller('mainController', function($scope, Page) {
        $scope.Page = Page;
    })
    .controller('clientController', function($scope, $modal, clientService, Page) {
        Page.setTitle('CLIENTS');
        $scope.totalItems = 0;
        $scope.currentPage = 1;
        $scope.pageSize = 10;

        $scope.clientCollection = null;

        $scope.filterClients = function(tableState) {
            $scope.isLoading = true;
            if (tableState) {
                $scope.filter.sortColumn = tableState.sort.predicate;
                $scope.filter.order = tableState.sort.reverse ? 'DESC' : 'ASC';
            }
            $scope.filter.pageNumber = $scope.currentPage;
            $scope.filter.pageSize = $scope.pageSize;
            clientService.filterClients($scope.filter).then(function(data) {
                $scope.clientCollection = data.Data;
                $scope.totalItems = data.TotalItems;
                $scope.pageNumber = data.PageNumber;
                $scope.numPages = data.TotalPages;
                $scope.isLoading = false;

            });
        }

        $scope.resetForm = function() {
            $scope.filter = {
                inputFind: null,
                inputInvoicedFrom: null,
                inputInvoicedTo: null,
                inputTotalPaidFrom: null,
                inputTotalPaidTo: null,
                inputBalanceFrom: null,
                inputBalanceTo: null,
                inputCurrency: 1
            }
        }
        $scope.changePageSize = function(size) {
            $scope.pageSize = size;
            $scope.filterClients();
        };
        $scope.resetForm();
        $scope.filter.inputCurrency = 1;
        $scope.removeClient = function(client) {
            var modalInstance = $modal.open({
                animation: true,
                controller: 'clientDeleteModalController',
                templateUrl: 'deleteModalContent.html',
                size: 'sm',
                resolve: {
                    client: function() {
                        return client;
                    }
                }
            });

            modalInstance.result.then(function(clientId) {
                clientService.removeClient(clientId).then(function(data) {
                    $scope.filterClients();
                });

            }, function() {

            });
        }
    })
    .controller('clientDeleteModalController', function($scope, $modalInstance, client) {
        $scope.client = client;
        $scope.ok = function() {
            $modalInstance.close($scope.client.clientId);
        };

        $scope.cancel = function() {
            $modalInstance.dismiss(false);
        };
    })
    .controller('clientCreateEditController', function($scope, $routeParams, $location, clientService, Page) {
        Page.setTitle('Create/Edit Client');
        if ($routeParams.id) {
            clientService.findClient($routeParams.id).then(function(data) {
                $scope.client = data;
            }, function(err) {

            });
        }
        $scope.cancel = function() {
            $location.path("/clients");
        }

        $scope.save = function() {
            $scope.$broadcast('show-errors-check-validity');
            if ($scope.clientForm.$valid) {
                if ($routeParams.id) {
                    clientService.editeClient($scope.client).then(function(data) {
                        $location.path("/clients");
                    }, function(err) {

                    });
                } else {
                    clientService.createClient($scope.client).then(function(data) {
                        $location.path("/clients");
                    }, function(err) {

                    });
                }

            }
        }
    })
    .controller('clientDetailController', function($scope, $routeParams, $location, $modal, clientService, invoiceService, paymentService) {
        $scope.balanceCollection = null;
        $scope.getBalances = function() {
            clientService.getBalances($routeParams.id).then(function(data) {
                $scope.balanceCollection = data.BalanceSheet;
                $scope.TotalInvoiced = data.TotalInvoiced;
                $scope.TotalPaid = data.TotalPaid;
                $scope.Balance = data.Balance;
            });
        }
        $scope.getBalances();

        $scope.invoiceCollection = null;
        $scope.getInvoices = function() {
            clientService.getInvoices($routeParams.id).then(function(data) {
                $scope.invoiceCollection = data;
            });
        }

        $scope.paymentCollection = null;
        $scope.getPayments = function() {
            clientService.getPayments($routeParams.id).then(function(data) {
                $scope.paymentCollection = data;
            });
        }

        $scope.openInvoiceForm = function(invoice) {
            var client = clientService.findClient($routeParams.id);
            var modalInstance = $modal.open({
                animation: true,
                controller: 'invoiceModalController',
                templateUrl: 'addInvoiceModalContent.html',
                size: 'lg',
                resolve: {
                    client: function() {
                        return client;
                    },
                    invoice: function() {
                        return invoice;
                    }
                }
            });

            modalInstance.result.then(function(invoice) {
                $scope.getBalances();
                $scope.getInvoices();
            }, function() {

            });
        }
        $scope.removeInvoice = function(invoice) {
            var modalInstance = $modal.open({
                animation: true,
                controller: 'invoiceDeleteModalController',
                templateUrl: 'deleteInvoiceModalContent.html',
                size: 'sm',
                resolve: {
                    invoice: function() {
                        return invoice;
                    }
                }
            });

            modalInstance.result.then(function(invoiceId) {
                invoiceService.removeInvoice(invoiceId).then(function(data) {
                    $scope.getInvoices();
                });
            }, function() {

            });
        }

        $scope.openPaymentForm = function(payment) {
            var client = clientService.findClient($routeParams.id);
            var modalInstance = $modal.open({
                animation: true,
                controller: 'paymentModalController',
                templateUrl: 'addPaymentModalContent.html',
                resolve: {
                    client: function() {
                        return client;
                    },
                    payment: function() {
                        return payment;
                    }
                }
            });

            modalInstance.result.then(function(payment) {
                $scope.getBalances();
                $scope.getPayments();
            }, function() {

            });
        }
        $scope.removePayment = function(payment) {
            var modalInstance = $modal.open({
                animation: true,
                controller: 'paymentDeleteModalController',
                templateUrl: 'deletePaymentModalContent.html',
                size: 'sm',
                resolve: {
                    payment: function() {
                        return payment;
                    }
                }
            });

            modalInstance.result.then(function(payment) {
                paymentService.removePayment(payment.paymentId).then(function(data) {
                    $scope.getPayments();
                });
            }, function() {

            });
        }

    })
    .controller('invoiceController', function($scope, $modal, invoiceService, clientService, Page) {
        Page.setTitle('INVOICES');
        $scope.totalItems = 0;
        $scope.currentPage = 1;
        $scope.pageSize = 10;
        $scope.invoiceCollection = null;
        $scope.isLoadingClient = false;

        $scope.filterInvoices = function(tableState) {
            $scope.isLoading = true;
            if (tableState) {
                $scope.filter.sortColumn = tableState.sort.predicate;
                $scope.filter.order = tableState.sort.reverse ? 'DESC' : 'ASC';
            }
            $scope.filter.pageNumber = $scope.currentPage;
            $scope.filter.pageSize = $scope.pageSize;
            invoiceService.filterInvoices($scope.filter).then(function(data) {
                $scope.invoiceCollection = data.Data;
                $scope.totalItems = data.TotalItems;
                $scope.pageNumber = data.PageNumber;
                $scope.numPages = data.TotalPages;
                $scope.isLoading = false;
            });
        }
        $scope.resetForm = function() {
            $scope.filter = {
                inputFind: null,
                inputInvoice: null,
                inputClientName: null,
                inputClientId: null,
                inputTotalFrom: null,
                inputTotalTo: null,
                inputNetFrom: null,
                inputNetTo: null
            }
        }

        $scope.resetForm();

        $scope.openInvoiceForm = function(invoice) {
            var client = clientService.findClient(invoice.ClientId);
            var modalInstance = $modal.open({
                animation: true,
                controller: 'invoiceModalController',
                templateUrl: 'addInvoiceModalContent.html',
                size: 'lg',
                resolve: {
                    client: function() {
                        return client;
                    },
                    invoice: function() {
                        return invoice;
                    }
                }
            });

            modalInstance.result.then(function(invoice) {
                $scope.filterInvoices();
            }, function() {

            });
        }
        $scope.removeInvoice = function(invoice) {
            var modalInstance = $modal.open({
                animation: true,
                controller: 'invoiceDeleteModalController',
                templateUrl: 'deleteModalContent.html',
                size: 'sm',
                resolve: {
                    invoice: function() {
                        return invoice;
                    }
                }
            });

            modalInstance.result.then(function(invoiceId) {
                invoiceService.removeInvoice(invoiceId).then(function(data) {
                    $scope.filterInvoices();
                });

            }, function() {

            });
        }

        $scope.getClients = function(val) {
            $scope.isLoadingClient = true;
            return clientService.filterClients({
                inputFind: val
            }).then(function(data) {
                var clients = [];
                angular.forEach(data.Data, function(item) {
                    clients.push({
                        clientId: item.clientId,
                        name: item.name
                    });
                });
                $scope.isLoadingClient = false;
                return clients;
            });
        }
        $scope.setClientId = function(client) {
            $scope.filter.inputClientId = client.clientId;
        }

        $scope.changePageSize = function(size) {
            $scope.pageSize = size;
            $scope.filterInvoices();
        };

        $scope.$watch('filter.inputClientName', function(val) {
            if (!val) {
                $scope.filter.inputClientId = null;
            }
        });
    })
    .controller('paymentController', function($scope, $modal, paymentService, clientService, Page) {
        Page.setTitle('PAYMENTS');
        $scope.totalItems = 0;
        $scope.currentPage = 1;
        $scope.pageSize = 10;
        $scope.paymentCollection = null;
        $scope.isLoadingClient = false;

        $scope.filterPayments = function(tableState) {
            $scope.isLoading = true;
            if (tableState) {
                $scope.filter.sortColumn = tableState.sort.predicate;
                $scope.filter.order = tableState.sort.reverse ? 'DESC' : 'ASC';
            }
            $scope.filter.pageNumber = $scope.currentPage;
            $scope.filter.pageSize = $scope.pageSize;
            paymentService.filterPayments($scope.filter).then(function(data) {
                $scope.paymentCollection = data.Data;
                $scope.totalItems = data.TotalItems;
                $scope.pageNumber = data.PageNumber;
                $scope.numPages = data.TotalPages;
                $scope.isLoading = false;
            });
        }

        $scope.resetForm = function() {
            $scope.filter = {
                inputClientName: null,
                inputClientId: null
            }
        }
        $scope.resetForm();

        $scope.openPaymentForm = function(payment) {
            var modalInstance = $modal.open({
                animation: true,
                controller: 'paymentModalController',
                templateUrl: 'addPaymentModalContent.html',
                resolve: {
                    payment: function() {
                        return payment;
                    }
                }
            });

            modalInstance.result.then(function(payment) {
                $scope.filterPayments();
            }, function() {

            });
        }
        $scope.removePayment = function(payment) {
            var modalInstance = $modal.open({
                animation: true,
                controller: 'paymentDeleteModalController',
                templateUrl: 'deleteModalContent.html',
                size: 'sm',
                resolve: {
                    payment: function() {
                        return payment;
                    }
                }
            });

            modalInstance.result.then(function(paymentId) {
                paymentService.removePayment(payment.PaymentId).then(function(data) {
                    $scope.filterPayments();
                });

            }, function() {

            });
        }

        $scope.getClients = function(val) {
            $scope.isLoadingClient = true;
            return clientService.filterClients({
                inputFind: val
            }).then(function(data) {
                var clients = [];
                angular.forEach(data.Data, function(item) {
                    clients.push({
                        clientId: item.clientId,
                        name: item.name
                    });
                });
                $scope.isLoadingClient = false;
                return clients;
            });
        }

        $scope.setClientId = function(client) {
            $scope.filter.inputClientId = client.clientId;
        }

        $scope.changePageSize = function(size) {
            $scope.pageSize = size;
            $scope.filterPayments();
        };
        $scope.$watch('filter.inputClientName', function(val) {
            if (!val) {
                $scope.filter.inputClientId = null;
            }
        });
    })
    .controller('invoiceModalController', function($scope, $modalInstance, $filter, client, invoice, invoiceService) {
        $scope.client = client;
        $scope.invoice = {
            InvoiceId: null,
            ClientId: client.ClientId,
            Date: null,
            ItemList: []
        }

        if (invoice) {
            $scope.isEditMode = true;
            angular.extend($scope.invoice, invoice);
            $scope.invoice.Date = $filter('jsonDate')($scope.invoice.Date);
        }

        $scope.today = function() {
            $scope.dt = new Date();
        };

        $scope.openDatePicker = function($event) {
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

        $scope.removeItem = function(item) {
            var index = $scope.invoice.ItemList.indexOf(item);
            $scope.invoice.ItemList[index].IsDeleted = true;
        }

        $scope.save = function() {
            $scope.$broadcast('show-errors-check-validity');
            if ($scope.invoiceForm.$valid) {
                if ($scope.isEditMode) {
                    invoiceService.editInvoice($scope.invoice).then(function(data) {
                        $modalInstance.close($scope.invoice);
                    });
                } else {
                    invoiceService.addinvoice($scope.invoice).then(function(data) {
                        $modalInstance.close($scope.invoice);
                    });
                }

            }
        };

        $scope.cancel = function() {
            $modalInstance.dismiss(false);
        };
    })
    .controller('invoiceDeleteModalController', function($scope, $modalInstance, invoice) {
        $scope.invoice = invoice;
        $scope.ok = function() {
            $modalInstance.close($scope.invoice.InvoiceId);
        };

        $scope.cancel = function() {
            $modalInstance.dismiss(false);
        };
    })
    .controller('paymentModalController', function($scope, $modalInstance, $filter, payment, paymentService) {
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
            $scope.payment.Date = $filter('jsonDate')($scope.payment.Date);
        }

        $scope.today = function() {
            $scope.dt = new Date();
        };

        $scope.openDatePicker = function($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.save = function() {
            $scope.$broadcast('show-errors-check-validity');
            if ($scope.paymentForm.$valid) {
                if (!$scope.payment.paymentDate || 0 === $scope.payment.paymentDate.length) {
                    $scope.payment.paymentDate = $scope.payment.Date;
                }
                if (!$scope.payment.description) {
                    $scope.payment.description = $scope.payment.Description;
                }
                if (!$scope.payment.method) {
                    $scope.payment.method = $scope.payment.Method;
                }
                if (!$scope.payment.total) {
                    $scope.payment.total = $scope.payment.Total;
                }
                if (!$scope.payment.invoiceId) {
                    $scope.payment.invoiceId = $scope.payment.InvoiceId;
                }
                if (!$scope.payment.paymentId) {
                    $scope.payment.paymentId = $scope.payment.PaymentId;
                }
                if ($scope.isEditMode) {
                    paymentService.editePayment($scope.payment).then(function(data) {
                        $modalInstance.close($scope.payment);
                    });
                } else {
                    paymentService.addPayment($scope.payment).then(function(data) {
                        $modalInstance.close($scope.payment);
                    });
                }

            }
        };

        $scope.cancel = function() {
            $modalInstance.dismiss(false);
        };
    })
    .controller('paymentDeleteModalController', function($scope, $modalInstance, payment) {
        $scope.payment = payment;
        $scope.ok = function() {
            $modalInstance.close($scope.payment);
        };

        $scope.cancel = function() {
            $modalInstance.dismiss(false);
        };
    })
    .controller('reportController', function($scope, $q, chartService, Page) {
        Page.setTitle('REPORTS');
        $scope.reportType = 'monthly';
        $scope.metrics = {
            invoice: {
                name: "invoice",
                isChecked: true
            },
            payment: {
                name: "payment",
                isChecked: true
            }
        };

        $scope.getInvoices = function(type) {
            chartService.getInvoiceData(type).then(function(data) {

            });
        }

        $scope.getPaymentData = function(type) {
            chartService.getPaymentData(type).then(function(data) {

            });
        }

        $scope.getReport = function() {
            var pointSeries = [];
            var categories = [];
            var invoicePromise = null;
            var paymentPromise = null;

            if ($scope.metrics.invoice.isChecked) {
                invoicePromise = chartService.getInvoiceData($scope.reportType);
            }
            if ($scope.metrics.payment.isChecked) {
                paymentPromise = chartService.getPaymentData($scope.reportType);
            }
            $scope.chartConfig.loading = true;
            $q.all([invoicePromise, paymentPromise]).then(function(data) {
                if (invoicePromise) {
                    var processedInvoicesPoint = new Array();
                    var processedInvoicesCategory = new Array();
                    angular.forEach(data[0], function(idata) {
                        processedInvoicesPoint.push({
                            y: idata.Value,
                            name: idata.Label
                        });
                        processedInvoicesCategory.push(idata.Label);
                    });
                    pointSeries.push({
                        name: 'Invoices',
                        data: processedInvoicesPoint,
                        marker: {
                            symbol: 'circle'
                        },
                        color: '#418CF0'
                    });

                    categories = processedInvoicesCategory;
                }
                if (paymentPromise) {
                    var processedPaymentsPoint = new Array();
                    var processedPaymentsCategory = new Array();
                    angular.forEach(data[1], function(pdata) {
                        processedPaymentsPoint.push({
                            y: pdata.Value,
                            name: pdata.Label
                        });
                        processedPaymentsCategory.push(pdata.Label);
                    });
                    pointSeries.push({
                        name: 'Payments',
                        data: processedPaymentsPoint,
                        marker: {
                            symbol: 'circle'
                        },
                        color: '#FCB441'
                    });

                    categories = processedPaymentsCategory;
                }
                $scope.chartConfig.series = pointSeries;
                $scope.chartConfig.xAxis.categories = categories;
                $scope.chartConfig.loading = false;
            });
        }

        $scope.chartConfig = {
            options: {
                chart: {
                    type: 'line',
                    zoomType: 'x'
                },
                tooltip: {
                    shared: true,
                    crosshairs: true
                },

            },
            series: [],
            title: {
                text: ''
            },
            xAxis: {
                categories: []
            },

            loading: false
        };

        $scope.getReport();
    })