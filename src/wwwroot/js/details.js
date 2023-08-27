document.getElementById("copy-button").addEventListener("click", function () {
    navigator.clipboard.writeText(document.getElementById("content-block").innerText);
    var originalInnerHtml = this.innerHTML;
    this.innerHTML = '<i class="fa-solid fa-check"></i>';
    setTimeout(() => {
        this.innerHTML = originalInnerHtml;
    }, 2000);
});