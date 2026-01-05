// Navigation functions
function EdituserForm(email) {
    window.location.href = "/Dashboard/EditUsers?email=" + encodeURIComponent(email);
}

function ViewUser(email) {
    window.location.href = "/Dashboard/CustomerProfile?email=" + encodeURIComponent(email);
}

function DeletedUser() {
    alert("Implement delete logic here");
}

// ---- Transaction Button ----
$(document).on("click", ".Transaction", function () {

    const email = $(this).data("email");

    $.get("/Account/TransactionForm", { email: email }, function (data) {

        $("#transactionModalBody").html(data);

        $("#transactionModal").modal("show");
    });
});

// ---- Upload Button ----
$(document).on("click", ".Upload", function () {

    const id = $(this).data("id");

    window.location.href = "/Dashboard/UploadDocuments?id=" + id;
});
