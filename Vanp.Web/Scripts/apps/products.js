var Products = function () {
    var dataFilter = {
        pageSize: 10,
        pageNo : 1,
        category: null,
        orderBy: "dateto",
        asc: false
    }
    var Product = function () {
        this.$element = null, 
        this.$template = $(".product-template"),
        this.$placeholder = $("#products-list"),
        this.$name = null,
        this.$image = null,
        this.$priceCurrent = null,
        this.$price = null,
        this.$bidCount = null,
        this.$endTime = null,
        this.$createdBy = null,
        this.$bidCurrentBy = null,
        this.$btnWishList = null

        Product.prototype.init = function (product) {
            this.$element = this.$template.clone();
            this.$element.appendTo(this.$placeholder);

            this.$image = this.$element.find(".product-image");
            this.$name = this.$element.find(".product-name");
            this.$price = this.$element.find(".product-price");
            this.$priceCurrent = this.$element.find(".price-current");
            this.$bidCount = this.$element.find(".bid-count");
            this.$endTime = this.$element.find(".end-time");
            this.$createdBy = this.$element.find(".created-by");
            this.$bidCurrentBy = this.$element.find(".bid-current-by");

            this.$name product.ProductName;
            this.$name = product.ProductName;
            this.$name = product.ProductName;
            this.$name = product.ProductName;
        }
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
                        $.each(data, function (i, v) {
                           
                        });
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