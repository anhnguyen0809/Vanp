var Vanp = function () {

    var assetsPath = '~/';
    var globalImgPath = 'images/';
    var resizeHandlers = [];
    //Method

    // Handle Select2 Dropdowns
    var handleSelect2 = function () {
        if ($().select2) {
            $.fn.select2.defaults.set("theme", "bootstrap");
            $('.select2me').select2({
                placeholder: "Select",
                width: 'auto',
                allowClear: true
            });
        }
    };
    // Handle DateTimePicker 
    var handleDateTimePicker = function () {
        if ($().datetimepicker) {
            $('.datepicker').datetimepicker({
                format: 'DD/MM/YYYY'
            });
        }
    };
    var handleVote = function () {
        $(".vote-up-on, .vote-down-on").on("click", function () {
            if ($(this).hasClass("vote-down-on")) {
                $(this).removeClass("vote-down-on").addClass("vote-down-off");
                $(this).siblings(".vote-up-off").removeClass("vote-up-off").addClass("vote-up-on");
                $(this).closest(".vote").find(".vote-count-post").html(-1);
            } else {
                $(this).removeClass("vote-up-on").addClass("vote-up-off");
                $(this).siblings(".vote-down-off").removeClass("vote-down-off").addClass("vote-down-on");
                $(this).closest(".vote").find(".vote-count-post").html(1);
            }
        });
        $(".vote-up-on").trigger("click");
    }
    //Begin Validate
    var validateEmail = function (email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }
    //End Validate
    return {
        init: function () {
            handleDateTimePicker();
        },
        handleAjaxPost: function (url, params, async) {
            if (!async) {
                async = true;
            }
            return $.ajax({
                url: url,
                type: "POST",
                data: JSON.stringify(params),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: async,
                success: function (data, textStatus, xhr) {
                    console.log(xhr.status);
                },
                complete: function (xhr, textStatus) {
                    console.log(xhr.status);
                }
            });
        },
        handleAjaxGet: function (url, params, async) {
            if (!async) {
                async = true;
            }
            return $.ajax({
                Type: "GET",
                url: url,
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: async,
                complete: function (xhr, textStatus) {
                    console.log(xhr.status);
                }
            });
        },
        getViewPort: function () {
            var e = window,
                a = 'inner';
            if (!('innerWidth' in window)) {
                a = 'client';
                e = document.documentElement || document.body;
            }

            return {
                width: e[a + 'Width'],
                height: e[a + 'Height']
            };
        },
        addResizeHandler: function (func) {
            resizeHandlers.push(func);
        },
        // wrApper function to scroll(focus) to an element
        scrollTo: function (el, offeset) {
            var pos = (el && el.size() > 0) ? el.offset().top : 0;

            if (el) {
                if ($('body').hasClass('page-header-fixed')) {
                    pos = pos - $('.page-header').height();
                } else if ($('body').hasClass('page-header-top-fixed')) {
                    pos = pos - $('.page-header-top').height();
                } else if ($('body').hasClass('page-header-menu-fixed')) {
                    pos = pos - $('.page-header-menu').height();
                }
                pos = pos + (offeset ? offeset : -1 * el.height());
            }

            $('html,body').animate({
                scrollTop: pos
            }, 'slow');
        },
        validateEmail: validateEmail,

        getURLParameter: function (paramName) {
            var searchString = window.location.search.substring(1),
                i, val, params = searchString.split("&");

            for (i = 0; i < params.length; i++) {
                val = params[i].split("=");
                if (val[0] == paramName) {
                    return unescape(val[1]);
                }
            }
            return null;
        },

        // Wrapper function to  block element(indicate loading)
        blockUI: function (options) {
            options = $.extend(true, {}, options);
            var html = '';
            if (options.animate) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + '<div class="block-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>' + '</div>';
            } else if (options.iconOnly) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""></div>';
            } else if (options.textOnly) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
            } else {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
            }

            if (options.target) { // element blocking
                var el = $(options.target);
                if (el.height() <= ($(window).height())) {
                    options.cenrerY = true;
                }
                el.block({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    centerY: options.cenrerY !== undefined ? options.cenrerY : false,
                    css: {
                        top: '10%',
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            } else { // page blocking
                $.blockUI({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    css: {
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            }
        },

        // Wrapper function to  un-block element(finish loading)
        unblockUI: function (target) {
            if (target) {
                $(target).unblock({
                    onUnblock: function () {
                        $(target).css('position', '');
                        $(target).css('zoom', '');
                    }
                });
            } else {
                $.unblockUI();
            }
        },
        startPageLoading: function (options) {
            if (options && options.animate) {
                $('.page-spinner-bar').remove();
                $('body').append('<div class="page-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>');
            } else {
                $('.page-loading').remove();
                $('body').append('<div class="page-loading"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif"/>&nbsp;&nbsp;<span>' + (options && options.message ? options.message : 'Loading...') + '</span></div>');
            }
        },

        stopPageLoading: function () {
            $('.page-loading, .page-spinner-bar').remove();
        },

        alert: function (options) {

        },

        getAssetsPath: function () {
            return assetsPath;
        },

        setAssetsPath: function (path) {
            assetsPath = path;
        },

        setGlobalImgPath: function (path) {
            globalImgPath = path;
        },

        getGlobalImgPath: function () {
            return assetsPath + globalImgPath;
        },
        getResponsiveBreakpoint: function (size) {
            // bootstrap responsive breakpoints
            var sizes = {
                'xs': 480,     // extra small
                'sm': 768,     // small
                'md': 992,     // medium
                'lg': 1200     // large
            };

            return sizes[size] ? sizes[size] : 0;
        },
        handleAddWishlist: function (productId) {
            return this.handleAjaxPost("/Customer/Wishlist/Insert", { productId: productId });
        },
        handleDeleteWishlist: function (productId) {
            return this.handleAjaxPost("/Customer/Wishlist/Delete", { productId: productId });
        },
        handleVote: handleVote
    };

}();

$(function () {
    $(document).ajaxError(function (e, xhr) {
        if (xhr.status == 401)
            window.location = "/Account/Login";
        else if (xhr.status == 403)
            alert("You have no enough permissions to request this resource.");
    });
    Vanp.init();
})