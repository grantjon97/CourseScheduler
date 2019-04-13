const showCourses = async () => {

    const response = await fetch('../api/Courses');
    const courseTermViews = await response.json();

    console.log(courseTermViews);
}