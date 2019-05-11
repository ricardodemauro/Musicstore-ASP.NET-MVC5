$(function () {
    // Document.ready -> link up remove event handler
    $(".RemoveLink").click((elem) => {
        var recordDelete = $(elem.currentTarget).attr("data-id");
        if (recordDelete !== '') {

            const defaultHeaders = {
                "Accept": "application/json",
                "Content-Type": "application/json"
            };

            const body = { id: recordDelete };

            const ajaxOptions = {
                method: 'POST',
                headers: defaultHeaders,
                body: JSON.stringify(body)
            };

            fetch("/ShoppingCart/RemoveFromCart", ajaxOptions)
                .then(response => response.json())
                .then(data => {
                    // Successful requests get here
                    // Update the page elements
                    if (data.ItemCount === 0) {
                        $('#row-' + data.DeleteId).fadeOut('slow');
                    }
                    else {
                        $('#item-count-' + data.DeleteId).text(data.ItemCount);
                    }
                    $('#cart-total').text(data.CartTotal);
                    $('#update-message').text(data.Message);
                    $('#cart-status').text('Cart (' + data.CartCount + ')');
                })
                .catch(error => console.log('deu alguma coisa errada', error));
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