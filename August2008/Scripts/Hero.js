
function Hero() {
    this.HeroId = null;
    this.MilitaryRankId = null;
    this.MilitaryGroupId = null;
    this.FirstName = null;
    this.LastName = null;
    this.MiddleName = null;
    this.Dob = null;
    this.Biography = null;
};
Hero.init = function(editUrl) {
    // editor dialog
    $('#hero-dialog').dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        buttons: {
            Save: function () {
                alert('Saved');
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });    
    // dialog open
    $('#createButton').click(function() {
        August2008.hero.edit(editUrl);
    });
};
Hero.prototype.edit = function(url) {
    $.ajax({
        url: url,
        method: 'GET',
        success: function(result) {
            $('#hero-dialog').html(result);
            $('#hero-dialog').dialog('open');
        },
        error: function(xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
};
Hero.prototype.create = function() {
    this.hydrateForm();
    return JSON.stringify(this);
};
Hero.prototype.hydrateForm = function() {
    for (var prop in this) {
        debugger;
        if (this.hasOwnProperty(prop)) {
            this[prop] = $('#'+prop).val();
        }
    }
};

August2008.hero = new Hero();