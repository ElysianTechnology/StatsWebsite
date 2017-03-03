import * as util from "util";

module.exports = angular.module('app.datacontext', []).factory('datacontext', datacontext);

function datacontext($http, $location, $state, $rootScope) {

    /* the below code will attempt to take the calling url and dynamiclly set the server location */
    const server = $location.$$protocol === 'https' ?
        util.format("%s://%s:%s", $location.$$protocol, $location.$$host, 433) :
        util.format("%s://%s:%s", $location.$$protocol, $location.$$host, $location.$$port);
   

    // helper function for returning data cleanly
    function _onSuccess(data) {
        return data.data;
    }

    function _onError(data) {
        switch (data.status) {
            case 401:
                $timeout(function () {
                    $rootScope.$broadcast('unauthorised')
                    $state.go("Login")
                }, 0)
                break;
        }
    }

    return {
        /* expose the functions here, example: */
       
    };
}