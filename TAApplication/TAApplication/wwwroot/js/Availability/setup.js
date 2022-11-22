/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      11/22/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and [Noah Carlson/Sebastian Ramirez] - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez and Noah Carlson, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *      This File contains the java script for the setup of the availability PIXI web application's functionality.
 */

var app = null;

function setup_pixi(width, height) {
    let bg_color = 0xFFFFFF;
    app = new PIXI.Application({ backgroundColor: bg_color });
    app.renderer.resize(width, height);
    $("#canvas_div").append(app.view);

}