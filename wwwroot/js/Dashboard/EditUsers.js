// Toggle password visibility
$(function () {
    $(document).on("click", "#toggleButton", function () {

        const $passField = $("#password");

        const type =
            $passField.attr("type") === "password"
                ? "text"
                : "password";

        $passField.attr("type", type);

        $(this).attr(
            "src",
            type === "password"
                ? "/images/hide.png"
                : "/images/view.png"
        );
    });

    document.querySelector("form").addEventListener("submit", function (e) {

        let isValid = true;
        let messages = [];

        const fullName = document.querySelector("[name='full_name']");
        const phone = document.querySelector("[name='phone_number']");
        const password = document.querySelector("[name='password']");

        // Full Name
        if (!fullName.value.trim()) {
            isValid = false;
            messages.push("Full Name is required");
        }

        // Phone
        if (!/^\d{10}$/.test(phone.value)) {
            isValid = false;
            messages.push("Phone number must be 10 digits");
        }

        // Password (only if entered)
        if (password.value && password.value.length < 6) {
            isValid = false;
            messages.push("Password must be at least 6 characters");
        }

        if (!isValid) {
            e.preventDefault();
            alert(messages.join("\n"));
        }
    });
});
