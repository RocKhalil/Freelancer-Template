using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Admin_PortfolioEdit : System.Web.UI.Page
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
            bool exists = false;
            foreach (dynamic obj1 in obj["Portfolio"])
            {
                if (obj1["id"] == id)
                {
                    txtTitle.Text = obj1["title"];
                    txtDescription.Text = obj1["description"];
                    txtSiteLink.Text = obj1["sitelink"];
                    txtClient.Text = obj1["client"];
                    txtDate.Text = obj1["date"];
                    txtService.Text = obj1["service"];
                    imgImage.ImageUrl = "../" + obj1["image"];
                    exists = true;
                }
            }
            if(!exists)
                Response.Redirect("PortfolioListing.aspx");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> ValuesToUpdate = new Dictionary<string, string>();
        if (fuImage.HasFile)
        {
            //save then change the value
            string mImage = "Images/Portfolio/" + fuImage.PostedFile.FileName;
            fuImage.PostedFile.SaveAs(Server.MapPath("~/" + mImage));
            imgImage.ImageUrl = "../" + mImage;
            ValuesToUpdate.Add("portfolio-image-" + id, mImage);
        }
        ValuesToUpdate.Add("portfolio-title-" + id, txtTitle.Text);
        ValuesToUpdate.Add("portfolio-description-" + id, txtDescription.Text);
        ValuesToUpdate.Add("portfolio-sitelink-" + id, txtSiteLink.Text);
        ValuesToUpdate.Add("portfolio-client-" + id, txtClient.Text);
        ValuesToUpdate.Add("portfolio-date-" + id, txtDate.Text);
        ValuesToUpdate.Add("portfolio-service-" + id, txtService.Text);
        Util.UpdateDataFile(ValuesToUpdate, new List<string>(), new List<string>());
    }
}