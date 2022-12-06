/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      09/27/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and [Noah Carlson/Sebastian Ramirez] - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *      This File contains the java script for the role changing functionality of the User roles page.
 */

function Change_Role(user_id, role) {
    $.post(
        {
            url: "/Admin/Change_Role",
            data: {
                user_id: user_id,
                role: role,
            }
        })
        .done(function (data) {
            console.log("Sample of data:", data);
        });
}

function GetEnrollmentData(start, end, dept, number) {
    $.get(
        {
            url: "/Admin/GetEnrollmentData",
            data: {
                start: start,
                end: end,
                dept: dept,
                number, number
            }
        })
        .done(function (data) {
            console.log("Sample of data:", data);
        });
}