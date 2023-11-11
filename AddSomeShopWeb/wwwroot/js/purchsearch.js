//$(document).ready(function () {
//    $('#productSearch').select2({
//        ajax: {
//            url: '/Admin/PurchaseOrder/GetProducts', // Update the URL to the GetProducts action
//            dataType: 'json',
//            delay: 250,
//            data: function (params) {
//                return {
//                    term: params.term
//                };
//            },
//            processResults: function (data) {
//                return {
//                    results: data
//                };
//            },
//            cache: true
//        },
//        placeholder: 'Search product...',
//        minimumInputLength: 1,
//        templateResult: formatResults

//    });

//});


//function formatResults(data) {
//    if (data.loading)
//        return data.text;

//    var imageUrl = data.img; // Assuming data.img contains the relative image path from the wwwroot folder

//    // Construct the complete image URL by prepending the application base URL
//    var completeImageUrl = window.location.origin + imageUrl;


//    var container = $(
//        `<table width="100%">
//            <tr>
//                <td style="width:60px">
//                    <img style="height:60px;width:60px;margin-right:10px" src="${completeImageUrl}"/>
//                </td>
//                <td>
//                    <p style="font-weight: bolder;margin:2px">${data.text}</p>
//                </td>
//            </tr>
//         </table>`
//    );

//    return container;
//}


//$(document).on('select2:open', () => {
//    document.querySelector('.select2-search__field').focus();
//});


$(document).ready(function () {
    $('#productSearch').select2({
        ajax: {
            url: '/Admin/PurchaseOrder/GetProducts', // Update the URL to the GetProducts action
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    term: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data
                };
            },
            cache: true
        },
        placeholder: 'Search product...',
        minimumInputLength: 1,
        templateResult: formatResults
    });

    function formatResults(data) {
        if (data.loading)
            return data.text;

        var imageUrl = data.img; // Assuming data.img contains the relative image path from the wwwroot folder

        // Construct the complete image URL by prepending the application base URL
        var completeImageUrl = window.location.origin + imageUrl;

        var container = $(
            `<table width="100%">
                <tr>
                    <td style="width:60px">
                        <img style="height:60px;width:60px;margin-right:10px" src="${completeImageUrl}"/>
                    </td>
                    <td>
                        <p style="font-weight: bolder;margin:2px">${data.text}</p>
                    </td>
                </tr>
             </table>`
        );

        return container;
    }

    $(document).on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });

    // Handle product selection and display in the table
    $('#productSearch').on('select2:select', function (e) {
        var selectedProduct = e.params.data;

        // Get the image URL
        var imageUrl = selectedProduct.img; // Assuming data.img contains the relative image path from the wwwroot folder
        var completeImageUrl = window.location.origin + imageUrl;

        // Add the selected product to the table with input fields for cost and quantity
        var row = `
        <tr>
            <td>
                <img src="${completeImageUrl}" style="height: 60px; width: 60px; margin-right: 10px;" />
            </td>
            <td>${selectedProduct.text}</td>
            <td><input type="number" class="form-control" name="cost" placeholder="Cost"></td>
            <td><input type="number" class="form-control" name="qty" placeholder="Qty"></td>
            <td><button class="btn btn-danger">Remove</button></td>
        </tr>
    `;

        // Append the row to the table
        $('#selectedProducts').append(row);
    });


});

