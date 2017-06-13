$('.insert-form').validate({
    errorElement: 'span', //default input error message container
    errorClass: 'help-block', // default input error message class
    focusInvalid: false, // do not focus the last invalid input
    rules: {
        ProductName: {
            required: true,
        },
        Price: {
            required: true,
        },
        PriceDefault: {
            required: true,
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
$('.selectpicker').selectpicker({
    style: 'btn-info',
});