$(document).ready(function () {

    controlLayout();

    $(window).bind('scroll', function (e) {
        controlLayout();
    });

    $(window).resize(function () {
        controlLayout();
    });
});


function controlLayout() {
}

