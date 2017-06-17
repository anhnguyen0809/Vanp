$('.insert-form').validate({
    errorElement: 'span', //default input error message container
    errorClass: 'help-block', // default input error message class
    focusInvalid: false, // do not focus the last invalid input
    rules: {
        ProductName: {
            required: true,
        },
        Price: {
            number: true
        },
        PriceStep:{
            required: true,
            number:true
        },
        PriceDefault: {
            required: true,
            number: true
        },
        ProductDescription: {
            required: true,
        },
        ProductText: {
            required: true,
        },
        image2:{
            required: true,
        },
        image1:{
            required: true,
        },
        image3: {
            required: true,
        }
    },
    messages: {
        ProductName: "Tên sản phẩm không được bỏ trống!!!",
        ProductCode: {
            required: "Mã sản phẩm không được để trống!!!",
        },
        PriceDefault: {
            required: "Giá sản phẩm không được để trống!!!",
            number: "Giá không hợp lệ!!!"
        },
        ProductDescription: {
            required: "Mô tả không được để trống!!!"
        },
        PriceStep: {
            required: "Bước giá không được để trống",
            number: "Bước giá không hợp lệ!!!"
        },
        Price: {
            number: "Giá không hợp lệ!!!"
        },
        image2: {
            required: "Chưa chọn hình thứ 2",
        },
        image1: {
            required: "Chưa chọn hình thứ 1",
        },
        image3: {
            required: "Chưa chọn hình thứ 3",
        }
    },
});
tinymce.init({
    selector: 'textarea',
    height: 300,
    menubar: false,
    plugins: [
      'advlist autolink lists link image charmap print preview anchor',
      'searchreplace visualblocks code fullscreen',
      'insertdatetime media table contextmenu paste code'
    ],
    toolbar: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
    content_css: [
      '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
      '//www.tinymce.com/css/codepen.min.css']
});