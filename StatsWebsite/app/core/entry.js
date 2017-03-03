require('./vendor.js')();

const appModule = require('../index');

angular.element(document).ready(() => {
    angular.bootstrap(document, [appModule.name],  {

    });

});