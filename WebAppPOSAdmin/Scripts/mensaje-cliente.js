function getParameter(param) {
    var query = window.location.search.substring(1);
    var params = query.split("&");

    for (var p in params) {
        var key = params[p].split("=");
        if (key[0] == param) return key[1];
    }

    return null;
    /*
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == param) {
            return pair[1];
        }
    }*/
}

$(document).ready(function () {
    $("#cerrar,.close").click(function (e) {
        e.preventDefault();
        $('.modal-contenedor').slideUp('slow');
    });
});

function mostrarmodal() {
    $('#notificaciones').load();
    $('.modal-contenedor').removeClass('esconder');
    $('.modal-contenedor').slideDown('slow');
}

function insercionCorrecta() {
    $('#modal-head').addClass('correcto');
    $('.modal-title').html(" <span class='glyphicon glyphicon-ok'></span> &nbsp&nbsp Enhorabuena ");
    $('.modal-body').html(" <h4>Datos insertados correctamente</h4>");
    $('#cerrar').addClass('btn btn-success');
    this.mostrarmodal();
}

function insercionErronea() {
    $('#modal-head').addClass('error');
    $('.modal-title').html(" <span class='glyphicon glyphicon-ok'></span> &nbsp&nbsp Error ");
    $('.modal-body').html(" <h4>Ocurrió un error, verifique con el administrador</h4>");
    $('#cerrar').addClass('btn btn-danger');
    this.mostrarmodal();
}

function actualizacionCorrecta() {
    $('#modal-head').addClass('correcto');
    $('.modal-title').html(" <span class='glyphicon glyphicon-ok'></span> &nbsp&nbsp Enhorabuena ");
    $('.modal-body').html(" <h4>Actualización realizada con éxito</h4>");
    $('#cerrar').addClass('btn btn-success');
    this.mostrarmodal();
}

function actualizacionErronea() {
    $('#modal-head').addClass('error');
    $('.modal-title').html(" <span class='glyphicon glyphicon-ok'></span> &nbsp&nbsp Error ");
    $('.modal-body').html(" <h4>Ocurrió un error, verifique con el administrador</h4>");
    $('#cerrar').addClass('btn btn-danger');
    this.mostrarmodal();
}

function sinResultados() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Información ");
    $('.modal-body').html(" <h4>No se encontrarón resultados.</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function MyMessageBox(title, message, modal) {
    $('#modal-head').addClass(modal);
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp " + title);
    $('.modal-body').html(" <h4>" + message + "</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function validacionFechas() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Mensaje del sistema ");
    $('.modal-body').html(" <h4>Verifique las fechas no pueden ser vacias</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function validacionDrop() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Mensaje del sistema ");
    $('.modal-body').html(" <h4>Seleccione un item diferente de seleccionar!</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function validacionGridView() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Mensaje del sistema ");
    $('.modal-body').html(" <h4>No hay elementos en la tabla.</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function validacionFiltro() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Mensaje del sistema ");
    $('.modal-body').html(" <h4>No se hay datos entro de la tabla.</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function validacionLinea() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Mensaje del sistema ");
    $('.modal-body').html(" <h4>Seleccione una línea.</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function suspencionCorrecta() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Mensaje del sistema ");
    $('.modal-body').html(" <h4>Suspención correcta.</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function suspencionErronea() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Mensaje del sistema ");
    $('.modal-body').html(" <h4>Ocurrió un error.</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}

function validacionImagenes() {
    $('#modal-head').addClass('info');
    $('.modal-title').html(" <span class='glyphicon glyphicon-info-sign'></span> &nbsp&nbsp Mensaje del sistema ");
    $('.modal-body').html(" <h4>Sólo formato de imagen (png,jpg,jpeg,bmp).</h4>");
    $('#cerrar').addClass('btn btn-default');
    this.mostrarmodal();
}


function deployAccordionDptos(index) {
    setTimeout(function () {
        $("#accordion").accordion("option", "active", index);
    }, 1e2)
}


function getEstadistica(barCode) {
    $.ajax({
        type: "POST",
        url: "/Resources.aspx/getEstadistica",
        data: JSON.stringify({ barCode: barCode }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            $(JSON.parse(res.d)).each(function (index, e) {
                var anio = index == 0 ? "#actual_" : "#pasado_";
                var meses = ["ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic"];
                for (var mes in meses) $(anio + meses[mes]).val(parseFloat(e[meses[mes]]).toFixed(1));
                //$(anio+mes)
            })
        }
    });
}

function changeTab(tabNum) {
    $('#tab' + tabNum).tabs();
    document.getElementById('ctl01').reset();
}


function getCodigo(typeCode, txtBox) {
    $.ajax({
        type: "POST",
        url: "/Resources.aspx/getCodigo",
        data: JSON.stringify({ typeCode: typeCode }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            $("#" + txtBox).val(res.d);
            document.getElementById(txtBox).select();
        }
    });
}

function validateBarCode(ctrl) {
    if (ctrl.value != "")
        if (ctrl.value != BarCode) {
            $.ajax({
                type: "POST",
                url: "/Resources.aspx/validateBarCode",
                data: JSON.stringify({ barCode: ctrl.value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d != null) window.location = "./CatArticulos.aspx?barCode=" + res.d;
                    else {
                        cleanFieldsArticulo();
                        statusCRUD = "create";
                    }
                }
            });
        }
}

function validateBarCodeAnexo(ctrl) {

    if (ctrl.value != "") {
        if (ctrl.value == BarCode) {
            alert('El código ingresado es el principal');
            ctrl.select();
            return false;
        }
        else {
            $.ajax({
                type: "POST",
                url: "/Resources.aspx/validateBarCode",
                data: JSON.stringify({ barCode: ctrl.value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d != null) window.location = "./CatArticulos.aspx?barCode=" + res.d;
                    else {
                        //cleanFieldsArticulo();
                        statusCRUD = "create";
                    }
                }
            });
            return true;
        }
    }
}

function validateBarCodeKit(ctrl) {
    if (ctrl.value != "")
        if (ctrl.value != BarCode) {

            $.ajax({
                type: "POST",
                url: "/Resources.aspx/validateBarCodeKit",
                data: JSON.stringify({ barCode: ctrl.value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d != null) {

                        if (!JSON.parse(res.d).length > 0) return;

                        var item = JSON.parse(res.d)[0];

                        if (item.kit) window.location = "./CatKitArticulo.aspx?barCode=" + item.cod_barras;
                        else {
                            alert("El artículo ya existe");
                            ctrl.select();
                        }
                    }
                    else {
                        //cleanFieldsArticulo();
                        statusCRUD = "create";
                    }
                }
            });

        }
}

function cleanFieldsArticulo() {
    $("#MainContent_txtCodIntero").val("");
    $("#MainContent_txtDescripcion").val("");
    $("#MainContent_txtDescripcionCorta").val("");
    $("#MainContent_txtPiezas").val(1);
    //$("#MainContent_txtStock").val(0);
    document.getElementById("MainContent_txtStock").value = 0;
    $("#MainContent_txtStockMinimo").val(0);
    $("#MainContent_txtStockMaximo").val(0);
    $("#MainContent_txtCosto").val((0).toFixed(2));
    $("#MainContent_txtUtilidad").val((0).toFixed(3));
    $("#MainContent_txtPrecioVenta").val((0).toFixed(2));
    //document.getElementById("chkIVA").checked = false;
}

function validateInternalCode(ctrl) {
    if (ctrl.value != "")
        if (ctrl.value != InternalCode) {
            $.ajax({
                type: "POST",
                url: "/Resources.aspx/validateInternalCode",
                data: JSON.stringify({ internalCode: ctrl.value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res != null)
                        if (res.d) {
                            alert("El Código Interno ingresado ya existe!!!");
                            ctrl.select();
                        }
                }
            });
        }
}

function loadArticulos() {
    $.ajax({
        type: "POST",
        url: "/Resources.aspx/getArticulosPrincipales",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            var r = res;
            alert(res.d);
            if (res.d != null) {
                $(JSON.parse(res.d)).each(function (index, e) {
                    alert(e.cod_barras);
                });
            }
        },
        fail: function (err) {
            alert(err)
        },
        error: function (xhr, msg) {
            alert(msg);
        }

    });
}

function copyText(ctrl, id) {
    var origen = ctrl.value;
    var destino = $("#MainContent_" + id).val();

    if (origen != "" && destino == "") {
        if (origen.length <= 30)
            $("#MainContent_" + id).val(origen);
        else
            $("#MainContent_" + id).val(origen.substring(0,29));
    }
}

function findArticulo(findOption, findText, kit) {
    var opt = findOption;
    var txt = findText;

    var resource = window.location.pathname;

    $('#tableRecords > tbody').empty().hide();

    if (findText == "") return false;

    $.ajax({
        type: "POST",
        url: "/Resources.aspx/findArticulos",
        data: JSON.stringify({ findOption: findOption, findText: findText, kit: kit }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            $(JSON.parse(res.d)).each(function (i, o) {
                $('#tableRecords > tbody')
                    .append($("<tr>")
                        .append($("<td>", { text: o.cod_barras, class: "text-center" }))
                        .append($("<td>", { text: o.cod_interno == null ? "" : o.cod_interno, class: "text-center" }))
                        .append($("<td>", { text: o.descripcion }))
                        .append($("<td>", { text: o.stock, class: "text-center" }))
                        .append($("<td>", { class: "text-center" })
                            .append($("<a>", { href: resource + "?barCode=" + o.cod_barras })
                                .append($("<img>", { src: "/Images/editar.png" })))
                        )
                    );
            });
            $('#tableRecords > tbody').slideDown();
        }
    });
}

function findArticuloToKit(findOption, findText) {

    if (findText == "") return false;

    $('#tableRecords2 > tbody').empty().hide();

    $.ajax({
        type: "POST",
        url: "/Resources.aspx/findArticulos",
        data: JSON.stringify({ findOption: findOption, findText: findText, kit: false }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            $(JSON.parse(res.d)).each(function (i, o) {
                $('#tableRecords2 > tbody')
                    .append($("<tr>")
                        .append($("<td>", { text: o.cod_barras, class: "text-center" }))
                        .append($("<td>", { text: o.cod_interno == null ? "" : o.cod_interno, class: "text-center" }))
                        .append($("<td>", { text: o.descripcion }))
                        .append($("<td>", { class: "text-center" })
                            .append($("<button>", {
                                'id': "btn_" + o.cod_barras,
                                'onclick': "setBarCodeKit('" + o.cod_barras + "'); return false;",
                                'text': "Seleccionar",
                                'data-dismiss': "modal"
                            }))
                        )
                    );
            });
            $('#tableRecords2 > tbody').slideDown();
        }
    });
}

function resetFinderKit() {
    $("#tableRecords2 > tbody").empty();
    $("#findText2").val("").focus().select();

    return false;
}

function setBarCodeKit(barCode) {
    $("#MainContent_txtBarCodeToKit")
        .val(barCode)
        .focus();
    //$('#myModal').modal('hide');
}

function getArticuloAnexo(barCode) {

    if (findText == "") return;

    $.ajax({
        type: "POST",
        url: "/Resources.aspx/getArticuloAnexo",
        data: JSON.stringify({ barCode: barCode }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            if (res.d != null) {
                var a = JSON.parse(res.d)[0];
                $("#MainContent_txtCodBarrasAnexo").val(a.cod_barras);
                $("#MainContent_txtCodInternoAnexo").val(a.cod_interno);
                $("#MainContent_txtDescripcionAnexo").val(a.descripcion);
                $("#MainContent_txtDescripcionLargaAnexo").val(a.descripcion_larga);
                $("#MainContent_ddlUnidadAnexo").val(a.id_unidad);
                $("#MainContent_txtPiezasAnexo").val(a.cantidad_um);
                $("#MainContent_txtCostoAnexo").val(a.precio_compra.toFixed(3));
                $("#MainContent_txtUtilidadAnexo").val(a.utilidad.toFixed(3));
                $("#MainContent_txtPrecioVentaAnexo").val(a.precio_venta.toFixed(3));
            }
        },
        error: function (e) {
            alert(e);
        }
    });
}

function hideShowEffect(id) {
    $("#MainContent_" + id).hide();
    $("#MainContent_" + id).slideDown(1e3);

}

function setDataOffers() {
    var desc = $("#MainContent_txtDescOffer").val();
    document.getElementById("MainContent_txtDescOffer").setAttribute("value", desc);

    var fec_ini = $("#MainContent_txtFecha_ini").val();
    document.getElementById("MainContent_txtFecha_ini").setAttribute("value", fec_ini);

    var fec_fin = $("#MainContent_txtFecha_fin").val();
    document.getElementById("MainContent_txtFecha_fin").setAttribute("value", fec_fin);

    var SuspendeOfferID = $("#MainContent_ddlOfertas").val();

    document.getElementById("MainContent_txtFecha_fin").setAttribute("value", fec_fin);

    return false;
}

function WhatDoIDoChangePrice() {
    jQuery.noConflict();

    $('#myModal').modal('show');

    return false;
}

function showKITsAffected() {
    jQuery.noConflict();

    $('#myModalKITs').modal('show');

    return false;
}