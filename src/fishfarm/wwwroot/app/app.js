(function () {
    var app = angular.module('farm', [
         'ui.router',                    // Routing
        'common',
        'oc.lazyLoad',                  // ocLazyLoad
        'ui.bootstrap',                 // Ui Bootstrap
        //'pascalprecht.translate',       // Angular Translate
        //'ngIdle',                       // Idle timer
        'ngSanitize',
         'ngStorage',
         'permission',
         'permission.ui',
         'formly'

    ]);

    app.run(['$templateCache', '$rootScope', '$state', '$stateParams', '$http', 'datacontext',
        function ($templateCache, $rootScope, $state, $stateParams, $http, datacontext) {

            datacontext.getUser().then(function (data) {

                $rootScope.user = data.data;

            });
            //$http.defaults.headers.common['X-XSRF-Token'] =
            //angular.element('input[name="__RequestVerificationToken"]').attr('value');
            $rootScope.issupportsvg = document.implementation.hasFeature("http://www.w3.org/TR/SVG11/feature#Shape", "1.0");
            $rootScope.$state = $state;
            $rootScope.$stateParams = $stateParams;
        }]);
})();
