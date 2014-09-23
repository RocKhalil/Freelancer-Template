using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_PublishChanges : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Admin"] == null || Session["Admin"].ToString() != "1")
            Response.Redirect("login.aspx");
        else
        {
            Util.SetFileContent("generated_content", "");
            Util.SetFileContent("generated_head", "");
            Response.Redirect("default.aspx");
        }
    }
}