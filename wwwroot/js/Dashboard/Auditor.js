function EdituserForm(email) {
    window.location.href = "/Dashboard/EditUsers?email=" + encodeURIComponent(email);
}

function DeletedUser() {
    alert("Add delete API call here");
}

// ---- Upload button ----
$(document).on("click", ".Upload", function () {

    const id = $(this).data("id");

    window.location.href = "/Dashboard/UploadDocuments?id=" + id;
});
