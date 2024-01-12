<%@ WebHandler Language="C#" Class="SaveClientInfo" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Web.SessionState;
using Protec.Authentication.Membership;

public class SaveClientInfo : IHttpHandler, IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        StreamReader rawData = new StreamReader(context.Request.InputStream);
        var clientInfo = serializer.Deserialize<ClientInformation>(rawData.ReadToEnd());
        SaveClient(clientInfo);
        context.Response.Write("true");
    }

    private void SaveClient(ClientInformation clientInfo)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int key = (int)HttpContext.Current.Session["quoteid"];
        int userid = ((UserInformation)HttpContext.Current.User.Identity).UserID;
        string street = null;
        string email = null;
        string fax = null;
        string phone = null;
        string city = null;
        string zip = null;
        string rfc = null;
        bool legalPerson = clientInfo.PersonType == "1";
        string lastname = legalPerson ? null : clientInfo.LastName.Trim();
        string additionalInsured = null;
        int? state = null;
        string stateText = null;
        if (clientInfo.LiveOutsideMexico)
        {
            street = clientInfo.Address.Trim();
            zip = clientInfo.ZipCode.Trim();
            state = clientInfo.State;
            stateText = clientInfo.StateText;
            city = clientInfo.City;
        }
        bool inMexico = street == null;

        rfc = clientInfo.RFC.Trim();
        email = clientInfo.Email.Trim();
        additionalInsured = clientInfo.AdditionalInsured.Trim();
        phone = clientInfo.Phone;

        db.spUpd_V2_quotePerson(
            key,
            userid,
            clientInfo.Name,
            lastname,
            string.Empty,
            street,
            email,
            fax,
            phone,
            city,
            state,
            zip,
            legalPerson,
            ApplicationConstants.COMPANY,
            additionalInsured,
            rfc,
            stateText
        );

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}