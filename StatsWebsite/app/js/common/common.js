module.exports = angular.module('app.common', [])
    .factory('common',  common)
    .provider('commonConfig', function () {
        this.config = {};

        this.$get = () => {
            return {
                config: this.config
            };
        };
    });

function common($q, $rootScope, commonConfig) {
    function activateController(promises, controller) {
        return $q.all(promises).then(eventArgs => {
            const data = { controller: controller };
            $broadcast(commonConfig.config.controllerActivateSuccessEvent, data);
        });
    }

    function $broadcast() {
        return $rootScope.$broadcast.apply($rootScope, arguments);
    }

    return {
        $broadcast: $broadcast,
        $q: $q,
        activateController: activateController
    };
}