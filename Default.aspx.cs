using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class _Default : System.Web.UI.Page
{
    protected string PageContent,PageHead;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Util.GetFileContent("generated_content") == "" || Util.GetFileContent("generated_head") == "")
            GeneratePage();

        PageHead = Util.GetFileContent("generated_head");
        PageContent = Util.GetFileContent("generated_content");

    }
    private void GeneratePage()
    {
        string pContent = "";
        string hContent = "";

        string json = Util.GetFileContent("data");
        var serializer = new JavaScriptSerializer();
        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
        dynamic obj = serializer.Deserialize(json, typeof(object));
        
        string TempStr;

        //Head
        TempStr = Util.GetTemplate("Head");
        TempStr = TempStr.Replace("$sitetitle$", GetValue("SiteConfigurations", "sitetitle", obj));
        TempStr = TempStr.Replace("$siteicon$", GetValue("SiteConfigurations", "siteicon", obj));

        TempStr = TempStr.Replace("$metadescription$", GetValue("SiteConfigurations", "metadescription", obj));
        TempStr = TempStr.Replace("$metakeywords$", GetValue("SiteConfigurations", "metakeywords", obj));
        TempStr = TempStr.Replace("$metaauthor$", GetValue("SiteConfigurations", "metaauthor", obj));
        
        TempStr = TempStr.Replace("$ogurl$", GetValue("OpenGraph","ogurl",obj));
        TempStr = TempStr.Replace("$ogsitename$", GetValue("OpenGraph","ogsitename",obj));
        TempStr = TempStr.Replace("$ogtitle$", GetValue("OpenGraph","ogtitle",obj));
        TempStr = TempStr.Replace("$ogimage$", GetValue("OpenGraph","ogimage",obj));
        TempStr = TempStr.Replace("$ogdescription$",GetValue("OpenGraph","ogdescription",obj));
        TempStr = TempStr.Replace("$fbadmins$", GetValue("OpenGraph","fbadmins",obj));
        hContent += TempStr;

        //NavBar
        TempStr = Util.GetTemplate("Navbar");
        TempStr = TempStr.Replace("$sitename$", GetValue("SiteConfigurations", "sitename", obj));
        pContent += TempStr;

        //Header
        TempStr = Util.GetTemplate("Header");
        TempStr = TempStr.Replace("$sitename$", GetValue("SiteConfigurations", "sitename", obj));
        TempStr = TempStr.Replace("$sitedescription$", GetValue("SiteConfigurations", "sitedescription", obj));
        TempStr = TempStr.Replace("$siteimage$", GetValue("SiteConfigurations", "siteimage", obj));
        pContent += TempStr;

        //Portfolio
        TempStr = Util.GetTemplate("Portfolio");
        string PortfolioItems = "", PortfolioModals = "";
        string[] Values;
        foreach (dynamic obj2 in obj["Portfolio"])
        {
            Values = RenderPortfolioItem(obj2);
            PortfolioItems = Values[0] + PortfolioItems; //we put it the first one since when we add a new item it will be the newest one
            PortfolioModals += Values[1];
        }
        TempStr = TempStr.Replace("$portfolioitems$", PortfolioItems);
        TempStr = TempStr.Replace("$portfoliomodals$", PortfolioModals);
        pContent += TempStr;

        //About
        TempStr = "";
        foreach (dynamic obj1 in obj["About"])
        {
            TempStr += "<div class=\"col-lg-" + obj1["cols"] + "\">";
            TempStr += obj1["content"];
            TempStr += "</div>";
        }
        TempStr = Util.GetTemplate("About").Replace("$rows$",TempStr);
        pContent += TempStr;

        pContent += Util.GetTemplate("Contact");

        //Footer
        TempStr = Util.GetTemplate("Footer");
        TempStr = TempStr.Replace("$col1$", obj["Footer"][0]["col1"]);
        TempStr = TempStr.Replace("$col2$", obj["Footer"][0]["col2"]);
        TempStr = TempStr.Replace("$sitename$", GetValue("SiteConfigurations", "sitename", obj));
        string SocialIcons = "";
        foreach (dynamic obj2 in obj["Social"])
            SocialIcons += RenderSocialIcon(obj2);
        TempStr = TempStr.Replace("$socialicons$", SocialIcons);
        pContent += TempStr;

        Util.SetFileContent("generated_content", pContent);
        Util.SetFileContent("generated_head", hContent);
    }
    
    /// <summary>
    /// Returns a portfolio item -> index 0 : item, index 1: modal
    /// </summary>
    /// <param name="obj">JSON Item</param>
    /// <returns></returns>
    private string[] RenderPortfolioItem(dynamic obj)
    {
        string itemTemplate = Util.GetTemplate("PortfolioItem");
        itemTemplate = itemTemplate.Replace("$id$", obj["id"]);
        itemTemplate = itemTemplate.Replace("$image$", obj["image"]);

        string modalTemplate = Util.GetTemplate("PortfolioItemModal");
        modalTemplate = modalTemplate.Replace("$id$", obj["id"]);
        modalTemplate = modalTemplate.Replace("$image$", obj["image"]);
        modalTemplate = modalTemplate.Replace("$title$", obj["title"]);
        modalTemplate = modalTemplate.Replace("$description$", obj["description"]);
        modalTemplate = modalTemplate.Replace("$client$", obj["client"]);
        modalTemplate = modalTemplate.Replace("$date$", obj["date"]);
        modalTemplate = modalTemplate.Replace("$service$", obj["service"]);
        modalTemplate = modalTemplate.Replace("$site$", obj["site"]);
        modalTemplate = modalTemplate.Replace("$sitelink$", obj["sitelink"]);
        return new string[] { itemTemplate, modalTemplate }; ;
    }
    private string RenderSocialIcon(dynamic obj)
    {
        if (obj["link"] != "")
        {
            string Template = Util.GetTemplate("SocialIcon");
            Template = Template.Replace("$link$", obj["link"]);
            Template = Template.Replace("$class$", obj["class"]);
            return Template;
        }
        else
            return "";
    }
    private string GetValue(string JsonKey, string Key, dynamic obj)
    {
        foreach (dynamic obj1 in obj[JsonKey])
        {
            if (obj1["key"] == Key)
                return obj1["value"];
        }
        return "";
    }
}