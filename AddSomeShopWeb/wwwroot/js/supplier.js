$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblsupplierData').DataTable({
        "ajax": { url: '/admin/supplier/getall'},
        "columns": [
            { data: 'supplierCompanyName', "width": "15%" },
            { data: 'supplierEmail', "width": "10%" },
            { data: 'supplierContactNumber', "width": "10%" },
            {
                data: null,
                render: function (data, type, row) {
                    // Combine city and province into one column
                    return data.supplierCity + ' ' + data.supplierProvince;
                },
                "width": "10%"
            },
            { data: 'supplierStatus', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
            <a href="/admin/supplier/edit?id=${data}" class="btn btn-outline-dark btn-sm"> <i class="bi bi-pencil-square"></i> Edit</a>
            <a href="/admin/supplier/delete?id=${data}" class="btn btn-outline-danger btn-sm"><i class="bi bi-trash-fill"></i> Delete</a>
        </div>`
                },
                "width": "15%"
            }
        ]
    });
}

