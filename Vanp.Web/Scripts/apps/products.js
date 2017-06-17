var Products = function () {
    var urlAction = "/Product/GetProducts";
    var dataFilter = {
        pageSize: 3,
        pageNo: 1,
        category: $.isNumeric(location.href.split('/').pop()) ? location.href.split('/').pop() : null,
        orderBy: "dateto",
        asc: false
    }
    var handleOrder = function () {
        $("#sort-by").find(".sort-by-list").on("click", ".sort-by-item", function (e) {
            var eSortByName = $("#sort-by").find(".sort-by-name");
            eSortByName.data("sort-by-name", $(this).find("a").data("sort-by-name"));
            eSortByName.text($(this).find("a").text());
            if (dataFilter.orderBy) {
                dataFilter.orderBy = eSortByName.data("sort-by-name");
                reset();

            }
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
            reset();


        });
    }
    var handleLoadMore = function () {
        $(".category-products .btn-load-more").on("click", function () {
            if (dataFilter.pageNo) {
                dataFilter.pageNo++;
            }
            getProducts();
        })
    }
    var getProducts = function () {
        $.when(Vanp.handleAjaxGet(urlAction, dataFilter)).done(
            function (data) {
                if (data) {
                    if (data.error !== 1) {
                        $.each(data.data, function (i, v) {
                            var product = new Product();
                            product.init(v);
                        });

                    }
                } else {
                    alert("Lỗi kết nối!");
                }
            });
    }
    var reset = function () {
        dataFilter.pageNo = 1;
        $("#products-list").empty();
        getProducts();
    }
    return {
        //main function to initiate the module
        init: function () {
            handleOrder();
            handleSort();
            handleLoadMore();
            getProducts();
        },
        getDataFilter: function () {
            return dataFilter;
        },
        setDataFilter: function (datafilter) {
            dataFilter = datafilter;
        },
        setUrlAction: function (url) {
            urlAction = url;
        },
        getUrlAction: function () {
            return urlAction;
        }
    };

}();
