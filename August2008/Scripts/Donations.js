
Donations.userMessageDialog = undefined;

function Donations() {
};
Donations.init = function (dialogTitle) {
    $("<div id=\"userMessageDialog\"></div>").appendTo(document.body);
    Donations.userMessageDialog = $('#userMessageDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        height: 340,
        title: dialogTitle
    });
};
Donations.openMessageDialog = function (action, id) {
    $.ajax({
        url: action,
        method: "GET",
        success: function (result) {
            Donations.userMessageDialog.empty();
            Donations.userMessageDialog.html(result);
            Donations.userMessageDialog.dialog("open");
        }
    });
};
Donations.closeMessageDialog = function () {
    Donations.userMessageDialog.empty();
    Donations.userMessageDialog.dialog("close");
};
Donations.updateMessage = function (id, linkText) {
    var span = $('#m' + id);
    if (span.length != undefined) {
        var msg = $("#UserMessage").val();
        var link = $("#a" + id);
        span.html(msg);
        link.html(linkText);
    }
    Donations.closeMessageDialog();
};
