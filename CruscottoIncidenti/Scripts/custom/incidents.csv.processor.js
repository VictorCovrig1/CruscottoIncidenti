function processCSVData(csvContent) {
    const lines = csvContent.trim().split("\n");
    const headers = lines[0].trim().split(",");

    if (!checkIfHeadersValid(headers))
        return null;

    const result = [];

    for (let i = 1; i < lines.length; i++) {
        const values = lines[i].trim().split(",");

        if (values.length === headers.length) {
            const entry = {};

            for (let j = 0; j < headers.length; j++) {
                entry[headers[j]] = values[j];
            }

            result.push(entry);
        }
    }

    return result;
}

function checkIfHeadersValid(headers) {
    const tableHeaders = ["RequestNr", "OpenDate", "CloseDate", "Type", "Urgency"];
    return tableHeaders.every(term => headers.includes(term));
}

