$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblpurchaseOrderData').DataTable({
        "ajax": { url: '/admin/purchaseorder/getall'},
        "columns": [
            { data: 'supplierName', "width": "15%" },
            { data: 'paymentTerm', "width": "10%" },
            { data: 'expectedDeliveryDate', "width": "10%" },
            { data: 'employeeName', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
            <a href="/admin/PurchaseOrder/details?id=${data}" class="btn btn-outline-dark btn-sm"> <i class="bi bi-eye"></i></a>
            <a href="/admin/PurchaseOrder/delete?id=${data}" class="btn btn-outline-danger btn-sm"><i class="bi bi-trash-fill"></i> Delete</a>
        </div>`
                },
                "width": "15%"
            }
        ]
    });
}

