let mousedown = false;
var typeselected = false;

$("#canvas_div").on('mousedown', function (e) {
    mousedown = true;
});

$("#canvas_div").on('mouseup', function (e) {
    mousedown = false;
});

var slots = [];

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
                slots[this.id] = "0";
                this.clear();
                this.beginFill(this.off_color);
                this.drawRect(0, 0, this.width, this.height);
            }
            else {
                slots[this.id] = "1";
                this.clear();
                this.beginFill(this.on_color);
                this.drawRect(0, 0, this.width, this.height);
            }
        }
    }

    init_draw(isSelected) {
        
        this.clear();
        if (isSelected) {
            slots.push("1");
            this.beginFill(this.on_color);
        }
        else {
            slots.push("0");
            this.beginFill(this.off_color);
        }
        this.drawRect(0, 0, this.width, this.height);
    }

}


function drawDays() {
    const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
    let dayX = 100;
    for (let i = 0; i < days.length; i++) {
        var dayText = new PIXI.Text(days[i],
            {
                fontFamily: 'Arial',
                fontSize: 14,
                fill: 0x000000,
                align: 'center',
            });

        app.stage.addChild(dayText)
        // Position the text
        dayText.x = dayX + 15;
        dayText.y = 25;
        dayX += 110;
    }
    console.log(slots);
}


function Save_Availability(userId) {
    let spinner = document.getElementById("saving-spinner");
    spinner.hidden = false;
    let data = "";
    for (let i = 0; i < slots.length; i++)
        data += slots[i];
    $.post(
            {
                url: "/Availabilities/SetSchedule",
                data: {
                    slots: data,
                    userId: userId,
                }
            })
        .done(function (data) {
            spinner.hidden = true;
        });
}

