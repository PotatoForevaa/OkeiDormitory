$(document).ready(function () {
    $(".user-list-header").click(function (event) {
        $(".users-info table").toggle();
    });

    $(".qr-download-header").click(function (event) {
        $(".qr-download form").toggle();
    });

    $(".qr-download form").submit(function (event) {
        event.preventDefault();
        const roomNumber = $(this).find("input").val();
        window.location.href = `/api/Qr/DownloadQr?roomNumber=${roomNumber}`;
    });
});