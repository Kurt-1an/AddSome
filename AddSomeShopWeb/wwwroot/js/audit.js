var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/auditlog/getall' },
        "columns": [
            { data: 'userName', "width": "10%" },
            { data: 'role', "width": "7%" },
            { data: 'changes', "width": "10%" },
            { data: 'action', "width": "7%" },
            { data: 'formattedTime', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/order/details?orderId=${data}" class="btn btn-outline-primary btn-sm"> <i class="bi bi-pencil-square"></i></a>
                            </div>`
                },
                "width": "10%"
            }
        ]
    });
}

