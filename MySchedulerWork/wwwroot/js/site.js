
function changeGroup() {
    var element = document.getElementById('programid');
    var element2 = document.getElementById('groups');

    var courseId = element.options[element.selectedIndex].value;

    if (courseId == "") {
        courseId = "-1";
    }

    console.log("Hello");
    $.ajax({
        type: "GET",
        url: "https://localhost:5001/CoursePrograms/GetOptions/" + courseId.toString(),
        success: function (data) {
            element2.innerHTML = data;
            console.log(data);
        }
    });

    console.log("Hello THere");
}