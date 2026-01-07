function showSection(section) {
    $(".withdraw, .deposit, .transfer").hide();
    $("." + section).show();
}

$(function () {

    $(document).off("submit", "form").on("submit", "form", function (e) {
        e.preventDefault();

        const f = $(this);

        let body = {
            Amount: f.find(".amount").val(),
            RecipientId: f.find(".recipient").val(),
            Type: f.find("input[name='Type']").val(),
            Email: f.find("input[name='Email']").val(),
            Description: f.find(".desc").val()
        };

        if (body.Type === "deposit" && f.find(".currency").val() !== "INR")
            body.Amount = f.find(".conversionAmount").val();

        $.ajax({
            url: "/Account/Transaction",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(body),
            success: function () {
                alert("Transaction Success");
                $("#transactionModal").modal("hide");
                $("form")[0].reset();
            }
        });

    });

    $(document).off("change", ".currency").on("change", ".currency", function () {

        const f = $(this).closest("form");
        const code = $(this).val();
        const amount = f.find(".amount").val();

        if (code === "INR") {
            f.find(".conversionAmountWrapper").hide();
            return;
        }

        $.post({
            url: "/Account/ConvertCurrencyLatest",
            contentType: "application/json",
            data: JSON.stringify({ Amount: amount, CurrencyCode: code }),
            success: function (res) {
                f.find(".conversionAmount").val(res.conversionAmount);
                f.find(".conversionAmountWrapper").show();
            }
        });
    });

    $(document).off("input", ".amount").on("input", ".amount", function () {

        const f = $(this).closest("form");
        const amount = $(this).val();

        // If this form doesn't have currency (withdraw / transfer) → STOP
        if (f.find(".currency").length === 0) {
            return;
        }

        // if blank, clear
        if (!amount) {
            f.find(".conversionAmount").val("");
            f.find(".conversionAmountWrapper").hide();
            return;
        }

        // if INR no API
        if (f.find(".currency").val() === "INR") {
            f.find(".conversionAmount").val(amount);
            f.find(".conversionAmountWrapper").show();
            return;
        }

        $.post({
            url: "/Account/ConvertCurrencyLatest",
            contentType: "application/json",
            data: JSON.stringify({
                Amount: amount,
                CurrencyCode: f.find(".currency").val()
            }),
            success: function (res) {

                f.find(".conversionAmount").val(res.conversionAmount);
                f.find(".conversionAmountWrapper").show();
            }
        });

    });


});
