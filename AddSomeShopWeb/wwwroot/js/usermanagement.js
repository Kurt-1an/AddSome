$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblusermanagementData').DataTable({
        "ajax": { url: '/admin/usermanagement/getall'},
        "columns": [
            {
                data: null,
                render: function (data, type, row) {
                    // Combine lastName and firstName into one column
                    return data.lastName + ' ' + data.firstName;
                },
                "width": "10%"
            },
            { data: 'formattedID', "width": "10%" },
            { data: 'email', "width": "10%" },
            { data: 'accessLevel', "width": "10%" },
            { data: 'dateCreated', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
            <a href="/admin/usermanagement/edit?id=${data}" class="btn btn-outline-dark btn-sm"> <i class="bi bi-pencil-square"></i> Edit</a>
            <a href="/admin/usermanagement/delete?id=${data}" class="btn btn-outline-danger btn-sm"><i class="bi bi-trash-fill"></i> Delete</a>
        </div>`
                },
                "width": "15%"
            }
        ]
    });
}

