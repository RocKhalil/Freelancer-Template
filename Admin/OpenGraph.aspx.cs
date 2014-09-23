using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Admin_OpenGraph : System.Web.UI.Page
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

        CreateFields();
    }
    private void CreateFields()
    {
        TextBox txt;
        LiteralControl MyControl;
        Panel MyPanel, BigPanel = null;

        foreach (dynamic obj1 in obj["OpenGraph"])
        {
            #region Create the big div
            BigPanel = new Panel();
            BigPanel.CssClass = "form-group";
            BigPanel.ID = "div_" + obj1["key"];
            #endregion
            #region Create the label -- propname
            MyControl = new LiteralControl();
            MyControl.Text = "<label>" + obj1["display"] + " :</label>";
            MyPanel = new Panel();
            MyPanel.CssClass = "controls";
            MyPanel.ID = "underdiv_" + obj1["key"];
            #endregion
            #region Create the textbox
            txt = new TextBox();
            txt.CssClass = "form-control";
            txt.ID = "field_" + obj1["key"];

            //Fill its value
            txt.Text = obj1["value"];

            MyPanel.Controls.Add(txt);
            #endregion
            #region Add everything to the big div
            BigPanel.Controls.Add(MyControl);
            BigPanel.Controls.Add(MyPanel);
            #endregion

            fields.Controls.Add(BigPanel);
        }
                
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> ValuesToUpdate = new Dictionary<string, string>();
        TextBox txt;
        string Value;
        dynamic obj1;
        for (int i = 0; i < obj["OpenGraph"].Count; i++)
        {
            obj1 = obj["SiteConfigurations"][i];
            txt = (TextBox)fields.FindControl("div_" + obj1["key"]).FindControl("underdiv_" + obj1["key"]).FindControl("field_" + obj1["key"]);
            Value = txt.Text.Trim();
            ValuesToUpdate.Add("opengraph-" + obj1["key"], Value);
        }
        Util.UpdateDataFile(ValuesToUpdate, new List<string>(), new List<string>());
    }
}