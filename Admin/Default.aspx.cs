using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Default : System.Web.UI.Page
{
    protected string PageHeader;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Admin"] == null || Session["Admin"].ToString() != "1")
            Response.Redirect("login.aspx");

        PageHeader = Util.GetTemplate("Admin_header");
    }
}