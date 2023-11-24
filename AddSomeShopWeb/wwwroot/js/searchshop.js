$(function () {
    $("#basic-text1").click(function (event) {
        event.preventDefault();
        $("form").submit();
    });
});