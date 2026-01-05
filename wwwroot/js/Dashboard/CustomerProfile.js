$(function () {

    var data = {
        pageSize: 5,
        pageNumber: 1,
        totalPages: 0,
        category: "All",
        userId: $("#transaction-id").val()
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

    function getTotalPage(callback) {
        $.ajax({
            url: "/Reports/GetTotalPage",
            type: "GET",
            data: data,
            success: function (res) {
                data.totalPages = res.totalPage;
                $("#total-pages").val(res.totalPage);
                if (callback) callback();
            }
        });
    }

    // Initial load
    getTotalPage(function () {
        loadTable();
    });

    // Prev / Next
    $("#prev, #next").on("click", function () {

        if (this.id === "prev" && data.pageNumber > 1)
            data.pageNumber--;

        if (this.id === "next" && data.pageNumber < data.totalPages)
            data.pageNumber++;

        $("#page-number").val(data.pageNumber);

        loadTable();
    });

    // Page size change
    $("#pageSize").on("change", function () {
        data.pageSize = Number($(this).val());
        data.pageNumber = 1;

        getTotalPage(function () {
            loadTable();
        });
    });

});
