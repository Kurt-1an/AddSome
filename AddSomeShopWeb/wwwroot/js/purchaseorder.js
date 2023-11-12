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
            <td><input type="number" class="form-control cost" name="cost" placeholder="Cost"></td>
            <td><input type="number" class="form-control qty" name="qty" placeholder="Qty"></td>
            <td><button type="button" class="btn btn-danger remove">Remove</button></td>
            <td><button type="button"  class="btn btn-primary add">Add</button></td>
        </tr>
    `;

        // Append the row to the table
        $('#selectedProducts').append(row);
    });


    //DINAGDAG
    var totalAmount = 0;

    // Use delegated event handlers to handle dynamically added elements
    $('#selectedProducts').on('click', '.add', function () {
        console.log('Add button clicked');

        var row = $(this).closest('tr');
        var costInput = row.find('.cost');
        var qtyInput = row.find('.qty');

        var cost = parseFloat(costInput.val()) || 0;
        var qty = parseFloat(qtyInput.val()) || 0;

        console.log('Cost:', cost);
        console.log('Quantity:', qty);

        var total = cost * qty;

        console.log('Total:', total);

        updateMainTotal(total);
    });

    $('#selectedProducts').on('click', '.remove', function () {
        var row = $(this).closest('tr');
        var cost = parseFloat(row.find('.cost').val()) || 0;
        var qty = parseFloat(row.find('.qty').val()) || 0;
        var total = cost * qty;

        updateMainTotal(-total);

        row.remove();
    });

    function updateMainTotal(amount) {
        var mainTotal = parseFloat($('#totaldisplay').val()) || 0;
        console.log('Current Main Total:', mainTotal);

        mainTotal += amount;
        console.log('New Main Total:', mainTotal);

        $('#totaldisplay').val(mainTotal.toFixed(2));
    }

});