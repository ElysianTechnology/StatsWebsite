class BaseCtrl {

    constructor($scope) {

        let vm = this

    }
}

module.exports = angular.module('app.base', []).directive('base', () => {
    return {
        controller: BaseCtrl,
        controllerAs: 'base',
        template: require('./base.html')
    };
});