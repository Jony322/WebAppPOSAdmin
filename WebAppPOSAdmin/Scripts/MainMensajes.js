$(document).on('ready', function ()
{
    //$("#btnAsociar").click(function (e)
    //{
    //    e.preventDefault();
    //    return false;
    //})
});

function mensajeErroneo(e)
{
    e.preventDefault();
    $("#msgError").RemoveClass("esconder").slideDown("slow");
}

function mensajeCorrecto(e) {
    e.preventDefault();
    $("#msgCorrecto").RemoveClass("esconder").slideDown("slow");
}