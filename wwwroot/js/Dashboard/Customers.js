// Navigation functions
function EdituserForm(email) {
    window.location.href = "/Dashboard/EditUsers?email=" + encodeURIComponent(email);
}

function DeletedUser() { }

function ViewUser(email) {
    window.location.href = "/Dashboard/CustomerProfile?email=" + encodeURIComponent(email);
}

// Delegated click handler for dynamically loaded .Transaction buttons
$(function () {
    $(document).on("click", ".Transaction", function () {

        // Reset all customers
        $(".customers_data").show();
        $(".Bank_transactions").hide();

        // Current containers
        const $customersContainer = $(this).closest(".customers_data");
        const $transactionsContainer = $customersContainer.siblings(".Bank_transactions");

        // Get email from second <p>
        const email = $customersContainer.find("p:nth-child(2)").text().replace("Email: ", "");

        // Load transaction form via AJAX
        $.get("/Account/TransactionForm", { email: email }, function (data) {
            $customersContainer.hide();           // hide current customer data
            $transactionsContainer.html(data).show(); // show form
        });
    });
});
