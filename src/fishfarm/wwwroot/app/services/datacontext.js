(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('farm').factory(serviceId, ['$http', 'dataProvider', 'common', datacontext]);

    function datacontext($http, dataProvider, common) {
        var $q = common.$q;
        var logger = common.logger.getLogFn('dataservice');
        var logError = common.logger.getLogFn('dataservice', 'error');
        var logWarning = common.logger.getLogFn('dataservice', 'warn');
        var service = {
            createGuid: createGuid,
            getYData: getYData,
            //getEntertein: getEntertain,
            createUser: createUser,
            logoff: logoff,
            getUser: getUser,
            getUsers: getUsers,
            getRoles: getRoles,
            addRoleToUser: addRoleToUser,
            changeUserRole: changeUserRole
        };

        return service;

        function getUsers() {
            var sc = {};
            return dataProvider.get(sc, '/api/GetUsers', sc,
                function success(data, status) {
                    var st = status;
                    return data;
                },
                function fail(data, status) {
                    //window.location.href = "/";
                });
        }

        function getRoles() {
            var sc = {};
            return dataProvider.get(sc, '/api/GetRoles', sc,
                function success(data, status) {
                    var st = status;
                    return data;
                },
                function fail(data, status) {
                    //window.location.href = "/";
                });
        }
        function addRoleToUser(userId, roleId) {
            var sc = {};
            return dataProvider.get(sc, '/api/AddRoleToUser/' + userId + '&' + roleId, sc,
                function success(data, status) {
                    var st = status;
                    return data;
                },
                function fail(data, status) {
                    logError(data);
                });
        }

        function changeUserRole(userId, roleId) {
            var sc = {};
            return dataProvider.get(sc, '/api/ChangeUserRole/' + userId + '&' + roleId, sc,
                function success(data, status) {
                    logger('Role Changed');
                    var st = status;
                    return data;
                },
                function fail(data, status) {
                    if (data == "Already Added") {
                        logWarning(data);
                    } else {
                        logError(data);
                    }

                });
        }

        function createUser(user) {
            var sc = {};
            return dataProvider.post(sc, '/api/CreateUser',

                JSON.stringify(user),   //+ userId + '&' + roleId

                function success(data, status) {
                    logger('User Created');
                    var st = status;
                    return data;
                },
                function fail(data, status) {
                    if (data == "User already exist") {
                        logWarning(data);
                    } else {
                        logError(data);
                    }
                });

            return $q.when(72);

        }

        function logoff() {
            var sc = {};
            return dataProvider.post(sc, '/Account/LogOff', sc
            , function success(data, status) {
                var st = status;
                window.location.href = "/";
            }),
            function fail(data, status) {
                window.location.href = "/";
            }
        }
        function getUser() {
            var sc = {};
            return dataProvider.get(sc, '/api/GetCurrentUser', sc,
                function success(data, status) {
                    var st = status;
                    return data;
                },
                function fail(data, status) {
                    //window.location.href = "/";
                });
        }

        function getYData() {
            var url = "http://query.yahooapis.com/v1/public/yql";
            var symbol = '"USDUGX", "USDEUR", "USDGBP", "USDUAH"';//, "USDJPY","USDCHF", "USDSEK", "USDNOK", "USDRUB", "USDTRY", "USDBRL", "USDCAD", "USDCNY", "USDHKD", "USDINR", "USDKRW", "USDMXN", "USDNZD", "USDSGD", "USDZAR"';
            var data = encodeURIComponent("select * from yahoo.finance.xchange where pair in (symbol)");
            data = data.replace("symbol", symbol);
            /*
            http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20('aapl')&format=json&diagnostics=true&env=http://datatables.org/alltables.env
            */
            var str1 = url.concat("?q=", data);
            str1 = str1.concat("&format=json&env=store://datatables.org/alltableswithkeys"); //http://datatables.org/alltables.env

            var sc = {};
            return dataProvider.get(sc, str1, function (data, status) {
                //$scope.GetAllProgresses = data;
            });

        }


        function createGuid() {
            // http://www.ietf.org/rfc/rfc4122.txt
            var s = [];
            var hexDigits = "0123456789abcdef";
            for (var i = 0; i < 36; i++) {
                s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
            }
            s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
            s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01
            s[8] = s[13] = s[18] = s[23] = "-";

            var uuid = s.join("");
            return uuid;
        }


    }
})();

