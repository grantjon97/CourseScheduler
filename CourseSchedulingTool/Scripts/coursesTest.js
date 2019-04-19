function showCourses() {
    $.ajax({
        url: "../api/Courses/GetCourses",
        type: "GET",
        dataType: "json",
        success: function (schedule) {
            if (schedule) {
                console.log(schedule);
            } else {
                console.log("Error in retrieving schedule.");
            }
        }
    });
}

//function printSchedule(schedule) {
//    for (var i = 0; i < schedule.SemesterSchedules)
//}