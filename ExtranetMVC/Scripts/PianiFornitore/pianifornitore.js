var testate;
var currentUser = $('#currentUsername').val();

$(document).ready(() => {
    $('#btnStampa').click(function () {
        $('#pRiepilogo').text("DELLORTO  " + $('#selFornitore option:selected').text() + " - " + $('#selOrdine option:selected').val() + " - " + $('#selTestata option:selected').text());
        window.print();
    });
    var el = document.getElementById("t");
    $('#selFornitore').bind("change", (() => {
        $.get('/api/ordine/' + $('#selFornitore option:selected').val())
            .done(o => {
                $('#selOrdine').empty();
                $('#selOrdine').append('<option value="">Selezionare un numero d\'ordine</option>');
                o.forEach(s => {
                    var option = '<option value="' + s + '">' + s + '</option>';
                    $('#selOrdine').append(option);
                });
                $.get('/api/user/' + $('#selFornitore option:selected').val())
                    .done(u => {
                        $('#nomeFornitore').text(u.FirstName);
                    });
            })
    }));
    
    $('#btnInviaPiano').bind("click", (() => {
        if (($('#selOrdine option:selected').val() != null && $('#selOrdine option:selected').val() != "") && $('#selFornitore option:selected').val() != 0) {
            $("#modalLabelTitolo").text("Ordine " + $('#selOrdine option:selected').val() + ' del fornitore ' + $('#selFornitore option:selected').val());
            $("#modal").modal('show');
        }
        else {
            alert("Seleziona fornitore e piano");
        }
         }));

    $.get('/api/user/' + currentUser)
        .done(u => {
            $('#nomeFornitore').text(u.FirstName);
        });
    $.get('/api/ordine/' + currentUser)
        .done(o => {
            $('#selOrdine').empty();
            $('#selOrdine').append('<option value="">Selezionare un numero d\'ordine</option>');
            o.forEach(s => {
                var option = '<option value="' + s + '">' + s + '</option>';
                $('#selOrdine').append(option);
            });
        });
    $('#selOrdine').change(() => {
        var ord = $('#selOrdine option:selected').val();
        //$.get('/api/testata/' + currentUser + '?ordine=' + ord)
        $.get('/api/testata/' + currentUser + '/' + ord)
            .done(t => {
                this.testate = t;
                $('#selTestata').empty();
                if (ord) {
                    var opts = t.map(ts => { return { id: ts.ID, descrizione: 'N. ' + ts.ID + ' del ' + ts.DataData } });
                    opts.forEach(s => {
                        var option = '<option value="' + s.id + '">' + s.descrizione + '</option>';
                        $('#selTestata').append(option);
                    });
                    var testataV = $('#selTestata option:selected').val();
                    $.get('/api/ordineChecked/' + currentUser + '/' + ord + '/' + testataV)
                        .done(a => { });
                    $('#lData').text(this.testate.filter(t => t.ID == $('#selTestata').val())[0].DATAVIS);
                    refreshTable(t);
                }
            });
        
    });
    $('#selTestata').change(() => {
        var ord = $('#selOrdine option:selected').val();
        var testataV = $('#selTestata option:selected').val();
        $.get('/api/ordineChecked/' + currentUser + '/' + ord + '/' + testataV)
            .done(a => { });
        // riempiamo il datatable
        refreshTable(this.testate);
        $('#lData').text(this.testate.filter(t => t.ID == $('#selTestata').val())[0].DATAVIS);
    });
});

function pInvio(a) {
    x = a;
    console.log(a);
    if (a.id == 'btnInvioNoMail') {
        email = 'N';
        testo = '';
    }
    else {
        email = 'S';
        testo = 'Controllare email per conferma';

    }
    $.get('/api/ordine/invio/' + $('#selOrdine option:selected').val() + '/' + $('#selFornitore option:selected').val() + '/' + email)
        .done(o => {
            $("#modal").modal('hide');
            if (o.status == '500') {
                alert("Si è verificato il seguente errore:\n" + o.data);
            }
            else {

                alert("Invio effettuato.\n " + testo);
            }

            //$("#modal").modal('hide');
        })
    
}

function printArea(areaName) {
    var printContents = document.getElementById(areaName).innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.style.width = "28cm";
    document.body.innerHTML = printContents;
    window.print();
    //document.body.innerHTML = originalContents;
}   
function PrintElem(elem) {
    var mywindow = window.open('', 'PRINT', 'height=21.5cm,width=28cm');

    mywindow.document.write('<html><body>')
    mywindow.document.write(document.getElementById(elem).innerHTML);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();
    return true;
}

function refreshTable(testate) {
    var righe = testate.filter(t => t.ID == $('#selTestata').val())[0].Righe;
    
    $('#righePiano').DataTable({
        "order": [[0, 'asc'], [1, 'asc']],
        "drawCallback": function (settings) {
            var api = this.api();   
            var rows = api.rows({ page: 'current' }).nodes();
            api.data().each(function (riga, i) {
                if (riga.TIPOLOGIA === 'U') {
                    $(rows).eq(i).before(
                        '<tr class="rigaGruppo ">' +
                        '<td>' + riga.ARTCOD + '</td>' +
                        '<td>' + riga.ARTVER + '</td>' +
                        '<td>' + riga.ARTDES + '</td>' +
                        '<td>' + riga.ARTUM + '</td>' +
                        '<td>' + (riga.LAST_DDT_F || '') + '</td>' +
                        '<td>' + (riga.LAST_DATA_SHORT || '') + '</td>' +
                        '<td>' + (riga.LAST_QTA || '') + '</td>' +
                        '<td>' + (riga.PROG_ARTICOLO || '') + '</td>' +
                        '</tr > '
                    );
                    $(rows).eq(i).addClass('rigaNascosta');
                }
            });
        },
        "paging": false,
        "ordering": false,
        "searching": false,
        "info": false,
        data: righe,
        destroy: true,
        columns: [
            {
                data: null,
                width: 150,
                "render": function (data, type, row, meta) {
                    return '';
                }
                
            },
            {
                data: null,
                width: 100,
                "render": function (data, type, row, meta) {
                    return '';
                }
            },
            {
                data: null,
                "render": function (data, type, row, meta) {
                    return '';
                }
            },
            {
                data: null,
                "render": function (data, type, row, meta) {
                    return '';
                }
            },
            {
                data: null,
                "render": function (data, type, row, meta) {
                    return '';
                }
            },
            {
                data: 'TIPOLOGIA'
            },
            {
                data: 'DATA_CONSEGNA_SHORT',
                width: 300
            },
            {
                data: 'ARTQTA'
            }
        ]
    });
}