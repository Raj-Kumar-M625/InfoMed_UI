async function GetRegisterType(type,email,) {
    await fetch(`/Customer/RegistrationType?type=${type}`)
          .then(response => response.text())
          .then(response => {
              var data = JSON.parse(response)
              $("#content").html(data.html)
          }).catch(e => {
              console.error(e)
          })
}

document.getElementById("reg-logo").href = localStorage.getItem("location")

var tabButton = document.querySelectorAll('.tab-btn');

tabButton.forEach(function (button) {
    button.addEventListener('click', async function () {
        tabButton.forEach(function (link) {
            link.classList.remove('bg-primary');
            link.classList.remove('text-light');
            link.classList.add('bg-light');
        });
        this.classList.remove('bg-light');
        this.classList.add('bg-primary');
        this.classList.add('text-light');

    })
})