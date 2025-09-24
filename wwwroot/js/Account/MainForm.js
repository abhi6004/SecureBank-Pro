// Tab switching function
function showSection(sectionClass) {
    $(".withdraw, .deposit, .transfer").hide();
    $("." + sectionClass).show();
}

$(function () {

    // Form submit handler (delegated)
    $(document).on("submit", "form", function (e) {
        e.preventDefault();
        const $form = $(this);
        const type = $form.find("input[name='Type']").val() || null;

        if (type === "deposit") {
            let currencyVal = $form.find(".currency").val();
            let amountVal = $form.find(".deposit-amount").val();
            if (!currencyVal || !amountVal) return;
        }

        let postData = {
            Amount: $form.find(".deposit-amount, input[name='Amount']").val() || null,
            RecipientId: $form.find("input[name='RecipientId']").val() || null,
            Type: type,
            Email: $form.find("input[name='Email']").val() || null,
            Description: $form.find("input[name='Description']").val() || null
        };

        if (type === "deposit" && $form.find(".currency").val() !== "INR") {
            postData.Amount = $form.find(".conversionAmount").val();
        }

        $.ajax({
            url: "/Account/Transaction",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(postData),
            success: function (data) {
                const $customerDiv = $(`#customer_${data}`).closest(".customers");
                $customerDiv.find(".Bank_transactions").hide();
                $customerDiv.find(`#customer_${data}`).show();
            }
        });
    });

    // Currency change
    $(document).on("change", ".currency", function () {
        const $form = $(this).closest("form");
        const code = $(this).val();
        const amount = $form.find(".deposit-amount").val();

        if (code === "INR") {
            $form.find(".conversionAmount").hide();
        } else {
            $.ajax({
                url: "/Account/ConvertCurrency",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ Amount: amount, CurrencyCode: code }),
                success: function (data) {
                    $form.find(".conversionAmount").val(data.conversionAmount).show();
                }
            });
        }
    });

    // Deposit amount input
    $(document).on("input", ".deposit-amount", function () {
        const $form = $(this).closest("form");
        if ($form.find(".currency").val() === "INR") return;

        $.ajax({
            url: "/Account/ConvertCurrencyLatest",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Amount: $(this).val(),
                CurrencyCode: $form.find(".currency").val()
            }),
            success: function (data) {
                $form.find(".conversionAmount").val(data.conversionAmount).show();
            }
        });
    });

});
