
Donations.userMessageDialog = undefined;
Donations.otherOption = undefined;

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
Donations.setCustomField = function (amount) {
    var custom = $('input[name=custom]');
    var obj = JSON.parse(custom.val());
    obj.Amount = amount;
    custom.val(JSON.stringify(obj));
}

$(document).ready(function () {
    $('#formSubmitter').click(function () {
        //debugger;
        var amount = $('#amount').val();
        Donations.setCustomField(amount);
        $('input[name="currency_code"]').val($('#currency').val());
        var isOneTime = $('#rdoOneTime').is(':checked');
        if (isOneTime) {
            $('input[name="amount"]').val(amount);
            document.forms["oneTimeForm"].submit();
        }
        else {
            $('input[name="a3"]').val(amount);
            $('input[name="srt"]').val($('#frequency').val());
            document.forms["monthlyForm"].submit();
        }
    });
    $('input[name="mychoice"]').change(function () {
        //debugger;
        var frequency = $('#choiceFrequency');
        if ($('#rdoMonthly').is(':checked')) {
            frequency.fadeIn(300, function () { frequency.css('display', 'block'); });
            Donations.otherOption = $('#amount option[value=""]').remove();
        }
        else {
            frequency.css('display', 'none');
            Donations.otherOption.appendTo($('#amount'));
        }
    });
});
