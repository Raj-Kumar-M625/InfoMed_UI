
var navLinks = document.querySelectorAll('.nav-link');

navLinks.forEach(function (navLink) {
    navLink.addEventListener('click', async function () {
        var enteredValuesFlagElement = document.getElementById('enteredValuesFlag');
        if (!enteredValuesFlagElement) {
            navLinks.forEach(function (link) {
                link.classList.remove('text-primary');
                link.classList.add('text-dark');
            });
            this.classList.remove('text-dark');
            this.classList.add('text-primary');
        } else if (enteredValuesFlagElement.value === "true") {
            const submitConfirmation = await swal({
                title: "Are you sure you want to switch to next tabs?",
                text: "",
                icon: "warning",
                dangerMode: true,
                buttons: {
                    cancel: "Cancel",
                    confirm: "Ok",
                },
            });
            if (submitConfirmation) {
                navLinks.forEach(function (link) {
                    link.classList.remove('text-primary');
                    link.classList.add('text-dark');
                });
                this.classList.remove('text-dark');
                this.classList.add('text-primary');     

                var viewInputValue = document.getElementById("viewInput").value;  
                var actionInputValue = document.getElementById("actionInput").value;  
                renderView1(viewInputValue,actionInputValue)
            }

        } else {
            navLinks.forEach(function (link) {
                link.classList.remove('text-primary');
                link.classList.add('text-dark');
            });
            this.classList.remove('text-dark');
            this.classList.add('text-primary');
        }
        
    });

});

async function redirectTo(controller, action) {
    var enteredValuesFlagElement = document.getElementById('enteredValuesFlag');

    if (!enteredValuesFlagElement) {
        var url = '/' + controller + '/' + action;
        window.location.href = url;
    } else if (enteredValuesFlagElement.value === "true") {
        const submitConfirmation = await swal({
            title: "Are you sure you want to switch to Home Screen ?",
            text: "",
            icon: "warning",
            dangerMode: true,
            buttons: {
                cancel: "Cancel",
                confirm: "Ok",
            },
        });
        if (submitConfirmation) {
            debugger;
            var url = '/' + controller + '/' + action;
            window.location.href = url;
        } else {
            // User canceled the confirmation
        }
    } else {
        var url = '/' + controller + '/' + action;
        window.location.href = url;
    }
}


var today = new Date().toISOString().split('T')[0];
$("#EventStartDate").attr("min", today);
$("#endDate").attr("min", today);

$("#EventStartDate").on("change", function () {    
    var startDate = $(this).val();
    $("#endDate").val('');
    $("#endDate").attr("min", startDate);
    if (startDate) {
        var startDateObj = new Date(startDate);
        var maxDateObj = new Date(startDateObj);
        maxDateObj.setDate(maxDateObj.getDate() + 30);     
        var maxDate = maxDateObj.toISOString().split('T')[0];
        $("#endDate").attr("max", maxDate);
    }
});
$("#endDate").on("focus", function () {
    var startDate = $("#EventStartDate").val();
    if (!startDate) {
         swal({
             title: "Please enter the start date.",
            text: "",
            icon: "warning",
        });
        $("#EventStartDate").focus();
    }
});

async function renderView(view, action) {
    var enteredValuesFlagElement = document.getElementById('enteredValuesFlag');
    var URL = actionType === "CREATE" ? `/Event/Event?view=${view}` : `/Event/EditEvent?view=${view}`
    if (!enteredValuesFlagElement) {
        await fetch(URL)
            .then(response => response.text())
            .then(response => {
                var data = JSON.parse(response)
                $("#root").html(data.html)
                $("#approve-btn").removeClass('text-dark')
            }).catch(e => {
                console.error(e)
            })
    } else if (enteredValuesFlagElement.value === "true") { 
        document.getElementById('viewInput').value = view;
        document.getElementById('actionInput').value = action;
        return; // Exit function if flag is true
    } else {
        await fetch(URL)
            .then(response => response.text())
            .then(response => {
                var data = JSON.parse(response);
                $("#root").html(data.html);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
    var actionType = localStorage.getItem('ACTION')
}
async function renderView1(view, action) {    
    var URL = actionType === "CREATE" ? `/Event/Event?view=${view}` : `/Event/EditEvent?view=${view}`
        await fetch(URL)
            .then(response => response.text())
            .then(response => {
                var data = JSON.parse(response);
                $("#root").html(data.html);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    var actionType = localStorage.getItem('ACTION')
}



async function ApproveEventMaster(idEventId, idVersion) {
    const submitConfirmation = await swal({
        title: "Are you sure to approve?",
        text: "",
        icon: "warning",
        dangerMode: true,
        buttons: {
            cancel: "Cancel",
            confirm: "Ok",
        },
    });

    if (submitConfirmation) {
        try {
            const response = await fetch(`/Event/ApproveEvent?idEventId=${idEventId}&idEventVersion=${idVersion}`, {
                method: 'POST'
            });
            const data = await response.json();

            if (data.success) {
                debugger
                await swal({
                    title: "Success",
                    text: "Event Approved successfully.",
                    icon: "success",
                });

                // Redirect to home controller index method after successful submission
                window.location.href = "/Home/Index";

            } else {
                console.error('Event submission failed:', data);
                // Handle potential errors in the response (optional)
                await swal({
                    title: "Error",
                    text: "Event submission failed. Please try again.",
                    icon: "error",
                });
            }
        } catch (error) {
            console.error('Error submitting event:', error);
            await swal({
                title: "Error",
                text: "An error occurred during submission. Please try again.",
                icon: "error",
            });
        }
    } else {
        // User canceled the confirmation
    }
}


async function RejectEventMaster(idEventId, idVersion) {
    debugger
    const submitConfirmation = await swal({
        title: "Are you sure to reject?",
        text: "",
        icon: "warning",
        dangerMode: true,
        buttons: {
            cancel: "Cancel",
            confirm: "Ok",
        },
    });

    if (submitConfirmation) {
        debugger
        try {            
            const response = await fetch(`/Event/RejectEvent?idEventId=${idEventId}&idEventVersion=${idVersion}`, {
                method: 'POST'
            });
            const data = await response.json();

            if (data.success) {
                debugger
                await swal({
                    title: "Success",
                    text: "Event Rejected successfully.",
                    icon: "success",
                });

                // Redirect to home controller index method after successful submission
                window.location.href = "/Home/Index";

            } else {
                console.error('Event submission failed:', data);
                // Handle potential errors in the response (optional)
                await swal({
                    title: "Error",
                    text: "Event submission failed. Please try again.",
                    icon: "error",
                });
            }
        } catch (error) {
            console.error('Error submitting event:', error);
            await swal({
                title: "Error",
                text: "An error occurred during submission. Please try again.",
                icon: "error",
            });
        }
    } else {
        // User canceled the confirmation
    }
}

async function SubmitEventMaster(idEventId, idVersion) {
    debugger
    const submitConfirmation = await swal({
        title: "Are you sure you want to send for approval?",
        text: "",
        icon: "warning",
        dangerMode: true,
        buttons: {
            cancel: "Cancel",
            confirm: "Ok",
        },
    });

    if (submitConfirmation) {
        debugger
        try {            
            const response = await fetch(`/Event/SubmitEvent?idEventId=${idEventId}&idEventVersion=${idVersion}`, {
                method: 'POST'
            });
            const data = await response.json(); 

            if (data.success) {
                debugger
                await swal({
                    title: "Success",
                    text: "Event submitted successfully.",
                    icon: "success",
                });

                // Redirect to home controller index method after successful submission
                window.location.href = "/Home/Index";

            } else {
                console.error('Event submission failed:', data);
                // Handle potential errors in the response (optional)
                await swal({
                    title: "Error",
                    text: "Event submission failed. Please try again.",
                    icon: "error",
                });
            }
        } catch (error) {
            console.error('Error submitting event:', error);
            await swal({
                title: "Error",
                text: "An error occurred during submission. Please try again.",
                icon: "error",
            });
        }
    } else {
        // User canceled the confirmation
    }
}
