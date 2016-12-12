(function () {
    'use strict';
    var controllerId = 'login';
    angular.module('farm').controller(controllerId, ['common', '$scope', '$localStorage', '$state', 'datacontext', login]);

    function login(common, $scope, $localStorage, $state, datacontext) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        var current = $state.current;
        $scope.user = {};
        vm.login = login;

        function login () {
            datacontext.login($scope.user).then(function (data) {

                //$scope.main = JSON.parse(data.data);
                //datacontext.getProjects();

                //$scope.$storage.main = $scope.main;

            });
        }
        

        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { log('Please, login!'); });//log('Activated Widgets View'); });
        }
    }
})();