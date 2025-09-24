// Toggle password visibility
$(function () {
    $(document).on("click", "#toggleButton", function () {
        const $passField = $("#password");
        const type = $passField.attr("type") === "password" ? "text" : "password";
        $passField.attr("type", type);

        $(this).attr("src", type === "password" ? "/images/hide.png" : "/images/view.png");
    });
});
