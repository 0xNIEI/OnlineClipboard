document.getElementById('btnSwitch').addEventListener('click', () => {
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        document.getElementById("btnSwitch").innerHTML = '<i class="fa-solid fa-sun"></i>';
        document.documentElement.setAttribute('data-bs-theme', 'light')
        fetch('/changeTheme', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `theme=light`,
        });
    }
    else {
        document.getElementById("btnSwitch").innerHTML = '<i class="fa-solid fa-cloud-moon"></i>';
        document.documentElement.setAttribute('data-bs-theme', 'dark')
        fetch('/changeTheme', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `theme=dark`,
        });
    }
});
