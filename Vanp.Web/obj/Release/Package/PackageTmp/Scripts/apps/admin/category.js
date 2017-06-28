var $modal = $("#basic");
var requestTable = $("#table-main").DataTable({
    ajax: {
        url: "/Admin/Category/GetList",
        dataSrc: "data"
    },
    pagingType: "bootstrap_full_number",
    columns: [
            {
                data: "Id", render: function (data, type, row, meta) {
                    return '<button type="button" class="btn dark btn-outline" onclick=update(this,' + data + ')>Sửa</button>';
                }
            },
            {
                data: "Id", render: function (data, type, row, meta) {
                    return '<button type="button" class="btn red btn-outline" onclick=deletecategory(this,' + data + ')>Xóa</button>';
                }
            },
            { data: "CategoryName" },

            {
                data: "ParentCategories", render: function (data, type, row, meta) {
                    if (data != null) {
                        var names = $.map(data.reverse(), function (e) { return e.CategoryName; });
                        return names.join(", ");
                    } return "";
                }
            },
             { data: "CategoryLevel" },
            {
                data: "ModifiedWhen", render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY hh:mm:ss");
                }
            },

            {
                data: "Show", render: function (data, type, row) {
                    if (data) {
                        return '<span class="label label-sm label-info"> Hiện </span>';
                    } else {
                        return '<span class="label label-sm label-warning"> Ẩn </span>';
                    }
                }
            }

    ],
    fnRowCallback: function (nRow, aData, iDisplayIndex) {
        $(nRow).addClass("row_" + aData["Id"]);
    },
});
var categorySelect2 =  $('.category-form .parent').select2({
    data: [],
    width: null
});
//init
handleCategory();
resetFrom();
function insert(event) {
    $modal.modal("show");
}

function update(event, id) {
    $modal.modal("show");
    Vanp.handleAjaxGet({ url: "/Admin/Category/GetById", params: { id: id }, event: event }).done(function (data) {
        if (data != null) {
            if (data.error === 0) {
                var category = data.data;
                $('.category-form .id').val(category.Id);
                $('.category-form .name').val(category.CategoryName);
                $('.category-form .parent').val(category.CategoryParentId);
                console.log(categorySelect2.select2('data'));
                $('.category-form .parent').find("option[value=" + category.Id + "]").remove();
                $('.category-form .show').prop("checked", category.Show);
            }
        } else {
            alert("Lỗi kết nối!");
        }
    });
}
function deletecategory(event, id) {
    $.confirm({
        title: 'Xác Nhận!',
        content: 'Bạn chắc chắn muốn xóa danh mục này!',
        buttons: {
            confirm: {
                text: 'Xác Nhận',
                btnClass: 'btn-blue',
                action: function () {
                    $.when(Vanp.handleAjaxPost({ url: "/Admin/Category/Delete", params: { categoryId: id }, event: event })).done(function (data) {
                        if (data != null) {
                            if (data.error === 0) {
                                requestTable.rows(".row_" + id).remove().draw();;
                            }
                            Vanp.notify(data.error, data.message);
                        }
                    });
                }
            },
            cancel: {
                text: 'Hủy'
            }
        }
    });
}
function handleCategory() {
    $('.category-form').validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-block', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            name: {
                required: true,
                minlength: 5
            }
        },

        messages: {
            name: {
                required: "Nội dung không được để trống.",
                minlength: "Tối thiểu 5 ký tự."
            }

        },

        invalidHandler: function (event, validator) { //display error alert on form submit   

        },

        highlight: function (element) { // hightlight error inputs
            $(element)
                .closest('.form-group').addClass('has-error'); // set error class to the control group
        },

        success: function (label) {
            label.closest('.form-group').removeClass('has-error');
            label.remove();
        },

        submitHandler: function (form) {
            categoryModel = {
                id: $(form.id).val(),
                categoryName: $(form.name).val(),
                categoryParentId: $(form.parent).val(),
                show: $(form.show).prop("checked")
            }
            var insert = true;
            var url = "/admin/category/insert";
            if (categoryModel.id) {
                insert = false;
                url = "/admin/category/update";
            }
            $.when(Vanp.handleAjaxPost({ url: url, params: { categoryModel: categoryModel }, event: form.btnSubmit })).done(function (data) {
                if (data != null) {
                    if (insert) {
                        requestTable.row.add(data.data).draw();
                    } else {
                        requestTable.row(".row_" + categoryModel.id).data(data.data).draw();
                    }
                    Vanp.notify(data.error, data.message);
                    $modal.modal("hide");
                }
            });
        }

    });

    $('.category-form input').keypress(function (e) {
        if (e.which == 13) {
            if ($('.category-form').validate().form()) {
                $('.category-form').submit();
            }
            return false;
        }
    });
}
function resetFrom() {
    $modal.on('show.bs.modal', function () {
        var data = $.map(requestTable.rows().data(), function (e) { return { id: e.Id, text: e.CategoryName } });
        data.unshift({ id: '', text: 'Rỗng' });
        categorySelect2.select2( { data: data, width: null});
        $('.category-form .id').val("");
        $('.category-form .name').val("");
        $('.category-form .parent').val("");
        $('.category-form .show').prop("checked", true);
    });
}
$.fn.select2.defaults.set("theme", "bootstrap");