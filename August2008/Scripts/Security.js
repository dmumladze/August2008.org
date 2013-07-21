
Security.manageUsersDialog = undefined;
Security.manageUsersAction = undefined;

function Security() {

}

Security.init = function(action) {
    Security.manageUsersAction = action;
    $("<div id=\"manageUsersDialog\"></div>").appendTo(document.body);
    Security.manageUsersDialog = $('#manageUsersDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 850,
        height: 700,
        title: "Manage Users"
    });
    $("#manageUsersOpener").click(function(e) {
        Security.manageUsers();
    });
};
Security.manageUsers = function () {
    $.ajax({
        url: Security.manageUsersAction,
        method: "GET",
        success: function (result) {
            Security.manageUsersDialog.empty();
            Security.manageUsersDialog.html(result);
            Security.manageUsersDialog.dialog("open");
        }
    });
};
