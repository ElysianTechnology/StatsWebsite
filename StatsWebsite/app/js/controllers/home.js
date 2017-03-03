module.exports = ($scope, common) => {

    var vm = this;

    activate();

    function activate() {
        common.activateController([], 'home').then(() => {
            console.log('home controller')
        });
    }

   
};