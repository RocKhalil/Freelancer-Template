using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Admin_Footer : System.Web.UI.Page
{
    protected string PageHeader;
    dynamic obj;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Admin"] == null || Session["Admin"].ToString() != "1")
            Response.Redirect("login.aspx");
        PageHeader = Util.GetTemplate("Admin_header");
        string json = Util.GetFileContent("data");
        var serializer = new JavaScriptSerializer();
        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
        obj = serializer.Deserialize(json, typeof(object));

        if (!IsPostBack)
        {
            txtCol1.Text = obj["Footer"][0]["col1"];
            txtCol2.Text = obj["Footer"][0]["col2"];
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> ValuesToUpdate = new Dictionary<string, string>();
        ValuesToUpdate.Add("footer-col1", txtCol1.Text);
        ValuesToUpdate.Add("footer-col2", txtCol2.Text);
        Util.UpdateDataFile(ValuesToUpdate, new List<string>(), new List<string>());
    }
}