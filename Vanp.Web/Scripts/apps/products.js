var Products = function () {

    var handleOrder = function () {


    }
    var handleSort = function () {


    }
    var handleLoadMore = function () {
        getProducts();

    }
    var getProducts = function () {
        $.when(Vanp.handleAjaxGet("/Product/GetProducts")).done(
            function (data) {
                if (data) {
                    if (data.error !== 1) {
                        console.log(data);
                    }
                } else {
                    alert("Lỗi kết nối!");
                }
            });
    }
    return {
        //main function to initiate the module
        init: function () {

            handleOrder();
            handleSort();
            handleLoadMore();
        }

    };

}();
jQuery(document).ready(function () {
    Products.init();
});