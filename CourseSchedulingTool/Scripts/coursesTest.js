const showCourses = async () => {

    const response = await fetch('../api/Courses');
    const courseTermViews = await response.json();

    courseTermViews.forEach(c => {
        console.log(c.Title)
    });
}