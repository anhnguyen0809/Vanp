var Request = function () {

    var handleSendRequest = function () {
        $('.vote-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                voteContent: {
                    required : true,
                    minlength: 20
                }
            },

            messages: {
                voteContent: {
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
                var vote = $(form).find(".vote-count-post").text();
                var productId = $(form).find(".productId").val();
                var voteModel = {
                    productId: productId,
                    content: $(form.voteContent).val(),
                    vote: isNaN(vote) ? 0 : vote
                }
                $.when(Vanp.handleVote(voteModel, form.btnSubmit)).done(function (data) {
                    if (data) {
                        if (data.error === 1) {
                        } else {
                            if (data.data) {
                                $.when(Vanp.getVoteById(data.data)).done(function (data) {
                                    var vote = new Vote($("#product_" + productId));
                                    vote.init(data.data);
                                    $("#product_" + productId).find(".button-vote").remove();
                                });
                                
                            }
                        }
                        Vanp.notify(data.error, data.message);
                    } else {
                        alert("Lỗi kết nối!");
                    }
                    $(form).slideToggle("slow");
                    $(form).find(".productId").val("");
                    $(form).find(".vote-up-on").trigger("click");
                    $(form).validate().resetForm();
                });
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