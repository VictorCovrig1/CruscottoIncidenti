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
            { data: "id", title: "Id", name: "id", visible: false },
            { data: "requestNr", title: "Request Number", name: "requestNr" },
            { data: "openDate", title: "Open Date", name: "openDate" },
            { data: "closeDate", title: "closeDate", name: "closeDate" },
            { data: "type", title: "Type", name: "type" },
            { data: "urgency", title: "Urgency", name: "urgency" },
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