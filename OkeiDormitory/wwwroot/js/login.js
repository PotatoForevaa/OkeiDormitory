$(document).ready(function () {
    $(".login form").submit(function (event) {
        event.preventDefault();

        var formData = {
            login: $("#login").val(),
            password: $("#password").val()
        };

        $.ajax({
            url: "api/Authentication/login",
            type: "POST",
            contentType: "application/JSON",
            data: JSON.stringify(formData),
            success: function (response) {
                window.location.href = "/home";
            },
            error: function (xhr, status, error) {
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    alert("Ошибка: " + xhr.responseJSON.message);
                } else {
                    alert("Произошла ошибка: " + error);
                }
            }
        });
    });
});
