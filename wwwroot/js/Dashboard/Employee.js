// Navigate to Edit page
function EdituserForm(email) {
    window.location.href = "/Dashboard/EditUsers?email=" + encodeURIComponent(email);
}

// Placeholder for delete logic
function DeletedUser() {
    alert("Implement delete logic here");
}

// Upload button click (reliable)
$(document).on("click", ".Upload", function () {
    const id = $(this).data("id");
    window.location.href = "/Dashboard/UploadDocuments?id=" + id;
});
