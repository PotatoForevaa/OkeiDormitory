$(document).ready(function () {
    $("#files").on("change", function (event) {
        $(".image-preview").empty();
        const files = event.target.files;

        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            const reader = new FileReader();

            reader.onload = function (e) {
                const img = $('<img>').attr('src', e.target.result).css('width', '100px').css('margin', '5px');
                $('.image-preview').append(img);
            }

            reader.readAsDataURL(file);
        }
    });

    $('.inspection form input[name="rating"]').change(function () {
        $(".inspection .error").hide();
    });

    $(".inspection form").submit(function (event) {
        event.preventDefault();
        if ($('.rating input[name="rating"]:checked').length === 0) {
            $(".inspection .error").show();
            return;
        }

        var formData = new FormData(this);

        $.ajax({
            url: '/api/Inspection',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                alert("ураа");
            },
            error: function (xhr, status, error) {
                alert("Ошибка при отправке формы: " + error);
            }
        });
    });
});