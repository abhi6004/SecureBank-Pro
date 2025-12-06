$(function () {
    $(document).on("click", "button", function () {

        var btnClass = $(this).attr("class");
        if (btnClass != "btn btn-primary") return;

        var btn = $(this);
        var id = $(this).closest("td").closest("tr").attr("id");

        var data = {
            offerId: id,
            loanType: "StaticLoan"
        };

        $.ajax({
            url: "/Loan/ApplyLoan",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function (response) {
                btn.attr("class", "btn btn-warning");
                btn.text("Applied");
            },
            error: function () {
                alert("Error selecting offer.");
            }
        });
    });

    $(document).on("click", "h2", function () {
        let id = $(this).attr("id");

        if (id === "all-Offers") {
            $("#loan-offers").show();
            $("#loan-applications").hide();
        }
        else if (id === "loan-History") {
            $.ajax({
                url: "/Loan/UpdateHistory",
                type: "POST",
                success: function (res) {
                    $("#loan-applications-content").load(location.href + " #loan-applications-content > *");
                    $("#loan-applications").show();
                    $("#loan-offers").hide();
                }
            });
        }
    });
});
