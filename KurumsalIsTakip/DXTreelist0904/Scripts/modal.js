
$(document).ready(function () {
    openModal();
    FormGonder();
});



function openModal(obj, url) {
    if (url == null || url == '')
        return;
    var tempclick = obj.attr('onclick');

    obj.attr('onclick', "");

    $('#modalDiv').html("");

    $.get(url, function (data, status) {
        if (status == 'success') {
            $('#modalDiv').html(data);
            $('#modalDiv').modal('show');
            obj.attr("onclick", tempclick);
        }
        else {
            $('#modalDiv').html('Sayfa Açılamadı!');
            $('#modalDiv').modal('show');
            obj.attr('onclick', tempclick);
        }
    });
}

function SetFormObjects() {

    $("a").click(function () {
        if ($(this).attr("href") == "#")
            return false;
        return true;
    });
    $("#adminEkle").validationEngine();
    $("#aciliyetSil").validationEngine();

    openModal();

    FormGonder();
}



function FormGonder(formId, url) {
    if (formId == null)
        return false;

    if ($("#" + formId).validationEngine('validate') == false)
        return false;

    $.post(url, $("#" + formId).serialize())
        .done(function (data) {
            $("#modalDiv").html(data);
            SetFormObjects();
        })
        .fail(function () {
            $("#modalDiv").html("Sayfa Açılamadı!");
        });

    return false;
}