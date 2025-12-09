$(function ()
{
    $('.ApplicationButton').on('click', function (e) {
        e.preventDefault();
        var form = $(this);
        var _class = form.attr('class');
        var status = ""

        if (_class.includes('btn-success'))
        {
            status = "Approved"
        }
        else if (_class.includes('btn-danger'))
        {
            status = "Denied"
        }

        var id = form.closest('td').children('.btn.btn-info.more-info').attr('id');

        var data = {
            id: id,
            status: status
        };

        $.ajax({
            type: "POST",
            url: "/LoanApplications/ApplicationProcess",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    alert("Application status updated successfully.");
                    location.reload();
                } else {
                    alert("Failed to update application status.");
                }
            },
            error: function () {
                alert("An error occurred while updating the application status.");
            }
        });
    });
})