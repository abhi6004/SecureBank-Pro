$(function () {

    /* ===============================
       TAB SWITCHING
    ================================ */

    $("#tab-new").on("click", function () {
        $(this).addClass("active");
        $("#tab-history").removeClass("active");

        $("#applications-list").show();
        $("#applications-history").hide();
        $("#details-area").hide();
    });

    $("#tab-history").on("click", function () {
        $(this).addClass("active");
        $("#tab-new").removeClass("active");

        $("#applications-list").hide();
        $("#details-area").hide();

        $("#applications-history")
            .load("/LoanApplications/HistoryOfApplication")
            .show();
    });


    /* ===============================
       MORE INFO (DETAILS VIEW)
    ================================ */
    $(document).on("click", ".btn.btn-info.more-info", function () {

        let id = $(this).attr("id");

        $("#details-area")
            .load("/LoanApplications/Details/" + id)
            .show();

        $("#applications-list").hide();
        $("#applications-history").hide();

        // remove active tab highlight
        $(".app-tab").removeClass("active");
    });


    /* ===============================
       ACCEPT / REJECT (TABLE)
    ================================ */
    $(document).on("click", ".ApplicationButton", function (e) {
        e.preventDefault();

        let btn = $(this);
        let status = "";

        if (btn.hasClass("btn-success")) {
            status = "Approved";
        }
        else if (btn.hasClass("btn-danger")) {
            status = "Denied";
        }

        let id = btn
            .closest("td")
            .find(".btn.btn-info.more-info")
            .attr("id");

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


    /* ===============================
       ACCEPT / REJECT (DETAILS PAGE)
    ================================ */

    $(document).on("click", ".btn.btn-success.btn-lg", function () {

        let id = $(this).closest("div").siblings("h2").attr("id");

        processApplication(id, "Approved");
    });

    $(document).on("click", ".btn.btn-danger.btn-lg.ms-3", function () {

        let id = $(this).closest("div").siblings("h2").attr("id");

        processApplication(id, "Denied");
    });


    function processApplication(id, status) {

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
    }

});
