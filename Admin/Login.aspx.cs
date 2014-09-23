using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Admin"] != null && Session["Admin"].ToString() == "1")
            Response.Redirect("default.aspx");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Username = txtUsername.Text.ToLower();
        string Password = Util.HashStringSHA1(txtPassword.Text);

        string aUsername = ConfigurationManager.AppSettings["AdminUsername"].ToString().ToLower();
        string aPassword = ConfigurationManager.AppSettings["AdminPassword"].ToString();

        if (Username == aUsername && Password == aPassword)
        {
            Session["Admin"] = "1";
            Response.Redirect("default.aspx");
        }
    }
}