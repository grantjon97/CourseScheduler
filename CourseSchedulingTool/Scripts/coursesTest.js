const showCourses = async () => {

    const response = await fetch('../api/Courses');
    const courses = await response.json();

    var i = 0;
    courses.forEach(course => {
        console.log(course.Title)
        var ul = document.getElementById("courseList");
        var li = document.createElement("li");
        li.setAttribute("id", "course" + i);
        li.appendChild(document.createTextNode(course.Title));
        ul.appendChild(li);

        i++;
    });
}