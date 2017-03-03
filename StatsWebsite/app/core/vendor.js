module.exports = function () {

    /*Global modules */
    global.$ = global.jQuery = require('jquery')

    /*core modules*/
    require('angular')
    require('angular-ui-router')

    /* 3rd Party */   
    require('lodash')

    /*styles*/
    require('../css/styles.scss')

};