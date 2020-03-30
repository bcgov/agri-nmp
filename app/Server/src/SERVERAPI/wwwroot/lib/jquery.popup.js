/**
 * jquery-popup - v0.0.1
 * http://www.github.com/gsmeira/jquery-popup
 *
 * Made by Gustavo Meireles
 * Under MIT License
 */
;(function($, window, document, undefined) {

    'use strict';

    var $document = $(document);

    /**
     * Popup wrapper.
     *
     * @returns {Popup}
     */
    var Popup = (function() {

        function Popup(element, options) {
            var _ = this,
                $element = $(element),
                defaults = {
                    name: '',
                    width: 640,
                    height: 480,
                    position: 'center',
                    top: null,
                    left: null
                };

            _.settings = $.extend({}, defaults, options);

            _.url = $element.attr('href');

            $element.click(function(e) {
                e.preventDefault();

                if (! _.settings.top || ! _.settings.left) {
                    _.getPosition();
                }

                _.show();

                return false;
            });
        }

        Popup.prototype.show = function() {
            var _ = this,
                popup = window.open(_.url, _.settings.name, 'width=' + _.settings.width + ',' +
                                                            'height=' + _.settings.height + ',' +
                                                            'top=' + _.settings.top + ',' +
                                                            'left=' + _.settings.left + ',' +
                                                            'scrollbars=yes');

            if (window.focus) {
                popup.focus();
            }
        };

        Popup.prototype.getPosition = function() {
            var _ = this,
                dualScreenLeft = (window.screenLeft != 'undefined') ? window.screenLeft : screen.left,
                dualScreenTop = (window.screenTop != 'undefined') ? window.screenTop : screen.top,
                viewportWidth = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width,
                viewportHeight = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height,
                top = 0,
                bottom = viewportHeight - _.settings.height + dualScreenTop,
                left = 0,
                right = viewportWidth - _.settings.width + dualScreenLeft,
                center = {
                    x: (viewportWidth / 2) - (_.settings.width / 2) + dualScreenLeft,
                    y: (viewportHeight / 2) - (_.settings.height / 2) + dualScreenTop
                };

            if (_.settings.position === 'top-left') {
                _.setPosition(top, left);
            } else if (_.settings.position === 'top') {
                _.setPosition(top, center.x);
            } else if (_.settings.position === 'top-right') {
                _.setPosition(top, right);
            } else if (_.settings.position === 'left') {
                _.setPosition(center.y, left);
            } else if (_.settings.position === 'center') {
                _.setPosition(center.y, center.x);
            } else if (_.settings.position === 'right') {
                _.setPosition(center.y, right);
            } else if (_.settings.position === 'bottom-left') {
                _.setPosition(bottom, left);
            } else if (_.settings.position === 'bottom') {
                _.setPosition(bottom, center.x);
            } else if (_.settings.position === 'bottom-right') {
                _.setPosition(bottom, right);
            }
        };

        Popup.prototype.setPosition = function(top, left) {
            var _ = this;

            _.settings.top = top;
            _.settings.left = left;
        };

        return Popup;

    })();

    $.fn.popup = function(options) {
        var _ = this,
            l = _.length,
            i;

        for (i = 0; i < l; i++) {
            _[i] = new Popup(_[i], options);
        }

        return _;
    };

    $document.ready(function() {
        var $popups = $('[data-popup]');

        $popups.each(function() {
            var $el = $(this),
                name = $el.data('popup'),
                width = $el.data('popup-width'),
                height = $el.data('popup-height'),
                position = $el.data('popup-position'),
                top = $el.data('popup-top'),
                left = $el.data('popup-left');

            $el.popup({
                name: name,
                width: width,
                height: height,
                position: position,
                top: top,
                left: left
            });
        });
    });

})(jQuery, window, document);