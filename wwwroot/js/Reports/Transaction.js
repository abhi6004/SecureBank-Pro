$(function () {

    var data = {
        pageSize: 3,
        pageNumber: 1,
        totalPages: Number($("#total-pages").val()),
        Category: "All",
        UserId: -1,

        // 🔥 new filters
        fromDate: "",
        toDate: "",
        searchTransactionId: "",
        searchUserId: "",
        searchType: "",
        searchDescription: ""
    };

    function updateButtons() {
        $("#prev").prop("disabled", data.pageNumber <= 1);
        $("#next").prop("disabled", data.pageNumber >= data.totalPages);
        $("#page-info").text("Page " + data.pageNumber + " of " + data.totalPages);
    }

    function updateURL() {
        let pageSize = $("#pageSize").val();
        let PageNumber = $("#page-number").val();

        let newUrl = '/Reports/DownloadPdf'
            + '?pageSize=' + pageSize
            + '&pageNumber=' + PageNumber
            + '&Category=' + data.Category
            + '&UserId=' + data.UserId
            + '&fromDate=' + (data.fromDate || "")
            + '&toDate=' + (data.toDate || "")
            + '&searchTransactionId=' + (data.searchTransactionId || "")
            + '&searchUserId=' + (data.searchUserId || "")
            + '&searchType=' + (data.searchType || "")
            + '&searchDescription=' + (data.searchDescription || "");

        $("#download-pdf").attr("href", newUrl);
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
                updateURL();
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
                $("#page-info").text("Page " + data.pageNumber + " of " + _page.totalPage);
                updateButtons();
            }
        });
    }

    // 🔥 initial load
    loadTable();

    // 🔥 pagination buttons
    $("button").on("click", function () {
        let btnid = $(this).attr("id");
        if (btnid === "prev") data.pageNumber--;
        if (btnid === "next") data.pageNumber++;
        $("#page-number").val(data.pageNumber);
        loadTable();
    });

    // 🔥 page size change
    $("#pageSize").on("change", function () {
        data.pageSize = $("#pageSize").val();
        data.pageNumber = 1;
        loadTable();
    });

    // 🔥 category change
    $("#Catogory").on("change", function () {
        data.Category = $("#Catogory").val();
        data.pageNumber = 1;
        loadTable();
    });


    // ===============================
    // ⭐ NEW: DATE FILTER EVENTS ⭐
    // ===============================

    $("#fromDate").on("change", function () {
        data.fromDate = $(this).val();
        data.pageNumber = 1;
        loadTable();
    });

    $("#toDate").on("change", function () {
        data.toDate = $(this).val();
        data.pageNumber = 1;
        loadTable();
    });


    // ===============================
    // ⭐ NEW: COLUMN FILTER EVENTS ⭐
    // ===============================

    $("#f_tid").on("input", function () {
        data.searchTransactionId = $(this).val();
    });

    $("#f_uid").on("input", function () {
        data.searchUserId = $(this).val();
    });

    $("#f_type").on("input", function () {
        data.searchType = $(this).val();
    });

    $("#f_desc").on("input", function () {
        data.searchDescription = $(this).val();
    });

    // 🔥 search button
    $("#btnSearch").on("click", function () {
        data.pageNumber = 1;
        loadTable();
    });

});
