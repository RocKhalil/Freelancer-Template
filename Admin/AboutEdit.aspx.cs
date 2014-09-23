using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Admin_AboutEdit : System.Web.UI.Page
{
    protected string PageHeader;
    dynamic obj;
    string id = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Admin"] == null || Session["Admin"].ToString() != "1")
            Response.Redirect("login.aspx");
        if (Request.QueryString["id"] == null)
            Response.Redirect("AboutListing.aspx");
        id = Request.QueryString["id"].ToString();

        PageHeader = Util.GetTemplate("Admin_header");
        string json = Util.GetFileContent("data");
        var serializer = new JavaScriptSerializer();
        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
        obj = serializer.Deserialize(json, typeof(object));

        if (!IsPostBack)
        {
            ddlColumns.Items.Add(new ListItem("1", "1"));
            ddlColumns.Items.Add(new ListItem("2", "2"));
            ddlColumns.Items.Add(new ListItem("3", "3"));
            ddlColumns.Items.Add(new ListItem("4", "4"));
            ddlColumns.Items.Add(new ListItem("5", "5"));
            ddlColumns.Items.Add(new ListItem("6", "6"));
            ddlColumns.Items.Add(new ListItem("7", "7"));
            ddlColumns.Items.Add(new ListItem("8", "8"));
            ddlColumns.Items.Add(new ListItem("9", "9"));
            ddlColumns.Items.Add(new ListItem("10", "10"));
            ddlColumns.Items.Add(new ListItem("11", "11"));
            ddlColumns.Items.Add(new ListItem("12", "12"));

            bool exists = false;
            foreach (dynamic obj1 in obj["About"])
            {
                if (obj1["id"] == id)
                {
                    txtValue.Text = obj1["content"];
                    ddlColumns.SelectedValue = obj1["cols"];
                    exists = true;
                }
            }
            if(!exists)
                Response.Redirect("AboutListing.aspx");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> ValuesToUpdate = new Dictionary<string, string>();
        ValuesToUpdate.Add("about-" + id, txtValue.Text);
        ValuesToUpdate.Add("about-cols-" + id, ddlColumns.SelectedValue);
        Util.UpdateDataFile(ValuesToUpdate, new List<string>(), new List<string>());
    }
}