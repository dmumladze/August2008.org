

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
});

