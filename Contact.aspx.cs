using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Contact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string name = Request.Params["name"].ToString();
            string phone = Request.Params["phone"].ToString();
            string email = Request.Params["email"].ToString();
            string message = Request.Params["message"].ToString();
            int SentMail = Util.SendMail(ConfigurationManager.AppSettings["MyEmail"].ToString(), email, message, name + " contacted you via your website");

            if (SentMail == 1)
                Response.StatusCode = 200;
            else
                Response.StatusCode = 400;
        }
        catch
        {
            Response.StatusCode = 400;            
        }
        Response.End();
    }
}