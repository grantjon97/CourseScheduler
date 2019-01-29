const showCourses = async () => {

    const response = await fetch('../api/Courses');
    const courseTermViews = await response.json();

    var i = 0;
    courseTermViews.forEach(c => {

        console.log(c)

        var ul = document.getElementById("courseList");
        var li = document.createElement("li");
        li.setAttribute("id", "course" + i);
        li.appendChild(document.createTextNode(c.Season + " " + c.Year + " -- " + c.CourseTitle));
        ul.appendChild(li);

        i++;
    });
}