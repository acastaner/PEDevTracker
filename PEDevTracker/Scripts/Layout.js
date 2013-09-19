$(document).ready(function () {

    $("body").css("display", "none");
    $("body").fadeIn(1500);

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

