
// global exntensions
Array.prototype.contains = function(value) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == value)
            return true;
    }
    return false;
};

// common scripts 
August2008 = {};
August2008.resizeTimer = undefined;

August2008.adjustDimensions = function() {
    //debugger;
    var bodyHeight = $(document.body).height();
    var contentHeight = $('#contentContainer').height() + 10;    
    if (contentHeight > bodyHeight) {
        return;
    }
    var documentHeight = $(document).height();
    var headerHeight = $('#header').height();
    var footerHeight = $('#footer').height();
    var totalHeight = headerHeight + contentHeight + footerHeight;
    if (totalHeight < documentHeight) {
        $('#contentContainer').css("height", bodyHeight - headerHeight);
    }    
};

August2008.ajaxSetup = function () {
    $(document).ajaxError(function (e, xhr, ajaxSettings, thrownError) {
        Spinner.hide();
        switch (thrownError.toLowerCase()) {
            case 'unauthorized':
                alert('You are not logged in.');
                break;
            case 'forbidden':
                alert('You do not have permission for this feature.');
                break;
            default:
                alert('Oops! Something went wrong... :(');
                break;
        }        
    });
    $(document).ajaxStart(function() {
        Spinner.show();
    });
    $(document).ajaxComplete(function(e, xhr, ajaxOptions) {
        Spinner.hide();
    });
};

$(document).ready(function() {
    August2008.ajaxSetup();
    August2008.adjustDimensions();

    $(window).resize(function() {
        clearTimeout(August2008.resizeTimer);
        August2008.resizeTimer = setTimeout(August2008.adjustDimensions, 100);
    });
});

