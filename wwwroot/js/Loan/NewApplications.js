$(function () {
    $(".btn.btn-info.more-info").on("click", function () {

        let id = $(this).attr("id");

        $("#details-area").load("/LoanApplications/Details/" + id);
        $("#applications-list").hide();
        $("#details-area").show();
    });

})