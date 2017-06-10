var Login = function () {

    var handleLogin = function () {

        $('#login-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                username: {
                    required: true
                },
                password: {
                    required: true
                },
                remember: {
                    required: false
                }
            },

            messages: {
                username: {
                    required: "Tài khoản không được bỏ trống."
                },
                password: {
                    required: "Mật khẩu không được bỏ trống."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   
                $('#login-form .alert-danger>span').html("Vui lòng nhập tài khoản và mật khẩu.").parent().show();
            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {

                var loginModel = {
                    userName: $(form.username).val(),
                    password: $(form.password).val()
                }
                form.submit();
                //var btnSubmit = $('.btn-login').ladda();
                //btnSubmit.ladda('start');
                //var returnUrl = Vanp.getURLParameter("ReturnUrl");
                //$.when(Vanp.handleAjaxPost("/Account/Login?returnUrl=" + returnUrl, loginModel)).done(
                //    function (data) {
                //        if (data) {
                //            if (data.error === 1) {
                //                $('#login-form .alert-danger>span').html(data.message).parent().show();
                //            } else {

                //            }
                //        } else {
                //            alert("Lỗi kết nối!");
                //        }
                //    //    btnSubmit.ladda('stop');
                //    });
                //return false;
            }
        });

        $('#login-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('#login-form').validate().form()) {
                    $('#login-form').submit(); //form validation success, call ajax form submit
                }
                return false;
            }
        });
    }

    var handleForgetPassword = function () {
        $('.forget-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                email: {
                    required: true,
                    email: true
                }
            },

            messages: {
                email: {
                    required: "Email is required."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   

            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        $('.forget-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.forget-form').validate().form()) {
                    $('.forget-form').submit();
                }
                return false;
            }
        });

        jQuery('#forget-password').click(function () {
            jQuery('.login-form').hide();
            jQuery('.forget-form').show();
        });

        jQuery('#back-btn').click(function () {
            jQuery('.login-form').show();
            jQuery('.forget-form').hide();
        });

    }

    var handleRegister = function () {

        if ($().datetimepicker && $("#dateofbirth").size() > 0) {
            $("#dateofbirth").datetimepicker({
                format: 'DD/MM/YYYY',
                maxDate: moment()//Date.now()
            })
        }

        $('.register-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                fullname: {
                    required: true,

                },
                email: {
                    required: true,
                    email: true,
                    remote: {
                        url: "/Account/IsNotExisted",
                        type: "POST",
                        data: {
                            username: function () {
                                return $("#email").val();
                            }

                        }
                    }
                },
                address: {
                },
                phone: {
                    required: true
                },
                username: {
                    required: true,
                    remote: {
                        url: "/Account/IsNotExisted",
                        type: "POST",
                        data: {
                            username: function () {
                                return $("#username").val();
                            }
                        }
                    },
                    regex: '^[a-zA-Z0-9]+$'
                },
                password: {
                    required: true,
                    minlength: 6
                },
                repassword: {
                    equalTo: "#password"
                }
            },

            messages: {
                fullname: "Họ tên không được bỏ trống.",
                email: {
                    required: "Email không được bỏ trống.",
                    email: "Email không hợp lệ. Ex: vanp@vanp.com",
                    remote: "Email đã tồn tại."
                },
                address: {
                },
                phone: {
                    required: "Số điện thoại không được bỏ trống."
                },
                username: {
                    required: "Tài khoản không được bỏ trống.",
                    remote: "Tài khoản đã tồn tại.",
                    regex: "Tài khoản phải thuộc a-z, A-Z hoặc 0-9."
                },
                password: {
                    required: "Mật khẩu không được bỏ trống.",
                    minlength: "Mật khẩu chứa ít nhất 6 ký tự."
                },
                repassword: {
                    equalTo: "Xác nhận mật khẩu không trùng khớp."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   

            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                if (element.attr("name") === "tnc") { // insert checkbox errors after the container                  
                    error.insertAfter($('#register_tnc_error'));
                } else if (element.closest('.input-icon').size() === 1) {
                    error.insertAfter(element.closest('.input-icon'));
                } else {
                    error.insertAfter(element);
                }
            },

            submitHandler: function (form) {
                console.log(form);
                form.submit();
                //var registerModel = {
                //    userName: $(form.username).val(),
                //    password: $(form.password).val(),
                //    email: $(form.email).val(),
                //    fullName: $(form.fullname).val(),
                //    address: $(form.address).val(),
                //    phone: $(form.phone).val(),
                //    dateofbirth: moment($(form.dateofbirth).data("DateTimePicker").date(), "MM-DD-YYYY").toDate()
                //}

                //$.when(Vanp.handleAjaxPost("/Account/Register", registerModel)).done(
                //    function (data) {
                //        if (data) {
                //            if (data.error === 1) {
                //                $('.register-form .alert-danger>span').html(data.message).parent().show();
                //            } else {
                //                var backURL = Vanp.getURLParameter("continue");
                //                if (backURL) {
                //                    location.href = backURL;
                //                }
                //            }
                //        } else {
                //            alert("Lỗi kết nối!");
                //        }
                //    });
            }
        });

        $('.register-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.register-form').validate().form()) {
                    $('.register-form').submit();
                }
                return false;
            }
        });

        jQuery('#register-btn').click(function () {
            jQuery('.login-form').hide();
            jQuery('.register-form').show();
        });

        jQuery('#register-back-btn').click(function () {
            jQuery('.login-form').show();
            jQuery('.register-form').hide();
        });
    }
    var handleChange = function () {

        if ($().datetimepicker && $("#dateofbirth").size() > 0) {
            $("#dateofbirth").datetimepicker({
                format: 'DD/MM/YYYY',
                maxDate: moment()//Date.now()
            })
        }

        $('.change-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                fullname: {
                    required: true,

                },
                email: {
                    required: true,
                    email: true,
                    remote: {
                        url: "/Account/IsNotExisted",
                        type: "POST",
                        data: {
                            username: function () {
                                return $("#email").val();
                            },
                            current: true
                        }
                    }
                },
                address: {
                },
                phone: {
                    required: true
                }
            },

            messages: {
                fullname: "Họ tên không được bỏ trống.",
                email: {
                    required: "Email không được bỏ trống.",
                    email: "Email không hợp lệ. Ex: vanp@vanp.com",
                    remote: "Email đã tồn tại."
                },
                address: {
                },
                phone: {
                    required: "Số điện thoại không được bỏ trống."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   

            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                if (element.attr("name") == "tnc") { // insert checkbox errors after the container                  
                    error.insertAfter($('#register_tnc_error'));
                } else if (element.closest('.input-icon').size() === 1) {
                    error.insertAfter(element.closest('.input-icon'));
                } else {
                    error.insertAfter(element);
                }
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        $('.change-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.change-form').validate().form()) {
                    $('.change-form').submit();
                }
                return false;
            }
        });
    }
    var handleChangePassword = function () {

        $('.change-password-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                passwordold: {
                    required: true,
                    minlength: 6
                },
                passwordnew: {
                    required: true,
                    minlength: 6
                },
                repassword: {
                    equalTo: "#passwordnew"
                }
            },

            messages: {
                passwordold: {
                    required: "Mật khẩu cũ không được bỏ trống.",
                    minlength: "Mật khẩu cũ chứa ít nhất 6 ký tự."
                },
                password: {
                    required: "Mật khẩu mới không được bỏ trống.",
                    minlength: "Mật khẩu mới chứa ít nhất 6 ký tự."
                },
                repassword: {
                    equalTo: "Xác nhận mật khẩu mới không trùng khớp."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   

            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                if (element.attr("name") == "tnc") { // insert checkbox errors after the container                  
                    error.insertAfter($('#register_tnc_error'));
                } else if (element.closest('.input-icon').size() === 1) {
                    error.insertAfter(element.closest('.input-icon'));
                } else {
                    error.insertAfter(element);
                }
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        $('.change-password-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.change-password-form').validate().form()) {
                    $('.change-password-form').submit();
                }
                return false;
            }
        });
    }
    return {
        //main function to initiate the module
        init: function () {

            handleLogin();
            handleForgetPassword();
            handleRegister();
            handleChange();
            handleChangePassword();
        }

    };

}();
jQuery(document).ready(function () {

    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
        },
        "Please check your input."
    );
    Login.init();
});