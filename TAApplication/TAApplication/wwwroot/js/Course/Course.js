/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      10/23/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Noah Carlson and Sebastian Ramirez - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez and Noah Carlson, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *
 *    This file contains scripts for actions related to updating a note in the course list
 */
function popup() {
    $("#popup").modal("show");
};


function setPopup(noteID) {

    //Gets anchor tag that has note text
    var noteElement = document.getElementById('note-' + noteID);
    //Gets attribute that holds note value
    var noteContents = noteElement.getAttribute('data-note-contents');
    //Sets popup text area to note content
    document.getElementById("txtNote").value = noteContents;

    document.getElementById("status-message").innerHTML = "";

    //Save changes button has id and contents updated
    var submitButton = document.getElementById("update-note-btn");
    submitButton.setAttribute('data-id',noteID);
}


function EditNote() {
    //Gets values
    var contents = document.getElementById("txtNote").value;
    var id = document.getElementById("update-note-btn").getAttribute("data-id");

    $.post(
        {
            url: "/Courses/EditNote",
            data: {
                id: id,
                contents: contents,
            }
        })
        .done(function (data) {
            successfulEdit(id, contents);
        })
        .fail(function (data) {
            document.getElementById("status-message").innerHTML = "Error saving changes";
        });
        
}

//TODO: further testing required
function successfulEdit(id, contents) {
    //Updates anchor tag after successful change
    var note = document.getElementById("note-" + id);
    note.value = contents;
    note.innerHTML = contents;
    note.setAttribute('data-note-contents', contents);

    document.getElementById("status-message").innerHTML = "Changes successful";

}
