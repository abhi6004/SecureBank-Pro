function EdituserForm(email) {
    window.location.href = "/Dashboard/EditUsers?email=" + encodeURIComponent(email);
}

function DeletedUser(id) {
    // Ask for confirmation
    if (confirm("Are you sure you want to delete this user?")) {
        $.ajax({
            url: '/Dashboard/DeleteUser', // Your controller/action
            type: 'DELETE',
            data: { id: id },
            success: function (res) {
                if (res.success) {
                    alert(res.message); // show success message
                    location.reload(); // reload page
                } else {
                    alert(res.message); // show error message
                }
            },
            error: function (err) {
                console.error(err);
                alert("Something went wrong while deleting the user.");
            }
        });
    }
}


// ---- Upload button ----
$(document).on("click", ".Upload", function () {

    const id = $(this).data("id");

    window.location.href = "/Dashboard/UploadDocuments?id=" + id;
});
