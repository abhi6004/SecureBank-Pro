$('.imageUpload').on('change', function () {
    const file = this.files[0];
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];

    if (file) {
        // 1. Validate Type
        if ($.inArray(file.type, allowedTypes) === -1) {
            alert('Please select a valid image file (JPG, PNG, or GIF).');
            $(this).val('');
            return false;
        }

        // 2. Validate Size (2MB)
        if (file.size > 2 * 1024 * 1024) {
            alert('File is too large! Max size is 2MB.');
            $(this).val('');
            return false;
        }
    }
});