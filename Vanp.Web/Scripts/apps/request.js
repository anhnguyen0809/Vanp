var Request = function () {

    var handleSendRequest = function () {
        $('.send-request-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                requestContent: {
                    required : true,
                    minlength: 20
                }
            },

            messages: {
                requestContent: {
                    required: "Nội dung không được để trống.",
                    minlength: "Tối thiểu 20 ký tự."
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

            submitHandler: function (form) {
                form.submit();
            }
        });

        $('.send-request-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.send-request-form').validate().form()) {
                    $('.send-request-form').submit();
                }
                return false;
            }
        });

    }

    return {
        //main function to initiate the module
        init: function () {

            handleSendRequest();
        }

    };

}();
jQuery(document).ready(function () {

    Request.init();
});