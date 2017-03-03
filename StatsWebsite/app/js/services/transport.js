module.exports = angular.module('app.transport', []).service('transport', transportService);

function transportService($http, $state, $rootScope, $transitions, datacontext, localStorageService) {

    var newReportDataId;

    function setNewReportDataId(id) {
        newReportDataId = id;
        return;
    }

    function getNewReportDataId(){
        return newReportDataId;
    }

    return {
        setNewReportDataId: setNewReportDataId,
        getNewReportDataId: getNewReportDataId
    };
}