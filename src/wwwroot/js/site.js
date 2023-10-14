document.addEventListener("DOMContentLoaded", function () {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))
});

document.getElementById('btnSwitch').addEventListener('click', () => {
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        document.getElementById("btnSwitch").innerHTML = '<i class="fa-solid fa-sun"></i>';
        document.documentElement.setAttribute('data-bs-theme', 'light');
        document.getElementsByTagName("footer")[0].classList.remove("bg-dark");
        document.getElementsByTagName("nav")[0].classList.remove("bg-dark");
        document.getElementsByTagName("footer")[0].classList.add("bg-light");
        document.getElementsByTagName("nav")[0].classList.add("bg-light");
        fetch('/ChangeTheme', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `theme=light`,
        });
    }
    else {
        document.getElementById("btnSwitch").innerHTML = '<i class="fa-solid fa-cloud-moon"></i>';
        document.documentElement.setAttribute('data-bs-theme', 'dark');
        document.getElementsByTagName("footer")[0].classList.remove("bg-light");
        document.getElementsByTagName("nav")[0].classList.remove("bg-light");
        document.getElementsByTagName("footer")[0].classList.add("bg-dark");
        document.getElementsByTagName("nav")[0].classList.add("bg-dark");
        fetch('/ChangeTheme', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `theme=dark`,
        });
    }
});