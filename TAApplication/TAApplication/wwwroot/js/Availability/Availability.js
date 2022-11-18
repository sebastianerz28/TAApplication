$("#save_me").hide();
/**
 * Author: H. James de St. Germain
 * Date:   Fall 2022
 *
 * You have permission to view this as a tutorial and then use it to create your own code
 * (in a very similiar fashion) as long as you type it out yourself from your
 * brain, not from your eyes.
 */

let bg_color = 0xabcdef;

app = new PIXI.Application({ backgroundColor: bg_color });
app.renderer.resize(800, 800);

$("#canvas_div").append(app.view);

class Slot extends PIXI.Graphics {
    on_color = 0xbb500;
    off_color = 0xbbbbbb;
    selected = false;
    width = 100;
    height = 50;
    text;

    color = 0xffffff;

    constructor(id, x, y) {
        super();

        this.x = x;
        this.y = y;
        this.id = id;
        this.selected = false;
        this.text = new PIXI.Text(`I am ${id}`);

        this.interactive = true;

        this.draw_me();

        app.stage.addChild(this);

        this.on('mousedown', this.pointer_down);
        this.addChild(this.text);
        this.text.visible = false;
    }

    draw_me() {
        if (this.selected) {
            this.clear();
            this.beginFill(this.on_color);
            this.drawRect(0, 0, this.width, this.height);
            this.text.visible = true;
        }
        else {
            this.clear();
            this.beginFill(this.off_color);
            this.drawRect(0, 0, this.width, this.height);
            this.text.visible = false;
        }
    }


    pointer_down() {
        this.selected = !this.selected;

        $("#save_me").show();

        console.log(`I am ${this.id}`);

        this.draw_me();
    }

}


/////////////////////// Main Program ///////////////////


for (let i = 0; i < 20; i++) {
    if (i < 10) {
        new Slot(i, 50, 50 + i * 50);
    }
    else {
        new Slot(i, 250, 50 + (i - 10) * 50);
    }
}

function doit() {
    $("#save_me").hide();
}