(function () {
    'use strict';
    var controllerId = 'productionStore';
    angular.module('farm').controller(controllerId, ['common', '$scope', '$localStorage', '$state', 'DTOptionsBuilder', productionStore]);

    function productionStore(common, $scope, $localStorage, $state, DTOptionsBuilder) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        $scope.bones = [];
        $scope.stores = [
            'Date',
            'Supply',
            'Raw',
            'Milling',
            'Milled',
            'Extruder',
        ];

        for (var i = 0; i < 90; i++) {
            var date = new Date();
            $scope.bones.push({
                date: date.setDate(date.getDate() - i),
                supply: 500 + i,
                raw: 400 + i,
                milling: 300 + i,
                milled: 200 + i,
                extruder: 100 + i
            });
        }
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDOM('<"html5buttons"B>lTfgitp')
        .withButtons([
            {extend: 'copy'},
            {extend: 'csv'},
            {extend: 'excel', title: 'ExampleFile'},
            {extend: 'pdf', title: 'ExampleFile'},

            {extend: 'print',
                customize: function (win){
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ]);

        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { log('Prodaction View'); });//log('Activated Widgets View'); });
        }
    }
})();