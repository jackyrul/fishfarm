//(function () {
//    'use strict';
//
//    angular.module('Resort', []).filter('urldropbox', function() {
//        return function(input) {
//            return input.replace('https://www.dropbox.com/', 'https://dl.dropboxusercontent.com/');
//        };
//    });
//})();
/**
 * Created by Jackyrul on 30.03.2016.
 */
angular.module('farm').filter('pagination', function () {
    return function (input, start) {
        if (input) {
            start = +start; //parse to int
            return input.slice(start);
        }
        return [];
    }
});
