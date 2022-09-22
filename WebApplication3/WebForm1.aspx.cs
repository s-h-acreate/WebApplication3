﻿using System;
using Aspose.Pdf;
using Aspose.Pdf.Annotations;
using Aspose.Pdf.Text;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

using System.Diagnostics;

namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                iframepdf.Visible = false;

            }
            else
            {
                iframepdf.Visible = true;
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetCompletionList(string prefixText)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["RMConnectionString"].ConnectionString;
                using (SqlCommand com = new SqlCommand("spGetsMatchingProducts", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ProductNames", prefixText);
                    com.Connection = con;
                    con.Open();
                    List<string> countryNames = new List<string>();
                    using (SqlDataReader sdr = com.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            countryNames.Add(sdr["Name"].ToString());
                        }
                    }
                    con.Close();
                    return countryNames;
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string searchProduct = (txtSearch.Text).ToUpper();
            string storeNumber = "BUF039";
            string data = GetpdfText(searchProduct, storeNumber);
            if (String.IsNullOrEmpty(data)==true)
            {
                using (SqlConnection con = new SqlConnection())
                {

                    con.ConnectionString = ConfigurationManager.ConnectionStrings["RMConnectionString"].ConnectionString;
                    using (SqlCommand com = new SqlCommand("InsertMissingSearchedProducts", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductName", searchProduct);
                        com.Parameters.AddWithValue("@StoreNumber", storeNumber);
                        com.Connection = con;
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                }
                iframepdf.Visible = false;
                showMessage.Text = "Currently this product is not available. Please contact store invigilator for further inquiries";
                showMessage.Visible=true;
            }
            else
            {
                highlight(data);
                showMessage.Text = "Can find your Product in below ";
                showMessage.Visible = true;
            }
            
        }
        private string GetpdfText(string productName, string storeNo)
        {
            using (SqlConnection con = new SqlConnection())
            {
                string subCategory = string.Empty;
                con.ConnectionString = ConfigurationManager.ConnectionStrings["RMConnectionString"].ConnectionString;
                using (SqlCommand com = new SqlCommand("spGetsubcategory", con))
                {
                    com.CommandType = CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@Productname", productName);
                    com.Parameters.AddWithValue("@StoreNo", storeNo);
                    com.Connection = con;
                    con.Open();
                    List<string> lstSubCategory = new List<string>();
                    using (SqlDataReader sdr = com.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            lstSubCategory.Add(sdr["Subcategory"].ToString());
                        }
                    }
                    con.Close();
                    
                    if (lstSubCategory != null && lstSubCategory.Count > 0)
                    {
                        subCategory = lstSubCategory[0];
                    }
                    return subCategory;
                }
            }
        }

        private void highlight(string productName)
        {
            double txtX = 0;
            double txtY = 0;

            using (Document doc = new Document(@"C:\Users\PCTR0042045\source\repos\WebApplication3\WebApplication3\data\FP_6258 (1).pdf"))
            {
                // Search target text to highlight
                TextFragmentAbsorber textFragmentAbsorber = new TextFragmentAbsorber(productName);
                Document newDocument = new Document();
                Page page = doc.Pages[1];
                page.Accept(textFragmentAbsorber);

                // Create a highlight annotation
                if (textFragmentAbsorber.TextFragments.Count > 0)
                {
                    HighlightAnnotation ha = new HighlightAnnotation(page, textFragmentAbsorber.TextFragments[1].Rectangle);
                    TextFragmentCollection textFragmentCollection = textFragmentAbsorber.TextFragments;

                    foreach (TextFragment textFragment in textFragmentCollection)
                    {
                        txtX = textFragment.Position.XIndent;
                        txtY = textFragment.Position.YIndent;
                    }
                    // Specify highlight color 
                    ha.Color = Color.Yellow;
                    // Add annotation to highlight text in PDF 
                    page.Annotations.Add(ha);
                    using (FileStream imageStream = new FileStream(@"C:\Users\PCTR0042045\source\repos\WebApplication3\WebApplication3\data\logo1.png", FileMode.Open))
                    {
                        // Add image to Images collection of Page Resources
                        page.Resources.Images.Add(imageStream);
                        // Using GSave operator: this operator saves current graphics state
                        page.Contents.Add(new Aspose.Pdf.Operators.GSave());
                        //Coordinate of the starting letter(x, y)(450, 100)
                        //àStarting point of the pin(LLX, LLY)->x - 20,y + 20
                        //àSize(Square)(URX, URY)->LLX + 50,LLY + 50
                        double lowerLeftX = txtX - 30;
                        double lowerLeftY = txtY;
                        double upperRightX = lowerLeftX + 30;
                        double upperRightY = lowerLeftY + 30;
                        // Create Rectangle and Matrix objects
                        Aspose.Pdf.Rectangle rectangle = new Aspose.Pdf.Rectangle(lowerLeftX, lowerLeftY, upperRightX, upperRightY);
                        Matrix matrix = new Matrix(new double[] { rectangle.URX - rectangle.LLX, 0, 0, rectangle.URY - rectangle.LLY, rectangle.LLX, rectangle.LLY });
                        // Using ConcatenateMatrix (concatenate matrix) operator: defines how image must be placed
                        page.Contents.Add(new Aspose.Pdf.Operators.ConcatenateMatrix(matrix));
                        XImage ximage = page.Resources.Images[page.Resources.Images.Count];
                        // Using Do operator: this operator draws image
                        page.Contents.Add(new Aspose.Pdf.Operators.Do(ximage.Name));
                    }
                    newDocument.Pages.Add(page);
                    // Save the document
                    newDocument.Save(@"C:\Users\PCTR0042045\source\repos\WebApplication3\WebApplication3\data\highlight1.pdf");
                }
            }

        }

    }
}
