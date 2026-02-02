$(document).ready(function () {
    $("select").selectpicker();
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
            var ambitText = "";
            $("#ambits").prop("disabled", id == 0);

            if (!id) {
                const defaultOptionTypes = $(defaultOption).text(`${defaultText} (Select Ambit)`);
                ambitText = " (Select Origin)";

                $("#types")
                    .html(defaultOptionTypes)
                    .prop("disabled", true)
                    .selectpicker("refresh");
            }

            const defaultOptionAmbits = $(defaultOption).text(`${defaultText}${ambitText}`);
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
            var typesText = "";

            if (!id)
                typesText = " (Select Ambit)";

            const defaultOptionTypes = $(defaultOption).text(`${defaultText}${typesText}`);
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