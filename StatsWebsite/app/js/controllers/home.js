module.exports = ($scope, common) => {
    
    $scope.test = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

    activate();

    function activate() {
        common.activateController([], 'home').then(() => {
            console.log('home controller')
        });
    }

};