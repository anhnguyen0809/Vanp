var requestTable = $("#table-main").DataTable({
    ajax: {
        url: "/Admin/Request/GetListNotApproved",
        dataSrc: "data"
    },
    pagingType: "bootstrap_full_number",
    columns: [
            {
                data: "Id", render: function (data, type, row, meta) {
                    return '<button type="button" class="btn blue btn-outline" onclick=approved(this,' + data + ')>Duyệt</button>';
                }
            },
            { data: "CreatedByName" },
            {
                data: "CreatedWhen", render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY hh:mm:ss");
                }
            },
            { data: "RequestContent" },
            {
                data: "DateFrom", render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY hh:mm:ss");
                }
            },
            {
                data: "DateTo", render: function (data, type, row) {
                    return moment(data).format("DD/MM/YYYY hh:mm:ss");
                }
            }
    ],
    fnRowCallback: function (nRow, aData, iDisplayIndex) {
        $(nRow).addClass("row_" + aData["Id"]);
    },
});
//Duyệt bán hàng
function approved(event, id) {
    $.when(Vanp.handleAjaxPost({ url: "/Admin/Request/Approved", params: { requestId: id }, event: event })).done(function (data) {
        if (data != null) {
            if (data.error === 0) {
                requestTable.rows(".row_" + id).remove().draw();;
            }
            Vanp.notify(data.error, data.message);
        }
    });
}