$(() => {
    $("#new-simcha").on("click", function () {       
        $(".new-simcha").modal();
    });

    $(".new-simcha").on('shown.bs.modal', function () {
        $("#simcha_name").focus()
    });

});