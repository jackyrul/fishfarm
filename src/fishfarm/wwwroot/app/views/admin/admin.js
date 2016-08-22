(function () {
    'use strict';
    var controllerId = 'admin';
    angular.module('farm').controller(controllerId, ['common', '$scope', '$localStorage', '$state', 'datacontext', '$uibModal','$rootScope', admin]);

    function admin(common, $scope, $localStorage, $state, datacontext, $uibModal, $rootScope) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        var current = $state.current;
        $scope.user = {};
        //vm.login = login;

        $scope.logoff = function () {

        }

        $scope.editUser = function (userId) {
            var user = $scope.users.filter(function(usr) {
                return usr.Id === userId;
            });
            user = user[0];
            var role = $scope.roles.filter(function (role) {
                return role.Name === user.Roles[0];
            });
            user.Role = role[0].Id;
            $rootScope.modal = {
                roles: $scope.roles,
                user: user,
                ok: function () {
                    return datacontext.changeUserRole($rootScope.modal.user.Id, $rootScope.modal.user.Role)
                        .then(function (data) {
                            if (data.status === 200) {
                                getUsers();
                            }
                            return data.status;
                        });
                }
            }
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/admin/editUser.html',//app/views/admin/admin.html
                controller: modalCtrl
            });
        };
        $scope.createUser = function () {
            $rootScope.modal = {
                roles: $scope.roles,
                user: {},
                ok: function() {
                    return datacontext.createUser($rootScope.modal.user)
                        .then(function (data) {
                            if (data.status === 200) {
                            getUsers();
                        }
                        return data.status;
                        });
                }
            }
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/admin/createUser.html',
                controller: modalCtrl
            });
        };

        $scope.addRoleToUser = function (userId, roleId) {
            datacontext.addRoleToUser(userId, roleId).then(function (data) {
                getUsers();
                //$scope.users = data.data;

            });
        }

        activate();

        function activate() {
            var promises = [getUsers()];
            common.activateController(promises, controllerId)
                .then(function () { log('Admin View') });
        }

        function getUsers() {
            $scope.roles = getRoles();
            datacontext.getUsers().then(function (data) {
                $scope.users = data.data;
            });
        }

        function getRoles() {
            datacontext.getRoles().then(function (data) {
                $scope.roles = data.data;
            });
        }
    }
})();