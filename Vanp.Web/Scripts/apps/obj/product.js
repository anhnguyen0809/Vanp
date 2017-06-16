function Product () {
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
    return {
        init : function (product) {
            _this.$element = _this.$template.clone();
            _this.$element.appendTo(_this.$placeholder).show('slow');

            _this.$image = _this.$element.find(".product-image");
            _this.$name = _this.$element.find(".product-name>a");
            _this.$price = _this.$element.find(".price");
            _this.$priceCurrent = _this.$element.find(".price-current");
            _this.$bidCount = _this.$element.find(".bid-count");
            _this.$endTime = _this.$element.find(".end-time");
            _this.$createdBy = _this.$element.find(".created-by");
            _this.$bidCurrentBy = _this.$element.find(".bid-current-by");
            _this.$description = _this.$element.find(".desc");
            _this.$categories = _this.$element.find(".categories");
            _this.$btnWishList = _this.$element.find(".button-wishlist");

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
            if (product.New === true) {
                _this.$element.addClass("new");
            }
            //Event
            _this.$btnWishList.off("click").on("click", addWishlist);
            var addWishlist = function () {
                Vanp.addWishlist(_this.Id);
            }
        }
    }
}