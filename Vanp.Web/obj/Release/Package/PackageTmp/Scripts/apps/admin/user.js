var requestTable = $("#table-main").DataTable({
    ajax: {
        url: "/Admin/User/GetList",
        dataSrc: "data"
    },
    pagingType: "bootstrap_full_number",
    columns: [
            {
                data: "Id", render: function (data, type, row, meta) {
                    return '<button type="button" class="btn dark btn-outline" onclick=resetpassword(this,' + data + ')>Reset</button>';
                }
            },
            {
                data: "Id", render: function (data, type, row, meta) {
                    return '<button type="button" class="btn red btn-outline" onclick=deleteuser(this,' + data + ')>Xóa</button>';
                }
            },
            { data: "UserName" },
            { data: "Email" },
            { data: "VoteUp" },
            { data: "VoteDown" },
            {
                data: "LastLogon", render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY hh:mm:ss");
                }
            },
            {
                data: "CreatedWhen", render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY hh:mm:ss");
                }
            },

            {
                data: "Authorized", render: function (data, type, row) {
                    if (data) {
                        return '<span class="label label-sm label-info"> Đã Xác Thực </span>';
                    } else {
                        return '<span class="label label-sm label-warning"> Chưa Xác Thực </span>';
                    }
                }
            }

    ],
    fnRowCallback: function (nRow, aData, iDisplayIndex) {
        $(nRow).addClass("row_" + aData["Id"]);
    },
});
//Duyệt bán hàng
function resetpassword(event, id) {
    $.when(Vanp.handleAjaxPost({ url: "/Admin/User/ResetPassWord", params: { userId: id }, event: event })).done(function (data) {
        if (data != null) {
            Vanp.notify(data.error, data.message);
        }
    });
}
function deleteuser(event, id) {
    $.confirm({
        title: 'Xác Nhận!',
        content: 'Bạn chắc chắn muốn xóa người dùng này!',
        buttons: {
            confirm: {
                text: 'Xác Nhận',
                btnClass: 'btn-blue',
                action: function () {
                    $.when(Vanp.handleAjaxPost({ url: "/Admin/User/Delete", params: { userId: id }, event: event })).done(function (data) {
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
    }
    );

}