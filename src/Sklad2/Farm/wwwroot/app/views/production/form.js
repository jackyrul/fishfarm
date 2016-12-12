(function () {
    'use strict';
    var controllerId = 'form';
    angular.module('farm').controller(controllerId, ['common', '$scope', '$localStorage', '$state', form]);

    function form(common, $scope, $localStorage, $state) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        vm.form = {};
        vm.stores = [{ Name: 'Supplies' }, { Name: 'Raw' }, { Name: 'Milling' }, { Name: 'Milled' }, { Name: 'Extruder' }, { Name: 'Raw' }, { Name: 'Finish' }];
        vm.raws = [{ Name: 'Bones' }, { Name: 'Seashells' }, { Name: 'Blood' }, { Name: 'Mukene' }, { Name: 'Yeast' }, { Name: 'Sunflower cake' }, { Name: 'Premix' }];
        var list = [];
        vm.add = function (list) {
            //list
            list.push({});
        };
        $scope.up = function (index, list) {
            //var list = $rootScope.modal.project.images;
            if (index <= 0)
                return;

            list.splice(index - 1, 2, list[index], list[index - 1]);
        }

        $scope.save = function (item, part) {
            //if(part ==1)
            log(item + " added");
        }
        //$scope.save();
        $scope.down = function (index, list) {
            //var list = $rootScope.modal.project.images;
            if (index == -1) {
                return;
            }
            if (list[index + 1]) {
                list.splice(index, 2, list[index + 1], list[index]);
            }
        }

        // note, these field types will need to be
        // pre-defined. See the pre-built and custom templates
        // http://docs.angular-formly.com/v6.4.0/docs/custom-templates
        //vm.userFields = [
        //  {
        //      key: 'email',
        //      type: 'input',
        //      templateOptions: {
        //          type: 'email',
        //          label: 'Email address',
        //          placeholder: 'Enter email'
        //      }
        //  },
        //  {
        //      key: 'password',
        //      type: 'input',
        //      templateOptions: {
        //          type: 'password',
        //          label: 'Password',
        //          placeholder: 'Password'
        //      }
        //  },
        //  {
        //      key: 'file',
        //      type: 'file',
        //      templateOptions: {
        //          label: 'File input',
        //          description: 'Example block-level help text here',
        //          url: 'https://example.com/upload'
        //      }
        //  },
        //  {
        //      key: 'checked',
        //      type: 'checkbox',
        //      templateOptions: {
        //          label: 'Check me out'
        //      }
        //  }
        //];

        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { log('Prodaction View'); });//log('Activated Widgets View'); });
        }
    }
})();