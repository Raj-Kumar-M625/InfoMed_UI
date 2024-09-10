$(".login-btn").on("click", () => $(this).addClass("disabled"))

$("#dt-length-0").css("margin-right", "8px")

$("#DataTables_Table_0").hide();

function applyJQTable() {
    new DataTable('#table', {
        "bScrollCollapse": true,
        "language": {
            "lengthMenu": "Show _MENU_ Entries per page"
        },
        "columnDefs": [
            { "width": "50%", "targets": 0 },
            { "width": "20%", "targets": 4 },
            { "width": "30%", "targets": 5 },
        ],
        "bPaginate": true
    });
}
applyJQTable()