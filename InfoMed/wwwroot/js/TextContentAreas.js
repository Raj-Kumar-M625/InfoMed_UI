async function addTextContentAreas() {
    var idEventVersion = parseInt($("#idEventVersion").val())
    if (idEventVersion == 0) {
        swal({
            title: "",
            text: "Please add event details.",
            icon: "warning",
        })
        return
    }
    await fetch("/TextContentArea/AddTextContent").then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content").html(data.html)
            $("#textContentModal").modal('show')
        })
}

async function editTextContent(idTextContent) {
    await fetch(`/TextContentArea/EditTextContent?idTextContentArea=${idTextContent}`).then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content").html(data.html)
            $("#textContentModal").modal('show')
        })
}

async function deleteTextContentArea(idTextContent) {
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
            await fetch(`/TextContentArea/DeleteTextContent?idTextContentArea=${idTextContent}`).then(response => response.text())
                .then(response => {
                    var data = JSON.parse(response)
                    $("#root").html(data.html)
                })
        }
    })
}

async function viewTextContent(IdTextContentArea,IdScheduleDetails) {
    await fetch(`/TextContentArea/ViewTextContent?IdTextContentArea=${IdTextContentArea}&IdScheduleDetails=${IdScheduleDetails}`)
        .then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content").html(data.html)
            $("#content-details").html(data.html)
            $("#contentModal").modal('show')
        })
}