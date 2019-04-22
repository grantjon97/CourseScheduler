function majorsDropDownList() {
    $.ajax({
        url: "../api/Majors/GetMajors",
        type: "GET",
        dataType: "json",
        success: function (majors) {
            if (majors) {
                // Fill dropdown list with all available majors
                for (var i = 0; i < majors.length; i++) {
                    $("#selectList").append("<option value='" + majors[i].id + "' >" + majors[i].title + "</option>");
                }
            }
            else {
                console.log("Error in retrieving majors.");
            }
        }
    });
}