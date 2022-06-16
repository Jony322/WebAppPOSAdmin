//function completeCategories() {
//    $("#txtAutoComplete").autocomplete({
//        source: function (request, response) {
//            $.ajax({
//                type: "POST",
//                contentType: "application/json; charset=utf-8",
//                url: "Default.aspx/GetCategory",
//                data: "{'term':'" + $("#txtAutoComplete").val() + "'}",
//                dataType: "json",
//                success: function (data) {
//                    response(data.d);
//                },
//                error: function (result) {
//                    alert("Error");
//                }
//            });
//        }
//    });
//}

