/**
 * cbpAnimatedHeader.js v1.0.0
 * http://www.codrops.com
 *
 * Licensed under the MIT license.
 * http://www.opensource.org/licenses/mit-license.php
 * 
 * Copyright 2013, Codrops
 * http://www.codrops.com
 */
var cbpAnimatedHeader = (function () {

    var docElem = document.documentElement,
		header = document.querySelector('.navbar-default'),
		didScroll = false,
		changeHeaderOn = 100;

    function init() {
        window.addEventListener('scroll', function (event) {
            if (!didScroll) {
                didScroll = true;
                setTimeout(scrollPage, 150);
            }
        }, false);
    }

    function scrollPage() {
        var sy = scrollY();
        if (sy >= changeHeaderOn) {
            document.getElementById("arrow").style.visibility = "hidden";
            document.getElementById("menu-logo").className = "menu-logo";
            document.getElementById("icon-texto").className = "icon-texto";
            document.getElementById("bs-example-navbar-collapse-1").style.marginTop = "0";
            document.getElementById("nav-bar").className = "navbar navbar-default navbar-fixed-top animated slideInDown navbar-shrink";
        
        }
        else {
           document.getElementById("arrow").style.visibility = "visible";
            document.getElementById("menu-logo").className = "menu-logo-grande";
            document.getElementById("icon-texto").className = "icon-texto-grande";
            document.getElementById("bs-example-navbar-collapse-1").style.marginTop = "50px";
            document.getElementById("nav-bar").className = "navbar navbar-default navbar-fixed-top animated slideInDown";
            
        }
        didScroll = false;
    }

    function scrollY() {
        return window.pageYOffset || docElem.scrollTop;
    }

    init();

})();