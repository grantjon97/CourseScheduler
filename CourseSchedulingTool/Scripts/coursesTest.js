function showCourses(id) {
    $.ajax({
        url: "../api/Courses/GetCourses/" + id,
        type: "GET",
        dataType: "json",
        success: function (semesterSchedules) {
            if (semesterSchedules) {
                logSchedule(semesterSchedules);
                printSchedule(semesterSchedules)
            } else {
                console.log("Error in retrieving schedule.");
            }
        }
    });
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
            $("#semesterPanels").append("<div class='row' id='row" + i+ "' style='background-color:lightblue'>" +
                                            "<div class='col' id='col" + i + "'></div>" +
                                            "<div class='col' id='col" + (i + 1) + "'></div>" +
                                        "</div>");
        }

        makeTable(semesterSchedules[i], i);
        fillTableBody(semesterSchedules[i], i);


    }
}

function makeTable(semester, i) {
    $("#col" + i).append("<div class='container'>" +
        "<h2>" + shortDate(semester.Term.StartDate) + "</h2>" +
        "<table class='table'>" +

        "<thead>" +
        "<tr>" +
        "<th>Title</th>" +
        "<th>Course Number</th>" +
        "<th>Credits</th>" +
        "</tr>" +
        "</thead>" +

        "<tbody id='" + "semester" + i + "'>" +
        "</tbody>" +

        "</table>" +
        "</div >");
}

function fillTableBody(semester, i) {
    for (j = 0; j < semester.Courses.length; j++) {
        $("#semester" + i).append("<tr>" +
                                      "<td width='50%'>" + semester.Courses[j].Title + "</td>" +
                                      "<td width='30%'>" + semester.Courses[j].Type + " " + semester.Courses[j].CourseNumber + "</td>" +
                                      "<td>" + semester.Courses[j].Credits + "</td>" +
                                  "</tr>");
    }
}

function shortDate(longDateString) {

    monthStr = longDateString.substring(5, 7);
    yearStr = longDateString.substring(0, 4);

    var months = ['January', 'February', 'March', 'April',
        'May', 'June', 'July', 'August',
        'September', 'October', 'November', 'December'];

    return months[parseInt(monthStr) - 1] + " " + yearStr;
}