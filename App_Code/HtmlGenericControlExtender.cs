using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.IO;

public static class HtmlGenericControlExtender
{
    public static string RenderHtmlString(this HtmlGenericControl htmlControl)
    {
        string result = string.Empty;
        using (StringWriter sw = new StringWriter()) {
            var writer = new System.Web.UI.HtmlTextWriter(sw);
            htmlControl.RenderControl(writer);
            result = sw.ToString();
            writer.Close();
        }
        return result;
    }
}
