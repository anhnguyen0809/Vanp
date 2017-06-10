$('.register-form').validate({
    errorElement: 'span', //default input error message container
    errorClass: 'help-block', // default input error message class
    focusInvalid: false, // do not focus the last invalid input
    rules: {
        ProductName: {
            required: true,
        },
        ProductCode: {
            required: true,
        },
        PriceDefault: {
            required: true,
        },
        CategoryId: {
            required: true
        },
        ProductDescription: {
            required: true,
        },
    },

    messages: {
        ProductName: "Tên sản phẩm không được bỏ trống.",
        ProductCode: {
            required: "Mã sản phẩm không được để trống!!!",
        },
        PriceDefault: {
            required: "Giá sản phẩm không được để trống!!!",
        },
        ProductDescription: {
            required: "Mô tả không được để trống"
        },
        ProductText: {
            required: "Mô tả không được để trống",
        },
    },
});
$(document).ready(function () {
    $('.upload').fileinput({
        showUpload: false,
    });
});
$("input[class='touchspin']").TouchSpin({
    verticalbuttons: true,
    max: 1000000000,
    step: 100000
});
$('.selectpicker').selectpicker({
    style: 'btn-info',
});
tinymce.init({
    selector: 'textarea',
    height: 250,
    menubar: false,
    plugins: [
      'advlist autolink lists link image charmap print preview anchor',
      'searchreplace visualblocks code fullscreen',
      'insertdatetime media table contextmenu paste code'
    ],
    toolbar: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
    content_css: [
      '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
      '//www.tinymce.com/css/codepen.min.css'],
    encoding: "xml",
});