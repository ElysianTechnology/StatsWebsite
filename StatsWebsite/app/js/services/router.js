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
    .controller('MainController', ($scope, router) => {
        $scope.reload = () => {
            router.setUpRoutes();
        };
    })

    .run((router) => {
        router.setUpRoutes();
    });