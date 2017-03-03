import angular from 'angular'
import uiRouter from 'angular-ui-router'

module.exports = angular.module('app', [
        uiRouter,
        require('./js/common/common').name,
        require('./js/services/datacontext').name,
        require('./js/services/router').name,
        require('./js/services/transport').name,
        require('./js/directives/base/base').name,
    ])
    .config(['$locationProvider', ($locationProvider) => {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }])