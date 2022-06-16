using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Utilities
{
    public partial class Basculas : System.Web.UI.Page
    {

        private enum FileType
        {
            toDAT,
            toXLS
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnExportToXLS_Click(object sender, EventArgs e)
        {
            exportDataToFile(FileType.toXLS);
        }

        protected void btnExportToDAT_Click(object sender, EventArgs e)
        {
            exportDataToFile(FileType.toDAT);
            EnableViewState = false;
        }

        private void exportDataToFile(FileType tipo)
        {
            base.Response.Clear();
            base.Response.Buffer = true;
            string empty = string.Empty;
            switch (tipo)
            {
                case FileType.toDAT:
                    {
                        StringBuilder data = new StringBuilder();
                        IOrderedEnumerable<sp_exportToDATResult> source2 = from d in new dcContextoSuPlazaDataContext().sp_exportToDAT()
                                                                           orderby d.Interno
                                                                           select d;
                        Encoding encoding = Encoding.GetEncoding(437);
                        base.Response.ContentType = "text/plain";
                        base.Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Plus.dat", empty));
                        source2.ToList().ForEach(delegate (sp_exportToDATResult row)
                        {
                            data.AppendLine(string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\" \"{9}\" \"{10}\"", row.Campo1 ?? "", row.Campo2 ?? "", row.Interno ?? "", row.Descripcion ?? "", row.Precio ?? "", row.Pesable ?? "", row.Campo3 ?? "", row.Campo4 ?? "", row.Campo5 ?? "", row.Campo6 ?? "", row.Codigo ?? ""));
                        });
                        HttpContext.Current.Response.BinaryWrite(encoding.GetBytes(data.ToString()));
                        break;
                    }
                case FileType.toXLS:
                    {
                        empty = $"FILE_{$"{DateTime.Now:yyyyMMddHHmmss}"}";
                        using (StringWriter stringWriter = new StringWriter())
                        {
                            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                            {
                                IOrderedEnumerable<sp_exportToXLSResult> source = from d in new dcContextoSuPlazaDataContext().sp_exportToXLS()
                                                                                  orderby d.COL11
                                                                                  select d;
                                base.Response.ContentType = "application/vnd.ms-excel";
                                base.Response.AddHeader("Content-Disposition", $"attachment;filename={empty}.xls");
                                List<string> obj = new List<string>
                    {
                        "[1.Departamento]", "[2.PLU No]", "[4.PLU Tipo]", "[11.CodiArt]", "[10.Nombre]", "[30.Nombre2]", "[31.Nombre3]", "[9.Grupo No]", "[80.Etiqueta]", "[81.Etiqueta Aux No]",
                        "[55.Origen No]", "[5.Peso Unit]", "[100.Pesofijo]", "[3.refijo]", "[14.Piezas]", "[15.Cant Unit No]", "[26.Usar tipo precio prefijo]", "[Precio]", "[91.PrecioEspecial]", "[8.Iva No]",
                        "[13.Tara]", "[12.tar No]", "[24.%Tara]", "[23.% limite tara]", "[85.Barcode ID]", "[86.Barcode ID2]", "[20.FechaPreparacion]", "[18.Fecha Empaque]", "[19.Tiempo Empaque]", "[16.Fecha de venta]",
                        "[17.Hora de venta]", "[22.Fecha de Elab]", "[25.Ingrediente No]", "[35.Traceability No]", "[50.Bonos]", "[70.Infonutric No]", "[90.Sales Msg No]", "[71.Referencia Dept]", "[69.Referencia PLU]", "[64.Dept Unido]",
                        "[68.PLU Unido]", "[60.# de PLU ligado]", "[61.Ligado Dept1]", "[65.Ligado PLU1]", "[62.Ligado Dept2]", "[66.Ligado PLU2]", "[99.Direct Ingredient]", "[101.PLU Picture]", "[102.SafetyCode]"
                    };
                                Table tableXLS = new Table();
                                TableRow fila = new TableRow();
                                foreach (string item in obj)
                                {
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = item
                                    });
                                }
                                tableXLS.Rows.Add(fila);
                                source.ToList().ForEach(delegate (sp_exportToXLSResult row)
                                {
                                    fila = new TableRow();
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL01
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL02
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL04
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL11
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL10
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL30
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL31
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL09
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL80
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL81
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL55
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL05
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL100
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL03
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL14
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL15
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL26
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.Precio.ToString()
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL91
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL08
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL13
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL12
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL24
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL23
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL85
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL86
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL20
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL18
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL19
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL16
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL17
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL22
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL25
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL35
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL50
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL70
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL90
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL71
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL69
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL64
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL68
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL60
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL61
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL65
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL62
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL66
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL99
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL101
                                    });
                                    fila.Cells.Add(new TableCell
                                    {
                                        Text = row.COL102
                                    });
                                    tableXLS.Rows.Add(fila);
                                });
                                tableXLS.RenderControl(writer);
                            }
                            HttpContext.Current.Response.Write(stringWriter.ToString());
                        }
                        break;
                    }
            }
            HttpContext.Current.Response.End();
        }
    }
}