async function addLastYearMaster() {
    debugger
    var idEventVersion = parseInt($("#idEventVersion").val())
    if (idEventVersion == 0) {
        swal({
            title: "",
            text: "Please add event details.",
            icon: "warning",
        })
        return
    }
    await fetch("/LastYearMemories/AddLastYearMemories")
        .then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-master").html(data.html)
            $("#LastYearModal").modal('show')
        })
}

async function editLastYearMaster(idLastYearMemory) {
    debugger
    await fetch(`/LastYearMemories/EditLastYearMemories?idLastYearMemory=${idLastYearMemory}`).then(response => response.text()) 
        .then(response => {
            var data = JSON.parse(response)
            $("#content-master").html(data.html)
            $("#LastYearModal").modal('show')
        })
}


async function deleteLastYearMaster(idLastYearMemory) {
    swal({
        title: "Are you sure to delete?",
        text: "",
        icon: "warning",
        dangerMode: true,
        buttons: {
            cancel: "Cancel",
            confirm: "Ok",
        },
    }).then(async (result) => {
        if (result) {
            await fetch(`/LastYearMemories/DeleteLastYearMaster?idLastYearMemory=${idLastYearMemory}`).then(response => response.text())
                .then(response => {
                    var data = JSON.parse(response)
                    //$("#scheduleModal").modal('hide')
                    $("#root").html(data.html)
                })
        }
    })
}

async function viewMediaContent(content) {
    debugger
    await fetch(`/LastYearMemories/ViewMediaContent?mediaContent=${content}`)
        .then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content").html(data.html)
            $("#content-details").html(data.html)            
            $("#MediacontentModal").modal('show')
        })
}