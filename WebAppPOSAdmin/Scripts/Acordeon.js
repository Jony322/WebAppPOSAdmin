    $(document).ready(function () {
        // JQUERY FUNCTION.
        $("#accordion").accordion({
            heightStyle: "content"
        });
    });
    // EXPAND AND COLLAPSE MENUS ON “HEADER CLICK”
    $("#accordion > h3").click(function () {
        $("#accordion > h3").animate({ 'background-color': '#FFF' }, 300);
        $("#accordion > h3").css({ 'color': '#000' });

        // HIGHLIGHT THE SELECTED HEADER (BLOCK)
        $(this).animate({ 'background-color': '#FA6305' }, 300);
        $(this).css({ 'color': '#FFF' });
    });
