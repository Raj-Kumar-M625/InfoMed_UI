async function addFeesMaster() {
    var idEventVersion = parseInt($("#idEventVersion").val())
    if (idEventVersion == 0) {
        swal({
            title: "",
            text: "Please add event details.",
            icon: "warning",
        })
        return
    }
    await fetch("/Fees/AddFees")
        .then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-master").html(data.html)
            $("#feesModal").modal('show')
        })
}



async function editFeesMaster(idFeesMaster) {
    await fetch(`/Fees/editFees?idfeesMaster=${idFeesMaster}`).then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-master").html(data.html)
            $("#feesModal").modal('show')
        })
}

async function deleteFeesMaster(idFeesMaster) {
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
            await fetch(`/Fees/DeleteFeesMaster?idFeesMaster=${idFeesMaster}`).then(response => response.text())
                .then(response => {
                    var data = JSON.parse(response)
                    $("#root").html(data.html)
                })
        }
    })
}

