function EdituserForm(email) {
    window.location.href = "/Dashboard/EditUsers?email=" + encodeURIComponent(email);
}


function DeletedUser() {

}

$(function () {
    $("#Upload").on("click", function () {
        let id = $("#Upload-id-Auditor").val();
        window.location.href = "/Dashboard/UploadDocuments?id=" + id;
    });
});
