(function () {
    'use strict';

    var app = angular.module('farm');

    // Collect the routes
    app.constant('routes', getRoutes());

    app.config(['routes', '$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', routeConfigurator]);

    function routeConfigurator(routes, $stateProvider, $urlRouterProvider, $ocLazyLoadProvider) {
        $urlRouterProvider.otherwise("/c/dashboard");
        //$urlRouterProvider.otherwise(function ($injector) {
        //    var $state = $injector.get("$state");
        //    $state.go('/c/dashboard');
        //});
        $ocLazyLoadProvider.config({
            // Set to true if you want to see what and when is dynamically loaded
            debug: false
        });

        routes.forEach(function (r) {
            $stateProvider.state(r);
        });


    }

    var events = {
        controllerActivateSuccess: 'controller.activateSuccess',
        spinnerToggle: 'spinner.toggle'
    };

    var config = {
        appErrorPrefix: '[HT Error] ', //Configure the exceptionHandler decorator
        docTitle: 'Farm: ',
        events: events,
        version: '2.1.0'
    };

    app.value('config', config);
    app.config(['commonConfigProvider', function (cfg) {
        cfg.config.controllerActivateSuccessEvent = config.events.controllerActivateSuccess;
        cfg.config.spinnerToggleEvent = config.events.spinnerToggle;
    }]);

    function getRoutes() {
        return [
             {
                 name: 'common',
                 url: "/c",
                 abstract: true,
                 templateUrl: "app/views/common/content.html",
                 //data: { pageTitle: 'О нас' }
             },
             {
                 name: 'admin',
                 url: "/a",
                 abstract: true,
                 templateUrl: "app/views/common/content.html",
                 //data: { pageTitle: 'О нас' }
             },
             {
                 name: 'production',
                 url: "/production",
                 abstract: true,
                 templateUrl: "app/views/common/content.html",
                 resolve: {
                     loadPlugin: function ($ocLazyLoad) {
                         return $ocLazyLoad.load([
                             {
                                 serie: true,
                                 files: ['/js/dataTables/datatables.min.js', '/css/datatables.min.css']
                             },
                             {
                                 serie: true,
                                 name: 'datatables',
                                 files: ['/lib/angular-datatables/dist/angular-datatables.min.js']
                             },
                             {
                                 serie: true,
                                 name: 'datatables.buttons',
                                 files: ['/js/dataTables/angular-datatables.buttons.min.js']
                             }
                         ]);
                     }
                 }
             },
            {
                name: 'common.dashboard',
                url: "/dashboard",
                templateUrl: "app/views/dashboard/dashboard.html",
                data: { pageTitle: 'Dashboard' }
            },
             {
                 name: 'production.store',
                 url: "/store",
                 templateUrl: "app/views/production/productionStore.html",
                 data: { pageTitle: 'Store' }
             },
             {
                 name: 'production.form',
                 url: "/form",
                 templateUrl: "app/views/production/form.html",
                 data: { pageTitle: 'Forms' }
             },
              {
                  name: 'admin.admin',
                  url: "/admin",
                  templateUrl: "app/views/admin/admin.html",
                  data: { pageTitle: 'Admin' }
              }
             //{
             //    name: 'login',
             //    url: "/login",
             //    templateUrl: "app/views/login/login.html",
             //    data: { pageTitle: 'Login' }
             //},

        ];
    }
})();