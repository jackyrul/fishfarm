(function () {
    'use strict';
    var controllerId = 'logoff';
    angular.module('farm').controller(controllerId, ['common', '$scope', '$localStorage', '$state', 'datacontext', logoff]);

    function logoff(common, $scope, $localStorage, $state, datacontext) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        var current = $state.current;
        $scope.user = {};
        //vm.login = login;

        $scope.logoff = function () {
            datacontext.logoff().then(function (data) {

                //$scope.main = JSON.parse(data.data);
                //datacontext.getProjects();

                //$scope.$storage.main = $scope.main;

            });
        }


        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { });//log('Activated Widgets View'); });
        }
    }
})();