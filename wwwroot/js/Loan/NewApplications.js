$(function () {
    $(".btn.btn-info.more-info").on("click", function () {

        let id = $(this).attr("id");

        $("#details-area").load("/LoanApplications/Details/" + id);
        $("#applications-list").hide();
        $("#details-area").show();
        $("#applications-history").hide();
    });

})

$(function () {
    $('.ApplicationButton').on('click', function (e) {
        e.preventDefault();
        var form = $(this);
        var _class = form.attr('class');
        var status = ""

        if (_class.includes('btn-success')) {
            status = "Approved"
        }
        else if (_class.includes('btn-danger')) {
            status = "Denied"
        }

        var id = form.closest('td').children('.btn.btn-info.more-info').attr('id');

        var data = {
            id: id,
            status: status
        };

        $.ajax({
            type: "POST",
            url: "/LoanApplications/ApplicationProcess",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    alert("Application status updated successfully.");
                    location.reload();
                } else {
                    alert("Failed to update application status.");
                }
            },
            error: function () {
                alert("An error occurred while updating the application status.");
            }
        });
    });
})

$(function () {

    $("#ApplicationsHistoryHeading").on("click", function () {

        var listHeading = $("#applications-history").attr("style") || "";

        if (listHeading.includes("none")) {
            $("#applications-history")
                .load("/LoanApplications/HistoryOfApplication")
                .show();

            $("#applications-list").hide();
            $("#details-area").hide();
        }
    });

    $("#ApplicationsHeading").on("click", function () {

        var listHeading = $("#applications-list").attr("style") || "";

        if (listHeading.includes("none")) {
            $("#applications-history").hide();
            $("#applications-list").show();
            $("#details-area").hide();
        }
    });

});


$(function () {

    // ACCEPT
    $(".btn.btn-success.btn-lg").on("click", function () {

        let id = $(this).closest("div").siblings("h2").attr("id");
        let status = "Approved";

        let data = {
            id: id,
            status: status
        };

        $.ajax({
            type: "POST",
            url: "/LoanApplications/ApplicationProcess",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    alert("Application status updated successfully.");
                    location.href = "/LoanApplications/NewApplications";
                } else {
                    alert("Failed to update application status.");
                }
            },
            error: function () {
                alert("An error occurred while updating the application status.");
            }
        });
    });


    // REJECT
    $(".btn.btn-danger.btn-lg.ms-3").on("click", function () {

        let id = $(this).closest("div").siblings("h2").attr("id");
        let status = "Denied";

        let data = {
            id: id,
            status: status
        };

        $.ajax({
            type: "POST",
            url: "/LoanApplications/ApplicationProcess",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    alert("Application status updated successfully.");
                    location.href = "/LoanApplications/NewApplications";
                } else {
                    alert("Failed to update application status.");
                }
            },
            error: function () {
                alert("An error occurred while updating the application status.");
            }
        });
    });

});



