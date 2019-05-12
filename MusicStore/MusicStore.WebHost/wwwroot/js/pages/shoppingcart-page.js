$(function () {
    // Document.ready -> link up remove event handler
    $(".RemoveLink").click((elem) => {
        var recordDelete = $(elem.currentTarget).attr("data-id");
        if (recordDelete !== '') {

            const defaultHeaders = {
                "Accept": "application/json",
                "Content-Type": "application/json"
            };

            const ajaxOptions = {
                method: 'DELETE',
                headers: defaultHeaders
            };

            fetch(`api/removeFromCart/${recordDelete}`, ajaxOptions)
                .then(response => response.json())
                .then(data => {
                    // Successful requests get here
                    // Update the page elements
                    if (data.itemCount === 0) {
                        $('#row-' + data.deleteId).fadeOut('slow');
                    }
                    else {
                        $('#item-count-' + data.deleteId).text(data.itemCount);
                    }
                    $('#cart-total').html("");
                    $('#update-message').text(data.message);
                    $('#cart-status').text('Cart (' + data.cartCount + ')');

                    fetch(`api/getTotal`)
                        .then(response => response.text())
                        .then(text => {
                            $('#cart-total').html(text);
                        })
                        .catch(error => console.log('something went wrong calling getTotal', error));
                })
                .catch(error => console.log('something went wrong calling removeFromCart', error));
        }
    });
});
function handleUpdate() {
    // Load and deserialize the returned JSON data
    var json = context.get_data();
    var data = JSON.parse(json);
    // Update the page elements
    if (data.ItemCount === 0) {
        $('#row-' + data.DeleteId).fadeOut('slow');
    } else {
        $('#item-count-' + data.DeleteId).text(data.ItemCount);
    }
    $('#cart-total').text(data.CartTotal);
    $('#update-message').text(data.Message);
    $('#cart-status').text('Cart (' + data.CartCount + ')');
}