let mousedown = false;
var typeselected = false;

$("#canvas_div").on('mousedown', function (e) {
    mousedown = true;
});

$("#canvas_div").on('mouseup', function (e) {
    mousedown = false;
});

class Slot extends PIXI.Graphics {
    on_color = 0xbb500;
    off_color = 0xbbbbbb;
    selected = false;
    width = 100;
    height = 10;
    //text;

    color = 0xffffff;

    constructor(id, x, y, isSelected = false) {
        super();

        this.x = x;
        this.y = y;
        this.id = id;
        this.selected = isSelected;
        this.init_draw(isSelected);
        this.interactive = true;

        this.on('mousedown', function (e) {
            if (this.selected) {
                typeselected = true;
            }
            else {
                typeselected = false;
            }
            this.draw_me();
        });

        this.on('mousemove', function (e) {
            if (mousedown) {
                if (e.data.global.x >= this.x && e.data.global.x <= this.x + this.width
                    && e.data.global.y >= this.y && e.data.global.y <= this.y + this.height) {
                    this.draw_me();
                }
            }
        });

    }

    draw_me() {
        if (typeselected == this.selected) {
            this.selected = !this.selected;
            if (typeselected) {
                this.clear();
                this.beginFill(this.off_color);
                this.drawRect(0, 0, this.width, this.height);
            }
            else {
                this.clear();
                this.beginFill(this.on_color);
                this.drawRect(0, 0, this.width, this.height);
            }
        }
    }

    init_draw(isSelected) {
        this.clear();
        if (isSelected) {
            this.beginFill(this.on_color);
        }
        else {
            this.beginFill(this.off_color);
        }
        this.drawRect(0, 0, this.width, this.height);
    }

}