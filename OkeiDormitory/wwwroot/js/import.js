$(document).ready(function () {
    $(".import-header").click(function (event) {
        $(".import form").toggle();
    });

    $(".import form").submit(function (event) {
        event.preventDefault();

        var formData = new FormData(this);

        $.ajax({
            url: '/api/Import/Users',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            error: function (xhr, status, error) {
                console.log("Ошибка при отправке формы: " + error);
            }
        });
    }); 
});