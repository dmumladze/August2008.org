
Hero.saveAction = undefined;
Hero.editAction = undefined;
Hero.deletePhotoAction = undefined;

function Hero() {
    // server model
    this.HeroId = null;
    this.MilitaryRankId = null;
    this.MilitaryGroupId = null;
    this.MilitaryAwardId = null;
    this.FirstName = null;
    this.LastName = null;
    this.MiddleName = null;
    this.Dob = null;
    this.Died = null;
    this.Biography = null;
    this.LanguageId = null;
    // client model
    this.photos = new Array();
};
Hero.init = function () {
    // editor dialog
    $('<div id="hero-dialog"></div>').appendTo(document.body);  
    // dialog open
    $('#createButton').click(function () {    
        $.hero.edit();
    });
};
Hero.prototype.edit = function (id) {
    this.initEditDialog();
    $.ajax({
        url: Hero.editAction + (id != undefined ? ("/" + id) : ""),
        method: 'GET',
        success: function (result) {
            $('#hero-dialog').empty();
            $('#hero-dialog').html(result);
            $('#hero-dialog').dialog('open');
        }
    });
};
Hero.prototype.toFormData = function () {
    //debugger;
    var formData = new FormData();
    for (var prop in this) {
        var elmnt = $('#' + prop);
        if (this.hasOwnProperty(prop) && elmnt.length != 0) {
            formData.append(prop, (this[prop] = elmnt.val()) || '');
        }
    }
    for (var i = 0; i < this.photos.length; i++) {
        formData.append("images", this.photos[i]);
    }
    return formData;
};
Hero.prototype.onFileChange = function (sender, args) {
    var i = 0;
    var len = sender.files.length;
    var img;
    var reader;
    var file;

    for (; i < len; i++) {
        file = sender.files[i];
        if (!!file.type.match(/image.*/)) {
            if (window.FileReader) {
                reader = new FileReader();
                reader.onloadend = function(e) {
                    $.hero.addPhoto(e.target.result, file);
                };
                reader.readAsDataURL(file);
            }
        }        
    }
};
Hero.prototype.addPhoto = function (src, file) {
    //debugger;    
    var img = document.createElement('img');
    img.src = src;
    img.setAttribute("class", "new-hero-photo");

    var maxWidth = 125;
    var maxHeight = 120;
    var adjust = 0;
    var percent = 0;
    var wPercent = 0;
    var hPercent = 0;
    var wAdjust = 0;
    var hAdjust = 0;

    var div = $('<div class="new-thumb-img"></div>');
    // 1. append first
    //if ($('.existing-humb-img').length != undefined) {
    //    div.insertBefore('.existing-thumb-img');
    //}
    //else {
        $('#upload-images').append(div.append(img));
    //}      
    // 2. get dimmensions
    var imageWidth = img.width;
    var imageHeight = img.height;

    // 3. calculate
    if (imageWidth >= (maxWidth - wAdjust)) {
        wPercent = (imageWidth - (maxWidth - adjust)) / imageWidth;
    }
    if (imageHeight >= (maxHeight - hAdjust)) {
        hPercent = (imageHeight - (maxHeight - adjust)) / imageHeight;
    }
    percent = Math.max(wPercent, hPercent);
    if (percent != 0) {
        imageHeight = Math.round(imageHeight - (imageHeight * percent));
        imageWidth = Math.round(imageWidth - (imageWidth * percent));
    }
    // 3. and scale to size
    img.setAttribute("width", "imageWidth");
    img.setAttribute("height", "imageHeight");
    img.width = imageWidth;
    img.height = imageHeight;

    this.photos[this.photos.length] = file;
};
Hero.prototype.save = function () {
    //debugger;
    var formData = $.hero.toFormData();
    $.ajax({
        url: Hero.saveAction,
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function(res) {
            alert(res);
        }
    });
};
Hero.prototype.deletePhoto = function(heroPhotoId, photoBox) {
    //debugger;
    var form = new FormData();
    form.append("id", heroPhotoId);
    $.ajax({
        url: Hero.deletePhotoAction,
        type: "POST",
        data: form,
        processData: false,
        contentType: false,
        success: function (res) {
            $('#upload-images #' + photoBox).remove();
        }
    });
}
Hero.prototype.initEditDialog = function () {
    var bodyHeight = $(window).height();
    var winHeight = 825;
    if (bodyHeight < winHeight) {
        winHeight = bodyHeight - 20;
    }
    $('#hero-dialog').dialog({
        autoOpen: false,
        modal: true,
        width: 725,
        height: winHeight,
        title: 'Hero',
        buttons: [
            {
                text: "Save",
                //"class": "btn btn-inverse",
                click: function () {
                    $.hero.save();
                }
            },
            {
                text: "Cancel",
                //"class": "btn btn-inverse",
                click: function () {
                    $(this).dialog("destroy");
                }
            }
        ]
    });
};

$.hero = new Hero();

/*
*/