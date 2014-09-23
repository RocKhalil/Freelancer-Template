using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Serialization;
/// <summary>
/// Summary description for Util
/// </summary>
public static class Util
{
    public static string HashStringSHA1(string textToHash)
    {
        System.Security.Cryptography.SHA1CryptoServiceProvider SHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(textToHash);
        byte[] byteHash = SHA1.ComputeHash(byteValue);
        SHA1.Clear();
        return Convert.ToBase64String(byteHash);
    }
    public static DateTime ToDate(object Value)
    {
        try { return Convert.ToDateTime(Value); }
        catch { }
        return Convert.ToDateTime("1/1/2000");
    }
    public static double ToDouble(int Value, double Default)
    {
        try { return Convert.ToDouble(Value); }
        catch { }
        return Default;
    }
    public static double ToDouble(string Value, double Default)
    {
        try { return Convert.ToDouble(Value); }
        catch { }
        return Default;
    }
    public static string ToString(object obj)
    {
        if (obj == null)
            return "";
        return obj.ToString();
    }
    public static bool ToBool(object Value, bool Default)
    {
        string Val = Util.ToString(Value);
        if (Val == "")
            return Default;
        return Util.ToBool(Val, Default);
    }
    public static bool ToBool(string Value, bool Default)
    {
        if (Value != null && Value != null)
            try { return Convert.ToBoolean(Value); }
            catch { }
        return Default;
    }
    public static int ToInt(object Value, int Default)
    {
        string Val = Util.ToString(Value);
        if (Val == "")
            return Default;
        return Util.ToInt(Val, Default);
    }
    public static int ToInt(string Value, int Default)
    {
        if (Value != null && Value != "" && Value.ToLower().IndexOf("empty") == -1)
        {
            try { return Convert.ToInt32(Value); }
            catch { return Default; }
        }
        return Default;
    }
    public static ArrayList StringToArrayList(string str, char separator, bool tolower)
    {
        ArrayList MyList = new ArrayList();
        if (str != null && str != "")
        {
            if (tolower) str = str.ToLower();
            string[] Arr = str.Split(separator);
            for (int i = 0; i < Arr.Length; i++)
            {
                MyList.Add(Arr[i]);
            }
        }
        return MyList;
    }
    public static string InternalEncode(string str)
    {
        str = str.Replace("'", " ‘ ");
        str = str.Replace("\"", "“");
        return str;
    }
    public static string InternalDecode(string str)
    {
        str = str.Replace( " ‘ ","'");
        str = str.Replace("“","\"");
        return str;
    }
    public static int SendMail(string EmailTo, string EmailFrom, string EmailBody, string EmailSubject)
    {
        try
        {
            MailMessage Msg = new MailMessage();
            SmtpClient SC = new SmtpClient();
            Msg.From = new MailAddress(EmailFrom);
            string[] Arr = EmailTo.Split(',');
            foreach (string mailto in Arr)
                Msg.To.Add(new MailAddress(mailto));
            Msg.Subject = EmailSubject;
            Msg.IsBodyHtml = true;
            Msg.Body = EmailBody;
            SC.EnableSsl = true;
            SC.Send(Msg);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return 1;
    }
    public static string GetTemplate(string Filename)
    {
        string TempalteFolder = ConfigurationManager.AppSettings["Localfilespath"] + "\\Templates\\";
        if (System.IO.File.Exists(@TempalteFolder + "\\" + Filename + ".html"))
        {
            try
            {
                string text = System.IO.File.ReadAllText(@TempalteFolder + "\\" + Filename + ".html");
                return text;
            }
            catch { }
        }
        return "";
    }
    public static string GetFileContent(string Filename)
    {
        string TempalteFolder = ConfigurationManager.AppSettings["Localfilespath"] + "\\data\\";
        if (System.IO.File.Exists(@TempalteFolder + "\\" + Filename + ".txt"))
        {
            try
            {
                string text = System.IO.File.ReadAllText(@TempalteFolder + "\\" + Filename + ".txt");
                return text;
            }
            catch { }
        }
        return "";
    }
    public static bool SetFileContent(string Filename, string Content)
    {
        string TempalteFolder = ConfigurationManager.AppSettings["Localfilespath"] + "\\data\\";
        if (System.IO.File.Exists(@TempalteFolder + "\\" + Filename + ".txt"))
        {
            try
            {
                System.IO.File.WriteAllText(@TempalteFolder + "\\" + Filename + ".txt",Content);
                return true;
            }
            catch { }
        }
        return false;
    }
    public static bool UpdateDataFile(Dictionary<string, string> ValuesToUpdate, List<string> ValuesToDelete,List<string> ValuesToAdd)
    {
        int MaxId=0;
        var serializer = new JavaScriptSerializer();
        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
        dynamic obj = serializer.Deserialize(GetFileContent("data"), typeof(object));
        
        string result = "{";
        dynamic objChild;
        
        //SiteConfig
        result += "\"SiteConfigurations\":[";
        objChild = obj["SiteConfigurations"];
        for (int i = 0; i < objChild.Count; i++)
        {
            result += "{";
            result += "\"key\":\"";
            result += ValidateJSonValue(objChild[i]["key"]);
            result += "\",";
            result += "\"display\":\"";
            result += ValidateJSonValue(objChild[i]["display"]);
            result += "\",";
            result += "\"value\":\"";
            if(ValuesToUpdate.ContainsKey("siteconfigurations-" + objChild[i]["key"]))
                result += ValidateJSonValue(ValuesToUpdate["siteconfigurations-" + objChild[i]["key"]]);
            else
                result += ValidateJSonValue(objChild[i]["value"]);
            result += "\"";
            result += "}";
            if (i != objChild.Count - 1)
                result += ",";
        }
        result += "],";

        result += "\"OpenGraph\":[";
        objChild = obj["OpenGraph"];
        for (int i = 0; i < objChild.Count; i++)
        {
            result += "{";
            result += "\"key\":\"";
            result += ValidateJSonValue(objChild[i]["key"]);
            result += "\",";
            result += "\"display\":\"";
            result += ValidateJSonValue(objChild[i]["display"]);
            result += "\",";
            result += "\"value\":\"";
            if (ValuesToUpdate.ContainsKey("opengraph-" + objChild[i]["key"]))
                result += ValidateJSonValue(ValuesToUpdate["opengraph-" + objChild[i]["key"]]);
            else
                result += ValidateJSonValue(objChild[i]["value"]);
            result += "\"";
            result += "}";
            if (i != objChild.Count - 1)
                result += ",";
        }
        result += "],";

        result += "\"Portfolio\":[";
        objChild = obj["Portfolio"];
        MaxId = 0;
        for (int i = 0; i < objChild.Count; i++)
        {
            if (ValuesToDelete.Contains("portfolio-" + objChild[i]["id"]))
            {
                //if its the last value we need to trim the last comma or we'll get an error
                if (i == objChild.Count - 1)
                    result = result.TrimEnd(',');
                continue;
            }
            result += "{";
            result += "\"id\":\"";
            result += ValidateJSonValue(objChild[i]["id"]);
            result += "\",";

            result += "\"image\":\"";
            if (ValuesToUpdate.ContainsKey("portfolio-image-" + objChild[i]["id"]))
                result += ValuesToUpdate["portfolio-image-" + objChild[i]["id"]];
            else
                result += ValidateJSonValue(objChild[i]["image"]);
            result += "\",";

            result += "\"title\":\"";
            if (ValuesToUpdate.ContainsKey("portfolio-title-" + objChild[i]["id"]))
                result += ValuesToUpdate["portfolio-title-" + objChild[i]["id"]];
            else
                result += ValidateJSonValue(objChild[i]["title"]);
            result += "\",";

            result += "\"description\":\"";
            if (ValuesToUpdate.ContainsKey("portfolio-description-" + objChild[i]["id"]))
                result += ValuesToUpdate["portfolio-description-" + objChild[i]["id"]];
            else
                result += ValidateJSonValue(objChild[i]["description"]);
            result += "\",";

            result += "\"sitelink\":\"";
            if (ValuesToUpdate.ContainsKey("portfolio-sitelink-" + objChild[i]["id"]))
                result += ValuesToUpdate["portfolio-sitelink-" + objChild[i]["id"]];
            else
                result += ValidateJSonValue(objChild[i]["sitelink"]);
            result += "\",";

            result += "\"client\":\"";
            if (ValuesToUpdate.ContainsKey("portfolio-client-" + objChild[i]["id"]))
                result += ValuesToUpdate["portfolio-client-" + objChild[i]["id"]];
            else
                result += ValidateJSonValue(objChild[i]["client"]);
            result += "\",";

            result += "\"date\":\"";
            if (ValuesToUpdate.ContainsKey("portfolio-date-" + objChild[i]["id"]))
                result += ValuesToUpdate["portfolio-date-" + objChild[i]["id"]];
            else
                result += ValidateJSonValue(objChild[i]["date"]);
            result += "\",";

            result += "\"service\":\"";
            if (ValuesToUpdate.ContainsKey("portfolio-service-" + objChild[i]["id"]))
                result += ValuesToUpdate["portfolio-service-" + objChild[i]["id"]];
            else
                result += ValidateJSonValue(objChild[i]["service"]);
            result += "\"";
            result += "}";
            if (i != objChild.Count - 1)
                result += ",";
            if (Util.ToInt(objChild[i]["id"], 0) > MaxId)
                MaxId = Util.ToInt(objChild[i]["id"], 0);
        }
        if (ValuesToAdd.Contains("portfolio"))
        {
            result += ",";
            result += "{";
            result += "\"id\":\"";
            result += ValidateJSonValue((MaxId + 1).ToString());
            result += "\",";
            result += "\"image\":\"\",";
            result += "\"title\":\"\",";
            result += "\"description\":\"\",";
            result += "\"sitelink\":\"\",";
            result += "\"client\":\"\",";
            result += "\"date\":\"\",";
            result += "\"service\":\"\"";
            result += "}";
        }
        result += "],";

        result += "\"About\":[";
        objChild = obj["About"];
        MaxId = 0;
        for (int i = 0; i < objChild.Count; i++)
        {
            if (ValuesToDelete.Contains("about-" + objChild[i]["id"]))
            {
                //if its the last value we need to trim the last comma or we'll get an error
                if (i == objChild.Count - 1)
                    result = result.TrimEnd(',');
                continue;
            }
            result += "{";
            result += "\"id\":\"";
            result += ValidateJSonValue(objChild[i]["id"]);
            result += "\",";
            result += "\"cols\":\"";
            if (ValuesToUpdate.ContainsKey("about-cols-" + objChild[i]["id"]))
                result += ValidateJSonValue(ValuesToUpdate["about-cols-" + objChild[i]["id"]]);
            else
                result += ValidateJSonValue(objChild[i]["cols"]);
            result += "\",";
            result += "\"content\":\"";
            if(ValuesToUpdate.ContainsKey("about-" + objChild[i]["id"]))
                result += ValidateJSonValue(ValuesToUpdate["about-" + objChild[i]["id"]]);
            else
                result += ValidateJSonValue(objChild[i]["content"]);
            result += "\"";
            result += "}";
            if (i != objChild.Count - 1)
                result += ",";

            if(Util.ToInt(objChild[i]["id"],0) > MaxId)
                MaxId = Util.ToInt(objChild[i]["id"],0);
        }
        if (ValuesToAdd.Contains("about"))
        {
            result += ",";
            result += "{";
            result += "\"id\":\"";
            result += ValidateJSonValue((MaxId + 1).ToString());
            result += "\",";
            result += "\"cols\":\"\",";
            result += "\"content\":\"\"";
            result += "}";
        }
        result += "],";

        result += "\"Footer\":[";
        objChild = obj["Footer"];
        result += "{";
        result += "\"col1\":\"";
        if (ValuesToUpdate.ContainsKey("footer-col1"))
            result += ValidateJSonValue(ValuesToUpdate["footer-col1"]);
        else
            result += ValidateJSonValue(objChild[0]["col1"]);
        result += "\"";
        result += ",";
        result += "\"col2\":\"";
        if (ValuesToUpdate.ContainsKey("footer-col2"))
            result += ValidateJSonValue(ValuesToUpdate["footer-col2"]);
        else
            result += ValidateJSonValue(objChild[0]["col2"]);
        result += "\"";
        result += "}";
        result += "],";

        result += "\"Social\":[";
        objChild = obj["Social"];
        for (int i = 0; i < objChild.Count; i++)
        {
            result += "{";
            result += "\"key\":\"";
            result += ValidateJSonValue(objChild[i]["key"]);
            result += "\",";
            result += "\"display\":\"";
            result += ValidateJSonValue(objChild[i]["display"]);
            result += "\",";
            result += "\"link\":\"";
            if (ValuesToUpdate.ContainsKey("social-" + objChild[i]["key"]))
                result += ValidateJSonValue(ValuesToUpdate["social-" + objChild[i]["key"]]);
            else
                result += ValidateJSonValue(objChild[i]["link"]);
            result += "\",";
            result += "\"class\":\"";
            result += ValidateJSonValue(objChild[i]["class"]);
            result += "\"";
            result += "}";
            if (i != objChild.Count - 1)
                result += ",";
        }
        result += "]";

        result += "}";
        return SetFileContent("data", result);
    }
    public static string ValidateJSonValue(string Value)
    {
        Value = Value.Replace("\"","\\\"");
        return Value;
    }
}