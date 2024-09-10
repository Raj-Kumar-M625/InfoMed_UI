async function addUser() {
    await fetch("/User/AddUser")
        .then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $(".user-content").html(data.html)
            $("#userModal").modal('show')
        })
}

async function editUser(idUser) {
    await fetch(`/User/EditUser?IdUser=${idUser}`).then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $(".user-content").html(data.html)
            $("#userModal").modal('show')
        })
}