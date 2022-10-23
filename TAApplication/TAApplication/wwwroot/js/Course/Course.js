
function popup() {
    $("#popupwindow").modal("show");
};

function EditNote(Course_ID, Note) {
    $.post(
        {
            url: "/Courses/EditNote",
            data: {
                id: id,
                contents: contents,
            }
        })
        .done(function (data) {
            console.log("Sample of data:", data);
        });
}