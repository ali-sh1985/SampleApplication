angular.module('mainApp.directives', [])
    .directive('integerValidator', function () {
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                var integerRegexp = /^\-?\d+$/;
                ctrl.$validators.integerValidator = function (modelValue, viewValue) {
                    if (ctrl.$isEmpty(modelValue)) {
                        // consider empty models to be valid
                        return true;
                    }

                    if (integerRegexp.test(viewValue)) {
                        // it is valid
                        return true;
                    }

                    // it is invalid
                    return false;
                };
            }
        };
    })
    .directive('invoiceValidator', function ($q, $timeout, invoiceService) {
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {


                ctrl.$asyncValidators.invoiceValidator = function (modelValue, viewValue) {

                    if (ctrl.$isEmpty(modelValue)) {
                        return $q.reject();
                    }

                    var def = $q.defer();
                    $timeout(function () {
                        invoiceService.validateInvoice(modelValue).then(function (data) {
                            if (data) {
                                def.resolve(data);
                            } else {
                                def.reject();
                            };
                        });
                    }, 2000);


                    return def.promise;
                };
            }
        };
    })
    .directive('showErrors', function ($timeout, showErrorsConfig, $interpolate) {
        var getShowSuccess, getTrigger, linkFn;
        getTrigger = function (options) {
            var trigger;
            trigger = showErrorsConfig.trigger;
            if (options && (options.trigger != null)) {
                trigger = options.trigger;
            }
            return trigger;
        };
        getShowSuccess = function (options) {
            var showSuccess;
            showSuccess = showErrorsConfig.showSuccess;
            if (options && (options.showSuccess != null)) {
                showSuccess = options.showSuccess;
            }
            return showSuccess;
        };
        linkFn = function (scope, el, attrs, formCtrl) {
            var blurred, inputEl, inputName, inputNgEl, options, showSuccess, toggleClasses, trigger;
            blurred = false;
            options = scope.$eval(attrs.showErrors);
            showSuccess = getShowSuccess(options);
            trigger = getTrigger(options);
            inputEl = el[0].querySelector('.form-control[name]');
            inputNgEl = angular.element(inputEl);
            inputName = $interpolate(inputNgEl.attr('name') || '')(scope);
            if (!inputName) {
                throw "show-errors element has no child input elements with a 'name' attribute and a 'form-control' class";
            }
            inputNgEl.bind(trigger, function () {
                blurred = true;
                return toggleClasses(formCtrl[inputName].$invalid);
            });
            scope.$watch(function () {
                return formCtrl[inputName] && formCtrl[inputName].$invalid;
            }, function (invalid) {
                if (!blurred) {
                    return;
                }
                return toggleClasses(invalid);
            });
            scope.$on('show-errors-check-validity', function () {
                return toggleClasses(formCtrl[inputName].$invalid);
            });
            scope.$on('show-errors-reset', function () {
                return $timeout(function () {
                    el.removeClass('has-error');
                    el.removeClass('has-success');
                    return blurred = false;
                }, 0, false);
            });
            return toggleClasses = function (invalid) {
                el.toggleClass('has-error', invalid);
                if (showSuccess) {
                    return el.toggleClass('has-success', !invalid);
                }
            };
        };
        return {
            restrict: 'A',
            require: '^form',
            compile: function (elem, attrs) {
                if (attrs['showErrors'].indexOf('skipFormGroupCheck') === -1) {
                    if (!(elem.hasClass('form-group') || elem.hasClass('input-group'))) {
                        throw "show-errors element does not have the 'form-group' or 'input-group' class";
                    }
                }
                return linkFn;
            }
        };
    })
    .provider('showErrorsConfig', function () {
        var _showSuccess, _trigger;
        _showSuccess = false;
        _trigger = 'blur';
        this.showSuccess = function (showSuccess) {
            return _showSuccess = showSuccess;
        };
        this.trigger = function (trigger) {
            return _trigger = trigger;
        };
        this.$get = function () {
            return {
                showSuccess: _showSuccess,
                trigger: _trigger
            };
        };
    })
    .directive('isActiveNav', function ($location) {
        return {
            restrict: 'A',
            link: function (scope, element) {
                scope.location = $location;
                scope.$watch('location.path()', function (currentPath) {
                    if ('#' + currentPath === element[0].attributes['href'].nodeValue) {
                        element.parent().addClass('active');
                    } else {
                        element.parent().removeClass('active');
                    }
                });
            }
        };
    });