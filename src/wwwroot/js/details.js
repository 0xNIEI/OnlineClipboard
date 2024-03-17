document.addEventListener("DOMContentLoaded", function () {
    hljs.highlightAll();
});
document.getElementById("copy-button").addEventListener("click", function () {
    navigator.clipboard.writeText(document.getElementById("content-block").innerText);
    var originalInnerHtml = this.innerHTML;
    var origText = this.getAttribute('data-bs-title');
    const tooltipInstance = bootstrap.Tooltip.getInstance(this);
    tooltipInstance.setContent({ '.tooltip-inner': 'Copied!' });
    this.innerHTML = '<i class="fa-solid fa-check"></i>';
    setTimeout(() => {
        this.innerHTML = originalInnerHtml;
        tooltipInstance.setContent({ '.tooltip-inner': origText });
    }, 2000);
});