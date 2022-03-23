$(() => {
    $("#new-contributor").on("click", function () {
        console.log("I'm here and working")
        $("#initial-Deposit-Div").show();
        $("#date-Div").show();
        $(".new-contrib").modal();
    });

    $(".new-contrib").on('shown.bs.modal', function () {
        $("#contributor_first_name").focus()
    });

    $(".edit-contrib").on("click", function () {
        const id = $(this).data('id');
        const firstName = $(this).data('first-name');
        const lastName = $(this).data('last-name');
        const amount = $(this).data('amount');
        const cell = $(this).data('cell');
        const alwaysInclude = $(this).data('always-include');
        const form = $(".contributor-form");

        $("#contributor_first_name").val(firstName);
        $("#contributor_last_name").val(lastName);
        $("#contributor_amount").val(amount);
        $("#contributor_cell").val(cell);

        form.find("#edit-id").remove();
        const hidden = $(`<input type='hidden' id='edit-id' name='id' value='${id}' />`);
        form.append(hidden);

        $("#contributor_always_include").prop('checked', alwaysInclude === "True");
        $("#date-Div").hide();
        $("#initial-Deposit-Div").hide();
        form.attr('action', "/contributors/update")
        $(".new-contrib").modal();
    });

    $(".deposit-button").on("click", function () {
        const id = $(this).data('contributor-id');
        const firstName = $(this).data('first-name-deposit');
        const lastName = $(this).data('last-name-deposit');
       
        $("#deposit-name").text(`${firstName} ${lastName}`);
        $('[name="contributorId"]').val(id);

        $(".deposit").modal();
    });

});