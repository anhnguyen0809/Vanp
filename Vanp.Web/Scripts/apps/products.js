var Products = function () {
    var dataFilter = {
        pageSize: 10,
        pageNo : 1,
        category: null,
        orderBy: "dateto",
        asc: false
    }
    var handleOrder = function () {
        $("#sort-by").find(".sort-by-list").on("click", ".sort-by-item", function (e) {
            var eSortByName = $("#sort-by").find(".sort-by-name");
            eSortByName.data("sort-by-name", $(this).find("a").data("sort-by-name"));
            eSortByName.text($(this).find("a").text());
            dataFilter.orderBy = eSortByName.data("sort-by-name");
            getProducts();
        });
    }
    var handleSort = function () {

        $("#sort-by").find(".button-asc").on("click", function () {
            var dataAsc = $(this).data("asc");
            if (dataAsc === false) {
                $(this).data("asc", true);
                $(this).find("span").attr("class", "top_arrow");
            } else {
                $(this).data("asc", false);
                $(this).find("span").attr("class", "bottom_arrow");
            }
            dataFilter.asc = $(this).data("asc");
            getProducts();
        });
    }
    var handleLoadMore = function () {
        $(".category-products .btn-load-more").on("click", function () {
            dataFilter.pageNo++;
            getProducts();
        })
    }
    var getProducts = function () {
        $.when(Vanp.handleAjaxGet("/Product/GetProducts", dataFilter)).done(
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