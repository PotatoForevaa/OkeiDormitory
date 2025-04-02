$(".nav-button").click(function () {
    var link = $(this).data("link");
    window.location.href = `${window.location.origin}/home/${link}`;
});