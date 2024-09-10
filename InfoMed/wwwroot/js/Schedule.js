async function addScheduleMaster() {
    var idEventVersion = parseInt($("#idEventVersion").val())
    if (idEventVersion == 0) {
        swal({
            title: "",
            text: "Please add event details.",
            icon: "warning",
        })
        return
    }
    await fetch("/Schedule/AddSchedule")
        .then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-master").html(data.html)
            $("#scheduleModal").modal('show')
        })
}

async function editScheduleMaster(idScheduleMaster) {
    await fetch(`/Schedule/EditSchedule?idScheduleMaster=${idScheduleMaster}`).then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-master").html(data.html)
            $("#scheduleModal").modal('show')
        })
}

async function loadScheduleDetails(idScheduleMaster) {
    await fetch(`/Schedule/ScheduleDetails?idScheduleMaster=${idScheduleMaster}`)
         .then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-master").html(data.html)
            $(`#scheduleDetailsModal-${idScheduleMaster}`).modal('show')
        })
}

async function addScheduleDetails(idScheduleMaster) {
    await fetch(`/Schedule/AddScheduleDetails?idScheduleMaster=${idScheduleMaster}`).then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-details").html(data.html)
            $("#detailsModal").modal('show')
        })
}

async function editScheduleDetails(idScheduleDetails) {
    await fetch(`/Schedule/EditScheduleDetails?idScheduleDetails=${idScheduleDetails}`).then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-details").html(data.html)
            $("#detailsModal").modal('show')
        })
}


async function deleteScheduleMaster(idScheduleMaster) {
    swal({
        title: "Are you sure to delete?",
        text: "",
        icon: "warning",
        dangerMode:true,
        buttons: {
            cancel: "Cancel",
            confirm: "Ok",
        },
    }).then(async (result) => {
        if (result) {
            await fetch(`/Schedule/DeleteScheduleMaster?idScheduleMaster=${idScheduleMaster}`).then(response => response.text())
                .then(response => {
                    var data = JSON.parse(response)
                    $("#scheduleModal").modal('hide')
                    $("#root").html(data.html)
                })
        }
    })
}

async function deleteScheduleDetails(idScheduleDetail, idScheduleMaster) {
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
            await fetch(`/Schedule/DeleteScheduleDetail?idScheduleDetail=${idScheduleDetail}`).then(response => response.text())
                .then(response => {
                    var data = JSON.parse(response)
                    $(`#scheduleDetailsModal-${idScheduleMaster}`).modal('hide')
                    $("#content-master").html(data.html)
                    $(`#scheduleDetailsModal-${idScheduleMaster}`).modal('show')
                })
        }
    })
}


async function viewTextContent(content) {
    await fetch(`/TextContentArea/ViewTextContent?textContent=${content}`)
        .then(response => response.text())
        .then(response => {
            var data = JSON.parse(response)
            $("#content-details").html(data.html)
            $("#editor").val(content)
            editor = ClassicEditor.create(document.getElementById('editor'))
            $("#contentModal").modal('show')
        })
}


function validateTimes(startTime, endTime) {
    const startTimeParts = startTime.split(':');
    const endTimeParts = endTime.split(':');
    const startDate = new Date();
    const endDate = new Date();

    startDate.setHours(parseInt(startTimeParts[0], 10), parseInt(startTimeParts[1], 10), 0, 0);
    endDate.setHours(parseInt(endTimeParts[0], 10), parseInt(endTimeParts[1], 10), 0, 0);

    return startDate < endDate;
}
