
function Security() {

}

Security.loginDialogId = '#login-dialog';
Security.loginButtonId = '#btn-login';

Security.prototype.init = function() {
    $(Security.loginDialogId).dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        height: 250,
        title: 'Log in',
        buttons: {
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
    $(Security.loginButtonId).click(function(e) {
        $(Security.loginDialogId).dialog('open');
    });
};
Security.prototype.login = function (action) {
    $.ajax({
        url: action,
        method: 'GET',
        success: function(result) {
            debugger;
            alert(result);
        },
        error: function(xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
};

$(document).ready(function() {
    $.security.init();
});

$.security = new Security();