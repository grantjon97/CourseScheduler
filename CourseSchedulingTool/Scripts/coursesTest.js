function showCourses(id) {
    $.ajax({
        url: "../api/Courses/GetCourses/" + id,
        type: "GET",
        dataType: "json",
        success: function (semesterSchedules) {
            if (semesterSchedules) {
                logSchedule(semesterSchedules);
                printSchedule(semesterSchedules);
            } else {
                console.log("Error in retrieving schedule.");
            }
        }
    });

    getElectives(id);
}

function getElectives(id) {
    $.ajax({
        url: "../api/Majors/GetElectives/" + id,
        type: "GET",
        dataType: "json",
        success: function (electives) {
            if (electives) {
                printElectives(electives);
            } else {
                console.log("Error in retrieving electives.");
            }
        }
    });
}

function printElectives(electives) {
    $("#electives").append("<h2>You still need to pick " + electives.numberOfElectivesNeeded + " electives:</h2>");

    $("#electives").append("<ul style='list-style-type:none;'>");

    for (i = 0; i < electives.courses.length; i++) {
        $("#electives").append("<li>" + electives.courses[i].Title + "</li>");
    }

    $("#electives").append("</ul>");
}

function logSchedule(semesterSchedules) {
    for (var i = 0; i < semesterSchedules.length; i++) {
        console.log(semesterSchedules[i]);
    }
}

function printSchedule(semesterSchedules) {
    for (var i = 0; i < semesterSchedules.length; i++) {
        if (i % 2 == 0) {
            // make a new row (new row for every new year or 2 semesters)
            $("#semesterPanels").append("<div class='row' id='row" + i+ "' style=''>" +
                                            "<div class='col col-left' id='col" + i + "'></div>" +
                                            "<div class='col col-right' id='col" + (i + 1) + "'></div>" +
                                        "</div>");
        }

        makeTable(semesterSchedules[i], i);
        fillTableBody(semesterSchedules[i], i);
    }
}

function makeTable(semester, i) {
    $("#col" + i).append("<div class='container'>" +
        "<h2>" + shortDate(semester.Term.StartDate) + "</h2>" +
        "<table id='table" + i + "' class='table-striped' width='100%'>" +

        "<thead>" +
        "<tr>" +
        "<th width='50%'>Title</th>" +
        "<th width='30%'>Course Number</th>" +
        "<th>Credits</th>" +
        "</tr>" +
        "</thead>" +

        "<tbody id='" + "semester" + i + "'>" +
        "</tbody>" +

        "</table>" +
        "</div >");
}

function fillTableBody(semester, i) {
    var totalCredits = 0;
    for (j = 0; j < semester.Courses.length; j++) {
        $("#semester" + i).append("<tr>" +
                                      "<td>" + semester.Courses[j].Title + "</td>" +
                                      "<td>" + semester.Courses[j].Type + " " + semester.Courses[j].CourseNumber + "</td>" +
                                      "<td>" + semester.Courses[j].Credits + "</td>" +
                                  "</tr>");

        totalCredits = totalCredits + semester.Courses[j].Credits;
    }

    $("#table" + i).append("<tfoot>" +
                                "<tr>" +
                                    "<td></td>" +
                                    "<td></td>" +
                                    "<td><strong>" + totalCredits + "</strong></td>" +
                                "</tr >" +
                            "</tfoot >");
}

function shortDate(longDateString) {

    monthStr = longDateString.substring(5, 7);
    yearStr = longDateString.substring(0, 4);

    var months = ['January', 'February', 'March', 'April',
        'May', 'June', 'July', 'August',
        'September', 'October', 'November', 'December'];

    return months[parseInt(monthStr) - 1] + " " + yearStr;
}