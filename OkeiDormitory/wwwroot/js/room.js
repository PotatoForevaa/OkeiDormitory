$(document).ready(function () {
    $(".add-reward form").submit(function (event) {
        event.preventDefault();

        var formData = new FormData(this);

        $.ajax({
            url: `/api/room/${$("#roomNumber").val()}/reward`,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            error: function (xhr, status, error) {
                console.log("Ошибка при отправке формы: " + error);
            }
        });
    });
});