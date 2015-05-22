angular.module('mainApp.filters', [])
.filter('jsonDate', function() {
    return function (inputDate) {
        var pattern = /Date\(([^)]+)\)/;
        if (pattern.test(inputDate)) {
            var results = pattern.exec(inputDate);
            var dt = new Date(parseFloat(results[1]));
            var month = (dt.getMonth() + 1) < 10 ? "0" + (dt.getMonth()) : (dt.getMonth());
            var day = dt.getDate() < 10 ? "0" + dt.getDate() : dt.getDate();
            var year = dt.getFullYear();
            return new Date(year, month, day);
        }
        return "";
    }
})