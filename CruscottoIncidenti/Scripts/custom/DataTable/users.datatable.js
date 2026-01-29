$(document).ready(function () {
    $("#editButton, #detailsButton, #deleteButton").prop("disabled", true);
});

var table;

function renderUsersGrid() {
    table = $("#usersTable").DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: "/User/GetUsersGrid",
            type: "POST",
            error: () => {
                toastr["error"]("Failed to load users");
            }
        },
        columnDefs: [
            { orderSequence: ["asc", "desc"], targets: "_all" }
        ],
        order: [[1, "asc"]],
        select: "single",
        columns: [
            { data: "id", title: "Id", name: "id", visible: false },
            { data: "username", title: "User Name", name: "username" },
            { data: "email", title: "Email", name: "email" },
            {
                data: "isEnabled", title: "Is Enabled", name: "isEnabled", orderable: false,
                render: function (data, type, full, meta) {
                    return data ?
                        "<i class='fas fa-check-circle text-success'></i>" :
                        "<i class='fas fa-times-circle text-danger'></i>";
                }
            }
        ]
    });

    setButtonsEnabledOrDisabled(table);
}

function setButtonsEnabledOrDisabled(table) {
    var actions = $("#editButton, #detailsButton, #deleteButton");

    table.on("select", function (e, dt, type, indexes) {
        actions.prop("disabled", false);

        var rowNode = table.row(indexes).node();
        $(rowNode).find("td:eq(2) i").removeClass("text-success").addClass("text-white");
    });

    table.on("deselect", function (e, dt, type, indexes) {
        actions.prop("disabled", true);

        var rowNode = table.row(indexes).node();
        $(rowNode).find("td:eq(2) i").removeClass("text-white").addClass("text-success");
    });

    table.on("draw.dt", function (e, settings) {
        actions.prop("disabled", true);
    });
}

function createAction() {
    $.ajax({
        url: "/User/GetCreateUser",
        method: "GET",
        success: (data) => {
            $("#modal").html(data);
            $("#modal").modal("show");
            $(".selectpicker").selectpicker();
        },
        error: () => {
            toastr["error"]("Modal failed to open");
        }
    });
}

function editAction() {
    var id = table.rows({ selected: true }).data()[0].id;

    $.ajax({
        url: "/User/GetUpdateUser",
        data: { id: id },
        method: "GET",
        success: (data) => {
            $("#modal").html(data);
            $("#modal").modal("show");
            $('.selectpicker').selectpicker();
        },
        error: () => {
            toastr["error"]("Modal failed to open");
        }
    });
}

function detailedAction(shouldBeDeleted = false) {
    var id = table.rows({ selected: true }).data()[0].id;

    $.ajax({
        url: "/User/GetDetailedUser",
        data: {
            id: id,
            shouldBeDeleted: shouldBeDeleted
        },
        method: "GET",
        success: (data) => {
            $("#modal").html(data);
            $("#modal").modal("show");
            $('.selectpicker').selectpicker();
        },
        error: () => {
            toastr["error"]("Modal failed to open");
        }
    });
}

function onSuccessModalAction(data) {
    if (!data) {
        $("#modal").modal("hide");
        $("#usersTable").DataTable().ajax.reload();
        toastr["success"]("Action completed successfuly");
    }
    else {
        $('.selectpicker').selectpicker();
        toastr["error"]("Action failed");
    }
}

function onFailureModalAction() {
    toastr["error"]("Action failed");
}