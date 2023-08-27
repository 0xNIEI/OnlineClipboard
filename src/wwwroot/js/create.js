// Get references to the checkbox and select elements
const checkbox = document.getElementById("onlyViewableOnce");
const select = document.getElementById("expiresIn");

// Add an event listener to the checkbox
checkbox.addEventListener("change", function() {
    // Check if the checkbox is checked
    if (checkbox.checked) {
        // Disable the select element
        select.disabled = true;
    } else {
        // Enable the select element
        select.disabled = false;
    }
});