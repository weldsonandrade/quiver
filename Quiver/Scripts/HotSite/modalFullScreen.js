// .modal-backdrop classes

$(".modal-transparent").on('show.bs.modal', function () {
  setTimeout( function() {
    $(".modal-backdrop").addClass("modal-backdrop-transparent");
  }, 0);
});
$(".modal-transparent").on('hidden.bs.modal', function () {
  $(".modal-backdrop").addClass("modal-backdrop-transparent");
});

$(".modal-fullscreen").on('show.bs.modal', function () {
  setTimeout( function() {
    $(".modal-backdrop").addClass("modal-backdrop-fullscreen");
  }, 0);
});
$(".modal-fullscreen").on('hidden.bs.modal', function () {
  $(".modal-backdrop").addClass("modal-backdrop-fullscreen");
});


var headerHeight = $("#modal-header-full-screen").height();

$("#body-full-screen").css('max-height', (getHeightNavigator() - 99) + "px");
$("#body-full-screen").css('min-height', (getHeightNavigator() - 99) + "px");


window.onresize = function (event) {

    $("#body-full-screen").css('max-height', (getHeightNavigator() - 99) + "px");
    $("#body-full-screen").css('min-height', (getHeightNavigator() - 99) + "px");


}
