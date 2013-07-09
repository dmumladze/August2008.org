
Spinner.instance = undefined;

function Spinner(src) {
    this.img = $('<img id="spinner" src="' + src + '" style="position:absolute;top:-99px;z-index:9999" alt=""/>');
    this.img.appendTo(document.body);
    this.img.hide();
};
Spinner.prototype.move = function(x, y) {
    this.img.css('top', y - 3);
    this.img.css('left', x + 13);
};
Spinner.show = function() {
    $(document).mousemove(function (e) {
        Spinner.instance.img.fadeIn('slow');
        Spinner.instance.move(Math.max(e.clientX, e.pageX), Math.max(e.clientY, e.pageY));
    });
};
Spinner.hide = function () {
    Spinner.instance.img.fadeOut('slow', function() {
        $(document).unbind('mousemove');
        Spinner.instance.move(-100, -100);
    });
   
};
Spinner.init = function(src) {
    if (Spinner.instance == undefined) {
        Spinner.instance = new Spinner(src);
    }
};