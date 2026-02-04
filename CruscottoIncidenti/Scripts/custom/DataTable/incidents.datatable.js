$(document).ready(function () {
    $("#editLink, #detailsLink, #deleteLink").addClass("disabled");
});

function renderIncidentsGrid() {
    var table = $("#incidentsTable").DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: "/Incident/GetIncidentsGrid",
            type: "POST",
            error: () => {
                toastr["error"]("Failed to load incidents");
            }
        },
        columnDefs: [
            { orderSequence: ["asc", "desc"], targets: "_all" }
        ],
        order: [[1, "asc"]],
        select: "single",
        columns: [
            { data: "Id", title: "Id", name: "id", visible: false },
            { data: "RequestNr", title: "Request Number", name: "requestNr" },
            { data: "OpenDate", title: "Open Date", name: "openDate" },
            { data: "CloseDate", title: "Close Date", name: "closeDate" },
            { data: "Type", title: "Type", name: "type", orderable: false },
            { data: "Urgency", title: "Urgency", name: "urgency", orderable: false },
        ]
    });

    setLinksEnabledOrDisabled(table);
}

function setLinksEnabledOrDisabled(table) {
    var links = $("#editLink, #detailsLink, #deleteLink");

    table.on("select", function (e, dt, type, indexes) {
        links.removeClass("disabled");
    });

    table.on("deselect", function (e, dt, type, indexes) {
        links.addClass("disabled");
    });

    table.on("draw.dt", function (e, settings) {
        links.addClass("disabled");
    });
}

function importData() {

}