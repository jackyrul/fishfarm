(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('farm').controller(controllerId, ['common', '$scope', '$localStorage', '$state', 'datacontext', dashboard]);

    function dashboard(common, $scope, $localStorage, $state, datacontext) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        var current = $state.current;
        $scope.user = {};
        //$scope.$storage = $localStorage;
        //$scope.data = {
        //    people: 20,
        //    messages: 57,
        //    tweets: 62
        //}
        //
        //$scope.$storage = $localStorage.$default({
        //    data: $scope.data
        //});

        function getYahoo() {
            return datacontext.getYData().then(function (data) {
                $scope.datayahoo = data.data.query.results.rate;
            });
        }


        activate();

        function activate() {
            common.activateController([getYahoo()], controllerId)
                .then(function () { log('Dashboard View'); });//log('Activated Widgets View'); });
        }
    }
})();/**
 * Created by Jackyrul on 29.03.2016.
 */
