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
                getTotalPage();
            }
        });
    }

    function getTotalPage() {
        $.ajax({
            url: "/Reports/GetTotalPage",
            type: "GET",
            data: data,
            success: function (_page) {

                data.totalPages = _page.totalPage;             
                $("#total-pages").val(_page.totalPage);         

                $("#page-info").text("Page 1 of " + _page.totalPage);

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
    });

    $("#pageSize").on("change", function () {
        data.pageSize = $("#pageSize").val();
        data.pageNumber = 1;
        loadTable();
    });
});
