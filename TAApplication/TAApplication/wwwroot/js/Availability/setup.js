/**
 * Global access to the PIXI Application 
 */
var app = null;

/**
 *  Create PIXI stage
 */
function setup_pixi(width, height) {
    let bg_color = 0xFFFFFF;
    app = new PIXI.Application({ backgroundColor: bg_color });
    app.renderer.resize(width, height);
    $("#canvas_div").append(app.view);

}