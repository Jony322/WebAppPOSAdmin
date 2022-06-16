 function EditarPrecio(td) {

         var tdControl = $(td);
    if (tdControl != null && tdControl.innerText != "" 


           && $(td).attr("editable") != null 


         && $(td).attr("editable") == "true") {


               var oldValue = tdControl.innerHTML;


               tdControl.innerHTML = '<input id="txtEditPrecios" '

           + 'onblur="OcultarTxtEdit(this, true);" '


           + 'onkeypress="FiltradoKey(event, this);" '


          + 'class="txtEditPrecios" type="text" value="' 

           + oldValue + '" />';

         $(".txtEditPrecios").focus().select();     

         console.log(td.value);
    }
 }


  function OcultarTxtEdit(val, tabular) {
         if (val != null) {
                 var txt = $(val);
                 var td = txt.parentNode;
                 if (txt.defaultValue != txt.value) {
                          td.setAttribute("EditVal", txt.value);
                          td.className = "txtEdit";
                      }
                 var tdNext = $(td).next();
                 td.innerHTML = txt.value;     
                  if (tabular)
                          if($(tdNext).length)
                                EditarPrecio(tdNext);
                     else
                         if($(td).parent('tr').next().children().length)
                            EditarPrecio($(td).parent('tr').next().children())
             }
      }


   function OcultarTxtEditTodos() {


           var txts = $("#txtEditPrecios");


           for (var i = 0; i < txts.length; i++) {


                   OcultarTxtEdit($(txts[i]), false); 


               }


       }



  function FiltradoKey(e, txt) {
           var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 27) {
                OcultarTxtEdit(txt, false);
                console.log("adentro");
                }
            console.log("afuera");

        }