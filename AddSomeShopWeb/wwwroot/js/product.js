var dataTable;

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'productName', "width": "10%" },
            { data: 'barcode', "width": "15%" },
            { data: 'costPrice', "width": "5%" },
            { data: 'retailPrice', "width": "5%" },
            { data: 'stockQuantity', "width": "10%" },
            { data: 'supplier.supplierCompanyName', "width": "10%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-outline-dark btn-sm"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick="Delete('/admin/product/delete/${data}')" class="btn btn-outline-danger btn-sm"><i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`;
                },
                "width": "15%"
            }
        ]
    });
}

$(document).ready(function () {
    loadDataTable();
});

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}

$("#btngenerate").attr("href", `/Admin/Product/GeneratePdf`);
