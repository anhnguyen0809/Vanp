function Product() {
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
    _this.$btnVote = null,
    _this.Id = null;
}
Product.prototype.init = function (product) {
    var _this = this;
    _this.Id = product.Id;
    _this.$element = _this.$template.clone();
    _this.$element.attr("id", "product_" + _this.Id);
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
    _this.$btnVote = _this.$element.find(".button-vote");


    _this.$name.html(product.ProductName);
    _this.$name.attr("title", product.ProductName);
    _this.$name.attr("href", "/product-detail/" + product.Id);

    _this.$image.find("a").attr("title", product.ProductName);
    _this.$image.find("a").attr("href", "/product-detail/" + product.Id);
    _this.$image.find("img").attr("alt", product.ProductName);
    _this.$image.find("img").attr("src", product.ProductImagePath);

    if (product.IsKicked) {
        _this.$element.addClass("kicked");
    }
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
        _this.$element.addClass("new").append('<div class="new-label new-top-right">new</div>');
    }
    if (product.InWishlist === true) {
        _this.$btnWishList.addClass("disabled");
    }
    //Event
    if (_this.$btnWishList.hasClass("remove")) {
        _this.$btnWishList.text("Xóa");
        _this.$btnWishList.off("click").on("click", function (e) {
            $.when(Vanp.handleDeleteWishlist(_this.Id, e)).done(function (data) {
                if (data) {
                    if (data.error === 1) {
                    } else {
                        _this.$element.remove();
                    }
                    Vanp.notify(data.error, data.message);
                } else {
                    alert("Lỗi kết nối!");
                }
            });
        });
    }
    else if (_this.$btnWishList.hasClass("disabled")) {
        _this.$btnWishList.text("Đã Yêu Thích");
    }
    else {
        _this.$btnWishList.text("Yêu Thích");
        var btnInWishList = _this.$btnWishList.clone().addClass("disabled").text("Đã Yêu Thích").prop("disabled", true);
        _this.$btnWishList.off("click").on("click", function (e) {
            $.when(Vanp.handleAddWishlist(_this.Id, e)).done(function (data) {
                if (data) {
                    if (data.error === 1) {
                    } else {
                        _this.$btnWishList.after(btnInWishList)
                        _this.$btnWishList.remove();
                    }
                    Vanp.notify(data.error, data.message);
                } else {
                    alert("Lỗi kết nối!");
                }
            });

        });
    }

}
