$(function () {

    var data = {
        pageSize: 3,
        pageNumber: 1,
        totalPages: Number($("#total-pages").val())
    };

    function updateButtons() {
        $("#prev").prop("disabled", data.pageNumber <= 1);
        $("#next").prop("disabled", data.pageNumber >= data.totalPages);
        $("#page-info").text("Page " + data.pageNumber + " of " + data.totalPages);
    }

    function loadTable() {
        $.ajax({
            url: "/Reports/TransactionReportTable",
            type: "GET",
            data: data,
            success: function (html) {
                $("#transaction-data").html(html);
                updateButtons();
            }
        });
    }

    function getTotalPage() {
        $.ajax({
            url: "/Reports/GetTotalPage",
            type: "GET",
            data: data,
            success: function (data) {
                $("#page-number").val(data.totalPage);
                updateButtons();
            }
        });
    }

    loadTable();

    $("button").on("click", function () {
        let btnid = $(this).attr("id");

        if (btnid === "prev") data.pageNumber--;
        if (btnid === "next") data.pageNumber++;

        loadTable();
        getTotalPage();
    });

    $("#pageSize").on("change", function () {
        data.pageSize = $("#pageSize").val();
        data.pageNumber = 1;                 // reset to first page on size change
        loadTable();
        getTotalPage();
    });
});
