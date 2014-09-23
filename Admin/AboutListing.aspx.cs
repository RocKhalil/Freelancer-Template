using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Admin_AboutListing : System.Web.UI.Page
{
    protected string PageHeader;
    dynamic obj;
    protected string TableContent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Admin"] == null || Session["Admin"].ToString() != "1")
            Response.Redirect("login.aspx");

        if (Request.QueryString["del"] != null)
        {
            string delid = Request.QueryString["del"].ToString();
            List<string> Delete = new List<string>();
            Delete.Add("about-" + delid);
            Util.UpdateDataFile(new Dictionary<string, string>(), Delete, new List<string>());
        }
        if (Request.QueryString["add"] != null)
        {
            List<string> Add =new List<string>();
            Add.Add("about");
            Util.UpdateDataFile(new Dictionary<string, string>(), new List<string>(), Add);
        }

        PageHeader = Util.GetTemplate("Admin_header");
        string json = Util.GetFileContent("data");
        var serializer = new JavaScriptSerializer();
        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
        obj = serializer.Deserialize(json, typeof(object));

        TableContent = "";
        string Row = "<tr style='cursor:pointer'>";
        Row += "<td onclick='window.location.href=\"AboutEdit.aspx?id=$id$\"'>$id$</td>";
        Row += "<td onclick='window.location.href=\"AboutEdit.aspx?id=$id$\"'>$cols$</td>";
        Row += "<td onclick='window.location.href=\"AboutEdit.aspx?id=$id$\"'>$content$</td>";
        Row += "<td><a href='javascript:;' onclick='window.location.href=\"AboutListing.aspx?del=$id$\"'><i class=\"fa fa-trash-o\"></i></a></td>";
        Row += "</tr>";
        foreach (dynamic obj1 in obj["About"])
        {
            TableContent += Row.Replace("$id$",obj1["id"]).Replace("$cols$",obj1["cols"]).Replace("$content$",obj1["content"]);
        }
    }
}