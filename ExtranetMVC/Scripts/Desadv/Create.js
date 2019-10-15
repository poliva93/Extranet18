$(document).ready(function () {
    //    $('select').formSelect();
    //    $('select').not('.disabled').formSelect();
    $.get('/api/desadv/listaclienti')
        .done(o => {
            $('#idCliente').empty();
            $('#idCliente').append('<option value="">Selezionare un cliente</option>');
            o.forEach(s => {
                var option = '<option value="' + s.id + '">'+ s.id + ' - ' + s.nome + '</option>';
                $('#idCliente').append(option);
            });
        });
    $("#idCliente").change(function () {
        $.get('/api/desadv/ClienteBolle/' + $('#idCliente').val())
            .done(o => {
                $('#ddlBolle').empty();
                $('#ddlBolle').append('<option value="">Selezionare una bolla</option>');
                o.forEach(s => {
                    var option = '<option value="' + s.id + '">' + s.bolla + '</option>';
                    $('#ddlBolle').append(option);
                });
            });
        $.get('/api/desadv/ClienteTrasportatori/' + $('#idCliente').val())
            .done(o => {
                $('#ddlTrasportatore').empty();
                $('#ddlTrasportatore').append('<option value="">Selezionare un trasportatore</option>');
                o.forEach(s => {
                    var option = '<option value="' + s + '">' + s + '</option>';
                    $('#ddlTrasportatore').append(option);
                });
            });
    });
    $("#ddlBolle").change(function () {
        $.get('/api/desadv/ImballoStandard/' + $('#ddlBolle').val())
            .done(o => {
                if (o == "True") {
                    $('#ddlImbStandard').prop('disabled', false);

                }
                else {
                    $('#ddlImbStandard').prop('disabled', true);
                }

            });
    });
});
$("#btnCrea").click(function () {
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    varCliente = $("#idCliente").val();
    varBolla = $("#ddlBolle").val();
    var varImballo = $("#ddlImbStandard option:selected").val();
    varBollaEffettiva = $("#ddlBolle option:selected").text();
    varBollaEffettiva = varBollaEffettiva.substr(0, varBollaEffettiva.indexOf(' '));
    if (varBollaEffettiva === "") { varBollaEffettiva = $("#ddlBolle option:selected").text() };
    varTrasportatore = $("#ddlTrasportatore").val();
    varPartenza = $("#ddlPartenza").val();

    //INSERIRE CHIAMATA AJAX

    //string cliente, string bolla, string bollaEffettiva, string trasportatore, string partenza
    $.ajax({
        url: "/Desadv/Create",
        type: 'POST',
        //async: false,
        data: { __RequestVerificationToken: token, cliente: varCliente, bolla: varBolla, bollaEffettiva: varBollaEffettiva, trasportatore: varTrasportatore, partenza: varPartenza, Imballo: varImballo },
        dataType: 'json'
        //success: function (xhr) { window.alert(xhr.status + xhr.data); },
        //error: function (xhr) { alert('FALLITO: status richiesta:' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText); }
    }).done(o => {
        alert("status: " + o.status + "" + "Messaggio: " + o.data);
        window.location.replace('invioedi?Cliente=_' + varCliente + '&sNumero=_' + o.ID);
    });
    //$.post('/Desadv/Create/' + varCliente + '/' + varBolla + '/' + varBollaEffettiva + '/' + varTrasportatore + '/' + varPartenza)
    //    .done((o) => { window.alert(o.responseText); });


});

