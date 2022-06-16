
function calcularUtilidadArticulo() {
    var pco = $("#MainContent_txtCosto").val();
    var pve = $("#MainContent_txtPrecioVenta").val();
    var iva = $("#MainContent_txtIva").val();

    var uti = (pve / (1.0 + iva / 100.0) - pco) / pco * 100.0;

    $("#MainContent_txtUtilidad").val(isNaN(uti) ? (0).toFixed(3) : uti.toFixed(3));
}

function calcularPrecioVentaArticulo() {
    var pco = $("#MainContent_txtCosto").val();
    var uti = $("#MainContent_txtUtilidad").val();
    var iva = $("#MainContent_txtIva").val();

    document.getElementById("MainContent_chkIVA").checked = (parseFloat(iva) > 0.0)

    var pve = Math.round10(((pco * (1.0 + uti / 100.0)) * (1.0 + iva / 100.0)), -1);

    $("#MainContent_txtPrecioVenta").val(isNaN(pve) ? (0).toFixed(3) : pve.toFixed(3));

}

function setIVA(ctrl) {

    $("#MainContent_txtIva").val(ctrl.checked ? (iva * 100).toFixed(3) : (0).toFixed(3));

    calcularPrecioVentaArticulo();
    calcularUtilidadArticulo();
}

function calcularCostoAnexoAndPrecioVenta() {
    var pzs = $("#MainContent_txtPiezasAnexo").val();
    var pco = $("#MainContent_txtCosto").val();

    var cto = parseFloat(pco) * parseFloat(pzs);

    $("#MainContent_txtCostoAnexo").val(isNaN(cto) ? (0).toFixed(3) : cto.toFixed(3));

    calcularPrecioVentaAnexo();
}

function calcularUtilidadAnexo() {
    var pco = $("#MainContent_txtCostoAnexo").val();
    var pve = $("#MainContent_txtPrecioVentaAnexo").val();
    var iva = $("#MainContent_txtIva").val();

    var uti = (pve / (1.0 + iva / 100.0) - pco) / pco * 100.0;

    $("#MainContent_txtUtilidadAnexo").val(isNaN(uti) ? (0).toFixed(3) : uti.toFixed(3));
}

function calcularPrecioVentaAnexo() {
    var pco = $("#MainContent_txtCostoAnexo").val();
    var uti = $("#MainContent_txtUtilidadAnexo").val();
    var iva = $("#MainContent_txtIva").val();

    document.getElementById("MainContent_chkIVA").checked = (parseFloat(iva) > 0.0)

    var pve = Math.round10(((pco * (1.0 + uti / 100.0)) * (1.0 + iva / 100.0)), -1);

    $("#MainContent_txtPrecioVentaAnexo").val(isNaN(pve) ? (0).toFixed(3) : pve.toFixed(3));

}

function CalcularTotalArticulo() {
    var can = $("#MainContent_txtCantidad").val();
    var pun = $("#MainContent_txtPrecioVta").val();

    var tot = Math.round10(pun * can, -1);

    $("#MainContent_txtTotalItem").val(isNaN(tot) ? (0).toFixed(2) : tot.toFixed(2));
}