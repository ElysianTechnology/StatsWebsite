module.exports = angular.module('app.routing', []).provider('router', function ($stateProvider) {

        var urlCollection;

        this.$get = ($http, $state) => {
            return {
                setUpRoutes: () => {
                    var collection = require('../services/routes.json');

                    for (var routeName in collection) {
                        if (collection.hasOwnProperty(routeName)) {
                            if (!$state.get(routeName)) {

                                var c = collection[routeName];
                                if (c.name == null) {
                                    continue;
                                }
                                if (c.group === null) {
                                    c.group = "";
                                }
                                if (c.views === null) {
                                    c.views = "";
                                }
                                $stateProvider.state(
                                    c.name, {
                                        url: c.url,
                                        cache: false,
                                        template: require('../../html/views/' + c.name.toLowerCase() + '.html'),
                                        controller: require('../controllers/' + c.name.toLowerCase() + '.js'),
                                        controllerAs: c.controllerAs,
                                        display: c.display,
                                        hideNav: (c.hideNav === 'true'),
                                        group: c.group
                                    }
                                );
                            }
                        }
                    }
                }
            };
        };

        this.setCollectionUrl = (url) => {
            urlCollection = url;
        };
    })

    .config(($stateProvider, $urlRouterProvider, routerProvider) => {

        $urlRouterProvider.otherwise('/');

        routerProvider.setCollectionUrl('../js/services/routes.json');
    })
    .controller('MainController', ($scope, router, $transitions, $rootScope, localStorageService, datacontext) => {
        $scope.reload = () => {
            router.setUpRoutes();
        };


    })

    .run((router, $transitions, $rootScope, localStorageService, datacontext, $timeout) => {
        router.setUpRoutes();

        $transitions.onBefore({}, function (trans) {
            // console.log('before stuff')
            // console.log(trans)

            var $state = trans.router.stateService
            var targetState = trans.to();

            $timeout(function () {

                if (targetState.name === 'Login') {
                    // console.log('going to login page')
                    $rootScope.$broadcast('unauthorised')
                }

            }, 0);
        });

        $transitions.onStart({}, function (trans) {

            // console.log(trans)
            $timeout(function () {
                // console.log(trans);
                if (trans.router.stateService.current.name === 'Login') {
                    return
                    // trans.cancel
                } else {
                    datacontext.check().then(function (data) {
                        // console.log('logging', data)
                        if (data != undefined || null)
                            $rootScope.$broadcast('authorised')

                        return
                    }, function (r) {
                        // console.log('unauthorised = show login page')
                        $state.go('Login')
                    })
                }

            }, 0);
        });



    });