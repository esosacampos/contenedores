using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEPA.CCO.UI.Web.CLIENTES
{
    public partial class wfAyuda : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //string FilePath = Server.MapPath("Manual.pdf");
                //WebClient User = new WebClient();
                //Byte[] FileBuffer = User.DownloadData(FilePath);
                //if (FileBuffer != null)
                //{
                //    Response.ContentType = "application/pdf";
                //    //Response.ContentType = "application/octet-stream";
                //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                //    Response.BinaryWrite(FileBuffer);
                //} protected void Page_Load(object sender, EventArgs e)

              
              Response.Clear();
              Response.AddHeader("content-disposition", "attachment;filename=Manual.pdf");

              Response.ContentType = "application/pdf";
              Response.WriteFile(Server.MapPath("Manual.pdf"));
  
              Response.End();

            }
        }
    }
}