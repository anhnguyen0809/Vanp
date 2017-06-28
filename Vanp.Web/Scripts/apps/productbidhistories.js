Product.prototype.addBidHistories = function (bidhistories) {
    var _this = this;
    var $bidhistory = $(".bidhistory-template>.bidhistory").clone();
    var $ptemplate = $('<p class="text-right"><i><span class="created-when"></span> - Giá hiện tại: <span class="price-current" style="color:inherit; font-size:13px"></span> - Giá đã đấu: <span class="price-bid" style="color:#e33b3f; font-weight:700"></span></i></p>');
    $.each(bidhistories, function (i, v) {
        var $p = $ptemplate.clone();
        $p.find(".created-when").text(moment(v.CreatedWhen).format("DD/MM/YYYY hh:mm:ss"));
        $p.find(".price-current").text(numeral(v.PriceCurrent).format("0,0"));
        $p.find(".price-bid").text(numeral(v.PriceBid).format("0,0"));
        $bidhistory.append($p);
    });
    _this.$element.append($bidhistory);
}
var Products = function () {
    var urlAction = "/Product/GetProducts";
    var dataFilter = {
        pageSize: 3,
        pageNo: 1,
        category: $.isNumeric(location.href.split('/').pop()) ? location.href.split('/').pop() : null,
        orderBy: "dateto",
        asc: false
    }
    var settingPage = {
        className: 'products'
    }
    var page = {
        $range: $('.' + settingPage.className).find(".page-info .range"),
        $total: $('.' + settingPage.className).find(".page-info .totalrows"),
        $loadMore: $('.' + settingPage.className).find(".btn-load-more")
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
        $(".category-products .btn-load-more").on("click", function (event) {
            if (dataFilter.pageNo) {
                dataFilter.pageNo++;
            }
            getProducts(event);
        })
    }
    var getProducts = function (event) {
        $.when(Vanp.handleAjaxGet({ url: urlAction, params: dataFilter, event: event })).done(
            function (data) {
                if (data) {
                    if (data.error !== 1) {
                        var products = data.data.products;
                        $.each(products, function (i, v) {
                            var product = new Product();
                            product.init(v.product);
                            if (v.bidhistories.length) {
                                product.addBidHistories(v.bidhistories);
                            }

                        });
                        var pageSize = dataFilter.pageSize;
                        var pageNo = dataFilter.pageNo;
                        var rows = pageSize * (pageNo - 1) + products.length;
                        page.$range.text(rows);
                        page.$total.text(data.data.totalRows);
                        if (data.data.totalRows === rows) {
                            page.$loadMore.remove();
                        }
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
