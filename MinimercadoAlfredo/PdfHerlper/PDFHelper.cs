using Root.Reports;
using System;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace MinimercadoAlfredo
{
    public class PdfHelper : Report
    {
        private FontDef fontDef_Helvetica;
        private Double rPosLeft = 20;  // millimeters
        private Double rPosRight = 195;  // millimeters
        private Double rPosTop = 24;  // millimeters
        private Double rPosBottom = 278;  // millimeters

        public void CreatePdfReport(string[] arraySelected)
        {
            fontDef_Helvetica = new FontDef(this, FontDef.StandardFont.Helvetica);
            FontProp fontProp_Text = new FontPropMM(fontDef_Helvetica, 1.9);  // standard font
            FontProp fontProp_Header = new FontPropMM(fontDef_Helvetica, 1.9);  // font of the table header
            fontProp_Header.bBold = true;

            //Stream stream_Phone = GetType().Assembly.GetManifestResourceStream("ReportSamples.Phone.jpg");
            Random random = new Random(6);

            // INICIO TABLE HEADER
            TableLayoutManager tlm;
            using (tlm = new TableLayoutManager(fontProp_Header))
            {
                tlm.rContainerHeightMM = rPosBottom - rPosTop;  // set height of table
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += new TableLayoutManager.NewContainerEventHandler(Tlm_NewContainer);

                // define columns
                TlmColumn col;
                col = new TlmColumnMM(tlm, "Nº", 13);

                col = new TlmColumnMM(tlm, "Cliente", 40);
                col.tlmCellDef_Default.tlmTextMode = TlmTextMode.MultiLine;

                col = new TlmColumnMM(tlm, "Dirección", 36);

                col = new TlmColumnMM(tlm, "Fecha", 22);


                // open data set
                DataSet ds = new DataSet();
                try
                {

                    string connectionString = System.Configuration.ConfigurationManager.
                 ConnectionStrings["DefaultConnection"].ConnectionString;


                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT * FROM Sales",
                    connectionString);

                    da.Fill(ds);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL exception occurred: " + ex.Message);
                }


                DataTable dataTable_Sales = ds.Tables[0];

                foreach (DataRow dr in dataTable_Sales.Rows)
                {


                    tlm.NewRow();
                    tlm.Add(0, new RepString(fontProp_Text, (String)dr["CustomerID"]));
                    tlm.Add(1, new RepString(fontProp_Text, (String)dr["CompanyName"]));
                    tlm.Add(2, new RepString(fontProp_Text, (String)dr["Address"]));
                    tlm.Add(3, new RepString(fontProp_Text, (String)dr["City"]));
                    tlm.Add(4, new RepString(fontProp_Text, (String)dr["PostalCode"]));


                }
            }
            ////FIN TABLE HEADER


            ///////INCIO TABLE CONTENT
            TableLayoutManager tlmContent;
            using (tlmContent = new TableLayoutManager(fontProp_Header))
            {
                tlmContent.rContainerHeightMM = rPosBottom - rPosTop;  // set height of table
                tlmContent.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlmContent.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlmContent.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlmContent.eNewContainer += new TableLayoutManager.NewContainerEventHandler(Tlm_NewContainer);

                // define columns
                TlmColumn col;
                col = new TlmColumnMM(tlmContent, "Nº", 13);

                col = new TlmColumnMM(tlmContent, "Producto", 40);
                col.tlmCellDef_Default.tlmTextMode = TlmTextMode.MultiLine;

                col = new TlmColumnMM(tlmContent, "Cantidad", 36);

                col = new TlmColumnMM(tlmContent, "Precio", 22);

                col = new TlmColumnMM(tlmContent, "Descuento", 22);

                col = new TlmColumnMM(tlmContent, "Subtotal", 22);

                // open data set
                DataSet dsContent = new DataSet();
                try
                {

                    string connectionString = System.Configuration.ConfigurationManager.
                 ConnectionStrings["DefaultConnection"].ConnectionString;


                    SqlDataAdapter daContent = new SqlDataAdapter(
                        "SELECT * FROM Sales",
                    connectionString);

                    // Fill the DataSet.
                    daContent.Fill(dsContent);

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL exception occurred: " + ex.Message);
                }

                DataTable dataTable_Content = dsContent.Tables[0];

                foreach (DataRow dr in dataTable_Content.Rows)
                {


                    tlm.NewRow();
                    tlm.Add(0, new RepString(fontProp_Text, (String)dr["CustomerID"]));
                    tlm.Add(1, new RepString(fontProp_Text, (String)dr["CompanyName"]));
                    tlm.Add(2, new RepString(fontProp_Text, (String)dr["Address"]));
                    tlm.Add(3, new RepString(fontProp_Text, (String)dr["City"]));
                    tlm.Add(4, new RepString(fontProp_Text, (String)dr["PostalCode"]));


                }
            }
            ///////FIN TABLE CONTENT

            page_Cur.AddCT_MM(rPosLeft + tlm.rWidthMM / 2, rPosTop + tlm.rCurY_MM + 2, new RepString(fontProp_Text, "- end of table -"));

            // print page number and current date/time
            Double rY = rPosBottom + 1.5;
            foreach (Page page in enum_Page)
            {
                page.AddLT_MM(rPosLeft, rY, new RepString(fontProp_Text, DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString()));
                page.AddRT_MM(rPosRight, rY, new RepString(fontProp_Text, page.iPageNo + " / " + iPageCount));
            }
        }


        public void Tlm_NewContainer(Object oSender, TableLayoutManager.NewContainerEventArgs ea)
        {  // only "public" for NDoc, should be "private"
            new Page(this);

            // first page with caption
            if (page_Cur.iPageNo == 1)
            {
                FontProp fontProp_Title = new FontPropMM(fontDef_Helvetica, 7);
                fontProp_Title.bBold = true;
                page_Cur.AddCT_MM(rPosLeft + (rPosRight - rPosLeft) / 2, rPosTop, new RepString(fontProp_Title, "Customer List"));
                ea.container.rHeightMM -= fontProp_Title.rLineFeedMM;  // reduce height of table container for the first page
            }

            // the new container must be added to the current page
            page_Cur.AddMM(rPosLeft, rPosBottom - ea.container.rHeightMM, ea.container);
        }
    }
}