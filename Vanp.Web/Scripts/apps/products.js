var Products = function () {
    var dataFilter = {
        pageSize: 3,
        pageNo: 1,
        category: $.isNumeric(location.href.split('/').pop()) ? location.href.split('/').pop() : null,
        orderBy: "dateto",
        asc: false
    }
    var Product = function () {
        var _this = this;
        _this.$element = null,
        _this.$template = $(".product-template>.item"),
        _this.$placeholder = $("#products-list"),
        _this.$name = null,
        _this.$image = null,
        _this.$priceCurrent = null,
        _this.$price = null,
        _this.$bidCount = null,
        _this.$endTime = null,
        _this.$createdBy = null,
        _this.$bidCurrentBy = null,
        _this.$description = null,
        _this.$categories = null,
        _this.$btnWishList = null,
        _this.Id = null;

        Product.prototype.init = function (product) {
            _this.$element = this.$template.clone();
            _this.$element.appendTo(this.$placeholder).show('slow');

            _this.$image = this.$element.find(".product-image");
            _this.$name = this.$element.find(".product-name>a");
            _this.$price = this.$element.find(".price");
            _this.$priceCurrent = this.$element.find(".price-current");
            _this.$bidCount = this.$element.find(".bid-count");
            _this.$endTime = this.$element.find(".end-time");
            _this.$createdBy = this.$element.find(".created-by");
            _this.$bidCurrentBy = this.$element.find(".bid-current-by");
            _this.$description = this.$element.find(".desc");
            _this.$categories = this.$element.find(".categories");
            _this.$btnWishList = this.$element.find(".button-wishlist");

            _this.Id = product.Id;
            _this.$name.html(product.ProductName);
            _this.$name.attr("title", product.ProductName);
            _this.$name.attr("href", "/product/" + product.Id);

            _this.$image.find("a").attr("title", product.ProductName);
            _this.$image.find("a").attr("href", "/product/" + product.Id);
            _this.$image.find("img").attr("alt", product.ProductName);
            _this.$image.find("img").attr("src", product.ProductImagePath);

            _this.$priceCurrent.html(numeral(product.PriceCurrent).format('0,0'));
            _this.$price.html(numeral(product.Price).format('0,0'));
            _this.$bidCount.html(numeral(product.BidCount).format('0,0'));
            _this.$createdBy.html(product.CreatedByName);
            _this.$bidCurrentBy.html(product.BidCurrentByName);
            _this.$endTime.html(product.EndTime);
            _this.$description.html(product.ProductDescription);
            var category = product.Category;
            var htmlCategory = "";
            if (category.ParentCategories) {
                var categories = category.ParentCategories.reverse();

                $.each(categories, function (i, v) {
                    htmlCategory += '<a href="/products/category/' + v.Id + '">' + v.CategoryName + '</a> <span class="separator">></span> ';
                });
            }

            htmlCategory += '<a href="/products/category/' + category.Id + '">' + category.CategoryName + '</a>';
            _this.$categories.find(".category-links").append(htmlCategory);

            //Event
            _this.$btnWishList.off("click").on("click", addWishList)
        }
        var addWishList = function () {
            $.when(Vanp.handleAjaxPost("/Customer/Wishlist/Insert", { productId: _this.Id })).done(
              function (data) {
                  if (data) {
                      if (data.error === 1) {
                          alert(data.message);
                      } else {
                          alert(data.message);
                      }
                  } else {
                      alert("Lỗi kết nối!");
                  }
              });
        }
    }
    var positionCurrent = $(".toolbar").position();
    var handleOrder = function () {
        $("#sort-by").find(".sort-by-list").on("click", ".sort-by-item", function (e) {
            var eSortByName = $("#sort-by").find(".sort-by-name");
            eSortByName.data("sort-by-name", $(this).find("a").data("sort-by-name"));
            eSortByName.text($(this).find("a").text());
            dataFilter.orderBy = eSortByName.data("sort-by-name");
            reset();
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
            dataFilter.pageNo++;
            positionCurrent = $(this).position();
            getProducts();
        })
    }
    var getProducts = function () {
        $.when(Vanp.handleAjaxGet("/Product/GetProducts", dataFilter)).done(
            function (data) {
                if (data) {
                    if (data.error !== 1) {
                        $.each(data.data, function (i, v) {
                            var product = new Product();
                            product.init(v);
                        });
                        //scrollTo(positionCurrent.left, positionCurrent.top);
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
        }

    };

}();
jQuery(document).ready(function () {
    Products.init();
});