$(document).ready(function () {
    $("#editButton, #detailsButton, #deleteButton").prop("disabled", true);
});

function getUsersGrid() {
    var table = $("#usersTable").DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: "/User/GetUserGrid",
            type: "POST"
        },
        columnDefs: [
            { orderSequence: ["asc", "desc"], targets: "_all" }
        ],
        order: [[1, 'asc']],
        select: "single",
        layout: {
            bottom2Start: {
                buttons: [
                    {
                        text: "<i class='fas fa-plus'></i> Add",
                        action: function (e, dt, node, config) {
                            createAction();
                        },
                        attr: {
                            id: "createButton",
                            class: "btn btn-success"
                        }
                    },
                    {
                        text: "<i class='fas fa-pen'></i> Edit",
                        action: function (e, dt, node, config) {
                            var selectedId = dt.rows({ selected: true }).data()[0].id;
                            editAction(selectedId);
                        },
                        attr: {
                            id: "editButton",
                            class: "btn btn-primary"
                        }
                    },
                    {
                        text: "<i class='fas fa-info'></i> Details",
                        action: function (e, dt, node, config) {
                            var selectedId = dt.rows({ selected: true }).data()[0].id;
                            detailedAction(selectedId);
                        },
                        attr: {
                            id: "detailsButton",
                            class: "btn btn-secondary"
                        }
                    },
                    {
                        text: "<i class='fas fa-trash'></i> Delete",
                        action: function (e, dt, node, config) {
                            var selectedId = dt.rows({ selected: true }).data()[0].id;
                            detailedAction(selectedId, true);
                        },
                        attr: {
                            id: "deleteButton",
                            class: "btn btn-danger"
                        }
                    },
                ]
            }
        },
        columns: [
            { data: "id", title: "Id", name: "id", visible: false },
            { data: "userName", title: "UserName", name: "userName" },
            { data: "email", title: "Email", name: "email" },
            {
                data: "isEnabled", title: "IsEnabled", name: "isEnabled",
                render: function (data, type, full, meta) {
                    return data ?
                        "<i class='fas fa-check-circle text-success'></i>" :
                        "<i class='fas fa-times-circle text-danger'></i>";
                }
            }
        ]
    });

    table.on("select", function (e, dt, type, indexes) {
        $("#editButton, #detailsButton, #deleteButton").prop("disabled", false);

        var rowNode = table.row(indexes).node();
        $(rowNode).find("td:eq(2) i").removeClass("text-success").addClass("text-white");
    });

    table.on("deselect", function (e, dt, type, indexes) {
        $("#editButton, #detailsButton, #deleteButton").prop("disabled", true);

        var rowNode = table.row(indexes).node();
        $(rowNode).find("td:eq(2) i").removeClass("text-white").addClass("text-success");
    });

    table.on("draw.dt", function (e, settings) {
        $("#editButton, #detailsButton, #deleteButton").prop("disabled", true);
    });
}

function createAction() {
    $.ajax({
        url: "/User/GetCreateUserModal",
        method: "GET",
        success: (data) => {
            $("#userModal").html(data);
            $("#userModal").modal("show");
            $('.selectpicker').selectpicker();
        }
    });
}

function editAction(id) {
    $.ajax({
        url: `/User/GetEditUserModal/${id}`,
        method: "GET",
        success: (data) => {
            $("#userModal").html(data);
            $("#userModal").modal("show");
            $('.selectpicker').selectpicker();
        }
    });
}

function detailedAction(id, shouldBeDeleted = false) {
    $.ajax({
        url: `/User/GetDetailedUserModal/${id}?shouldBeDeleted=${shouldBeDeleted}`,
        method: "GET",
        success: (data) => {
            $("#userModal").html(data);
            $("#userModal").modal("show");
        }
    });
}

function onSuccessModalAction(data) {
    if (!data) {
        $("#userModal").modal("hide");
        $("#usersTable").DataTable().ajax.reload();
    }
    else
        $('.selectpicker').selectpicker();
}