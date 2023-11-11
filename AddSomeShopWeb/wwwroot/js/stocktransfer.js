$(document).ready(function () {
    $('#productSearch').select2({
        ajax: {
            url: '/Admin/StockTransfer/GetProducts', 
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

        var imageUrl = data.img; 
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

    $('#productSearch').on('select2:select', function (e) {
        var selectedProduct = e.params.data;

        var imageUrl = selectedProduct.img;
        var completeImageUrl = window.location.origin + imageUrl;

        $.ajax({
            url: '/Admin/StockTransfer/GetProductDetails',
            type: 'GET',
            data: { productName: selectedProduct.text },
            success: function (productDetails) {
                var row = `
            <tr>
                <td>
                    <img src="${completeImageUrl}" style="height: 60px; width: 60px; margin-right: 10px;" />
                </td>
                <td>${selectedProduct.text}</td>
                <td>${productDetails.costPrice}</td>
                <td>${productDetails.stockQuantity}</td>
                <td><input type="number" class="form-control" name="qtytotransfer" placeholder="Qty. to transfer"></td>
                <td><button class="btn btn-danger btn-remove">Remove</button></td>
                <td><button class="btn btn-primary btn-add">Add</button></td>
            </tr>
        `;

                $('#selectedProducts').append(row);
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
    var totalAmount = 0;

    $('#selectedProducts').on('click', '.btn-add', function (e) {
        e.stopPropagation(); 
        e.preventDefault(); 

        var row = $(this).closest('tr');
        var costPrice = parseFloat(row.find('td:eq(2)').text());
        var oldQuantity = parseFloat(row.find('input[name="qtytotransfer"]').data('old-value')) || 0; 
        var newQuantity = parseFloat(row.find('input[name="qtytotransfer"]').val());

        totalAmount -= costPrice * oldQuantity;
        var productTotal = costPrice * newQuantity;

        totalAmount += productTotal;

        $('#totaldisplay').val(totalAmount.toFixed(2));

        row.find('input[name="qtytotransfer"]').data('old-value', newQuantity);
    });

    $('#selectedProducts').on('click', '.btn-remove', function () {
        var row = $(this).closest('tr');
        var costPrice = parseFloat(row.find('td:eq(2)').text());
        var oldQuantity = parseFloat(row.find('input[name="qtytotransfer"]').data('old-value')) || 0; 

        totalAmount -= costPrice * oldQuantity;

        $('#totaldisplay').val(totalAmount.toFixed(2));

        row.remove();
    });
});







