$(function () {

    /* ===============================
       APPLY LOAN (UNCHANGED LOGIC)
    ================================ */
    $(document).on("click", "button", function () {

        var btnClass = $(this).attr("class");
        if (btnClass !== "btn btn-primary") return;

        var btn = $(this);
        var id = btn.closest("tr").attr("id");

        var data = {
            offerId: id,
            loanType: "StaticLoan"
        };

        $.ajax({
            url: "/Loan/ApplyLoan",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function () {
                btn.removeClass("btn-primary")
                    .addClass("btn btn-warning")
                    .text("Applied");
            },
            error: function () {
                alert("Error selecting offer.");
            }
        });
    });


    /* ===============================
       TAB SWITCHING
    ================================ */
    $(".loan-tab").on("click", function () {

        $(".loan-tab").removeClass("active");
        $(this).addClass("active");

        let id = $(this).attr("id");

        if (id === "all-Offers") {
            $("#loan-offers").show();
            $("#loan-applications").hide();
        }
        else if (id === "loan-History") {
            $("#loan-applications")
                .load("/Loan/UpdateHistory")
                .show();

            $("#loan-offers").hide();
        }
    });

});
