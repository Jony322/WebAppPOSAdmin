(function () {
    /**
     * Ajuste decimal de un número.
     *
     * @param {String}  tipo  El tipo de ajuste.
     * @param {Number}  valor El numero.
     * @param {Integer} exp   El exponente (el logaritmo 10 del ajuste base).
     * @returns {Number} El valor ajustado.
     */

    function decimalAdjust(type, value, exp) {
        // Si el exp no está definido o es cero...
        if (typeof exp === 'undefined' || +exp === 0) {
            return Math[type](value);
        }
        value = +value;
        exp = +exp;
        // Si el valor no es un número o el exp no es un entero...
        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
            return NaN;
        }
        // Shift
        value = value.toString().split('e');
        value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
        // Shift back
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
    }

    // Decimal round
    if (!Math.round10) {
        Math.round10 = function (value, exp) {
            return decimalAdjust('round', value, exp);
        };
    }
    // Decimal floor
    if (!Math.floor10) {
        Math.floor10 = function (value, exp) {
            return decimalAdjust('floor', value, exp);
        };
    }
    // Decimal ceil
    if (!Math.ceil10) {
        Math.ceil10 = function (value, exp) {
            return decimalAdjust('ceil', value, exp);
        };
    }
})();

function actualizarUtilidadArticulo(event) {
    var row = event.parentNode.parentNode;
    var rowIndex = row.rowIndex - 1;

    var pco = parseFloat(row.cells[3].getElementsByTagName("input")[0].value);
    var pve = parseFloat(row.cells[5].getElementsByTagName("input")[0].value);
    var iva = parseFloat(row.cells[6].getElementsByTagName("input")[0].value);

    var uti = (pve / (1.0 + iva) - pco) / pco * 100.0;

    var utilidad = row.cells[4].getElementsByTagName("input")[0].value = uti.toFixed(3);
    return false;
}

function actualizarPrecioVenta(event) {
    var row = event.parentNode.parentNode;
    var rowIndex = row.rowIndex - 1;

    var pco = parseFloat(row.cells[3].getElementsByTagName("input")[0].value);
    var uti = parseFloat(row.cells[4].getElementsByTagName("input")[0].value);
    var iva = parseFloat(row.cells[6].getElementsByTagName("input")[0].value);

    var pve = Math.round10(((pco * (1.0 + uti / 100.0)) * (1.0 + iva)), -1);

    var precio_venta = row.cells[5].getElementsByTagName("input")[0].value = pve.toFixed(3);
    return false;
}

function handleKeyUpOrDownEvent(event, o) {
    var row = o.parentNode;

    switch (event.keyCode) {
        /* Arrow key UP */
        case 38:

            if (row.parentNode.previousElementSibling.previousElementSibling != null) {
                row.parentElement.previousElementSibling.cells[row.cellIndex].getElementsByTagName("input")[0].focus();
                document.getElementById(document.activeElement.id).select();

            }

            break;
            /* Arrow key DOWN */
        case 13:
        case 40:

            if (row.parentNode.nextElementSibling != null) {
                row.parentElement.nextElementSibling.cells[row.cellIndex].getElementsByTagName("input")[0].focus();
                document.getElementById(document.activeElement.id).select();
            }

            break;
    }

    return true;
}

function validateNumberAndGotoNextCtrl(event, o) {
    var row = o.parentNode;

    try {

        switch (event.keyCode) {
            /* Arrow key UP */
            case 38:
                if (!validateNumber(o.value, "Kg")) throw "El campo sólo acepta valores númericos";
                if (row.parentNode.previousElementSibling.previousElementSibling != null) {
                    row.parentElement.previousElementSibling.cells[row.cellIndex].getElementsByTagName("input")[0].focus();
                    document.getElementById(document.activeElement.id).select();
                }
                break;
            /* Arrow key DOWN */
            case 13:
            case 40:
                if (!validateNumber(o.value, "Kg")) throw "El campo sólo acepta valores númericos";
                if (row.parentNode.nextElementSibling != null) {
                    row.parentElement.nextElementSibling.cells[row.cellIndex].getElementsByTagName("input")[0].focus();
                    document.getElementById(document.activeElement.id).select();
                }
                break;
        }

        return true;
    }
    catch (err) {
        alert(err);
        o.select();

        return false;
    }
}

function validateNumberAndGotoNextCtrlPedido(event, o, um) {
    var row = o.parentNode;

    try {

        switch (event.keyCode) {
            /* Arrow key UP */
            case 38:
                if (!validateNumber(o.value, um)) throw "El campo debe ser númerico y verifique la unidad de medida";
                if (row.parentNode.previousElementSibling.previousElementSibling != null) {
                    row.parentElement.previousElementSibling.cells[row.cellIndex].getElementsByTagName("input")[0].focus();
                    document.getElementById(document.activeElement.id).select();
                }
                break;
                /* Arrow key DOWN */
            case 13:
            case 40:
                if (!validateNumber(o.value, um)) throw "El campo debe ser númerico y verifique la unidad de medida";
                if (row.parentNode.nextElementSibling != null) {
                    row.parentElement.nextElementSibling.cells[row.cellIndex].getElementsByTagName("input")[0].focus();
                    document.getElementById(document.activeElement.id).select();
                }
                break;
        }

        return true;
    }
    catch (err) {
        alert(err);
        o.select();

        return false;
    }
}

function validateNumber(number, um) {
    if (um == "Kg" || um == "Gms") return new RegExp(/^\d+(?:\.\d{1,3})?$/).test(number.toString());
    else return new RegExp(/^[0-9]{1,9}$/).test(number.toString());
}

function validateIntegerNumber(e)
{
    var key = e.charCode || e.keyCode || 0;
    return key == 8 || key == 9 || (key >= 48 && key <= 57);
}

function selected_row(ctrl)
{
    $('.selected').removeClass('selected');
    $(ctrl).parents('tr').addClass('selected');
}