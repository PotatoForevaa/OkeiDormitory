$(document).ready(function () {
    console.log("навбар скрипт загружен");
    $("header button").click(function () {
        var link = $(this).data("link");
        console.log(link + " кнопка бам");
        window.location.href = `${window.location.origin}/home/${link}`;
    });
});
