﻿
$("#idCliente").change(function () {
    varCliente = $(this).val();
    //use rfiSchooldropdown
    $('#ddlTestate').empty();
    $.ajax({
        url: "/DESADV/listaINVIO",
        type: 'POST',
        data: { Cliente: varCliente },
        success: function (testate) {
            // states is your JSON array
            var $select = $('#ddlTestate');
            $.each(testate, function (i, testata) {
                $('<option>', {
                    value: testata.Value,
                    text: testata.Text
                }).html(testata.Text).appendTo($select);
            });
            $("#ddlTestate option:last").attr("selected", "selected");
        }
    });
    $.get('/api/desadv/ListaSpedizioni/' + (varCliente !== null ? varCliente : ''))
        .done(o => {
            $('#spedizioni').DataTable({
                data: o,
                //order: [[0, 'desc'], [1, 'desc']],
                "ordering": false,
                "searching": false,
                "info": false,
                "lengthChange": false,
                destroy: true,
                createdRow: function (row, data, dataIndex) {
                    if (data["Stato"] == 'CHIUSO') {
                        $(row).addClass('spedizioneChiusa');
                    }
                    else {
                        $(row).addClass('spedizioneAperta');
                    }
                },

                columns: [
                    {
                        title: 'Cliente',
                        data: 'Cliente'
                    },
                    {
                        title: 'Numero',
                        data: 'Numero'
                    },
                    {
                        title: 'Data Spedizione',
                        data: 'DataSpedizione'
                    },

                    {
                        title: 'Bolla',
                        data: 'Bolla'
                    },
                    {
                        title: 'Articolo',
                        data: 'Articolo'
                    },

                    {
                        title: 'Quantità',
                        data: 'Quantita'
                    },
                    {
                        title: 'Stato',
                        data: 'Stato'
                    }]

            });
        })
});
$("#btnTest").click(function () {
    //bootbox.alert({
    //    message: o.data,
    //    backdrop: true,
    //    callback: function () {
    //        window.location.replace('invioedi');
    //    }

    //});
});


$("#btnInvia").click(function () {
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    varCliente = $("#idCliente").val();
    varNumero = $("#ddlTestate").val();

    //INSERIRE CHIAMATA AJAX

    //string cliente, string bolla, string bollaEffettiva, string trasportatore, string partenza
    $.ajax({
        url: "/DESADV/InvioEDI",
        type: 'POST',
        //async: false,
        data: { __RequestVerificationToken: token, cliente: varCliente, sNumero: varNumero },
        dataType: 'json',
        success: o => {
            if (o.status == '500') {
                $("#modalEsitoTitolo").text("ERRORE!");
                $("#modalEsitoBody").text("Si è verificato il seguente errore:" + o.data);
            }
            else {
                //alert("status: " + o.status + "" + "Messaggio: " + o.data);
                $("#modalEsitoTitolo").text("Spedizione effettuata!");
                $("#modalEsitoBody").text("La spedizione è stata effettuata correttamente!");
            }
            $("#modalEsito").modal('show');
        },
        error: o => {
            alert("ERRORE status: " + o.status + "" + "Messaggio: " + o.data);
            $("#modalEsitoTitolo").text("ERRORE!");
            $("#modalEsitoBody").text("Si è verificato il seguente errore:" + o.data);

            $("#modalEsito").modal('show');
            //$("#btnInvia")
        }

        //error: function (xhr) {alert('FALLITO: status richiesta:' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText); }
    });

    //$.post("/DESADV/InvioEDI?Cliente=" + varCliente + "&sNumero=" + varNumero + "&RequestVerificationToken=" + token)
    //    .done(o => {
    //        alert("status: " + o.status + "" + "Messaggio: " + o.data);
    //    });
});
$(document).ready(function () {
    var cliente = $("#idCliente").text();
    cliente = (cliente.charAt(0) !== '_' || cliente === null ? null : cliente.split('_').pop());
    var id = $("#ddlTestate").text();
    id = (id.charAt(0) !== '_' || id === null ? null : id.split('_').pop());
    $.get('/api/desadv/ListaClienti')
        .done(o => {
            $('#idCliente').empty();
            $('#idCliente').append('<option value="">Selezionare un cliente</option>');
            o.forEach(s => {
                var option = '<option value="' + s.id + '">' + s.id + ' - ' + s.nome + '</option>';
                $('#idCliente').append(option);
            })
            if (cliente !== null) {
                $('#idCliente').val(cliente);
            }

        });
    if (cliente !== null) {
        $.get('/api/desadv/' + (cliente !== null ? cliente + '?id=' : ''))
            .done(o => {
                $('#ddlTestate').empty();
                var i = 0;
                o.forEach(s => {
                    if (i == 0) {
                        var option = '<option selected="selected" value="' + s.NUMDES + '">' + s.CODEQUIP + '</option>';
                    }
                    else {
                        var option = '<option value="' + s.NUMDES + '">' + s.CODEQUIP + '</option>';
                    }
                    $('#ddlTestate').append(option);
                    i = i + 1;
                })
                if (o == null) {
                    $('#ddlTestate').append('<option value="">Selezionare una testata</option>');
                }
                if (id !== null) {
                    $('#ddlTestate').val(id);
                }

            });
    }
    $("#closeButton").click(function () {
        window.location.replace('invioedi?Cliente=_' + $("#idCliente").val());//+ '&sNumero=_' + o.ID);
    });
    $.get('/api/desadv/ListaSpedizioni/'+ (cliente !== null ? cliente :  ''  ))
        .done(o => {
            $('#spedizioni').DataTable({
                data: o,
                //order: [[0, 'desc'], [1, 'desc']],
                "ordering": false,
                "searching": false,
                "info": false,
                "lengthChange": false,
                createdRow: function (row, data, dataIndex) {
                    if (data["Stato"] == 'CHIUSO') {
                        $(row).addClass('spedizioneChiusa');
                    }
                    else {
                        $(row).addClass('spedizioneAperta');
                    }
                },
                destroy: true,
                
                columns: [
                    {
                        title: 'Cliente',
                        data: 'Cliente'
                    },
                    {
                        title: 'Numero',
                        data: 'Numero'
                    },
                    {
                        title: 'Data Spedizione',
                        data: 'DataSpedizione'
                    },

                    {
                        title: 'Bolla',
                        data: 'Bolla'
                    },
                    {
                        title: 'Articolo',
                        data: 'Articolo'
                    },

                    {
                        title: 'Quantità',
                        data: 'Quantita'
                    },
                    {
                        title: 'Stato',
                        data: 'Stato'
                    }]
                
            });
            })
    
});
