$(document).ready(function () {
    $("select").selectpicker();
    clearValidationMessageEvent();

    if (!$("#origins").val()) {
        $("#ambits").prop("disabled", true);
        $("#ambits").selectpicker("refresh");
    }
    if (!$("#ambits").val()) {
        $("#types").prop("disabled", true);
        $("#types").selectpicker("refresh");
    }
});

const defaultOption = "<option value=''></option>"
const defaultText = "None selected"

function getAmbitsByOrigin() {
    const id = $("#origins").val();

    $.ajax({
        url: "/Incident/GetAmbitsByOrigin",
        data: { id: id },
        method: "GET",
        success: (data) => {
            $("#ambits").prop("disabled", id == 0);

            const defaultOptionTypes = $(defaultOption).text(defaultText);
            $("#types")
                .html(defaultOptionTypes)
                .prop("disabled", true)
                .selectpicker("refresh");

            const defaultOptionAmbits = $(defaultOption).text(defaultText);
            $("#ambits").html(defaultOptionAmbits);
                   
            for (const [key, value] of Object.entries(data)) {
                $("#ambits").append(`<option value=${key}>${value}</option>`);
            }

            $("#ambits").selectpicker("refresh");
        },
        error: () => {
            toastr["error"]("Failed to load ambits");
        }
    });
}

function getIncidentTypeByAmbit() {
    const id = $("#ambits").val();

    $.ajax({
        url: "/Incident/GetIncidentTypeByAmbit",
        data: { id: id },
        method: "GET",
        success: (data) => {
            $("#types").prop("disabled", id == 0);

            const defaultOptionTypes = $(defaultOption).text(defaultText);
            $("#types").html(defaultOptionTypes);

            for (const [key, value] of Object.entries(data)) {
                $("#types").append(`<option value=${key}>${value}</option>`);
            }
            $("#types").selectpicker("refresh");
        },
        error: () => {
            toastr["error"]("Failed to load ambits");
        }
    });
}