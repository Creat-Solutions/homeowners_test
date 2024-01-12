<%@ WebHandler Language="C#" Class="SavePaymentType" %>

using System;
using System.Web;
using System.Linq;
using ME.Admon;

public class SavePaymentType : IHttpHandler {

  public void ProcessRequest (HttpContext context) {
    context.Response.ContentType = "application/json";
    try {
      int quoteid = int.Parse(context.Request["quoteid"]);
      int noOfPayments = int.Parse(context.Request["numberOfPayments"]);
      var validFee = context.Request["policyFee"];
      decimal policyFee = 0;
      bool valid = noOfPayments == 1 || noOfPayments == 2 || noOfPayments == 4;
      if (!valid) {
        throw new Exception();
      }
      HouseInsuranceDataContext db = new HouseInsuranceDataContext();
      var quote = db.quotes.Where(m => m.id == quoteid).Single();
      quote.cantidadDePagos = noOfPayments;

      if (validFee.ToString() == "false")
        policyFee = quote.fee.Value;
      else
        policyFee = decimal.Parse(context.Request["policyFee"]);

      quote.fee = quote.currency == ApplicationConstants.DOLLARS_CURRENCY ? policyFee : (policyFee/10);
      db.SubmitChanges();
      HouseEstate he = quote.insuredPersonHouse1.HouseEstate;
      db.spUpd_quote(quoteid, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
      db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, quote);
      var cotiza = db.spList_Cotiza(quoteid).Single();
      rxpfSur surcharge = ApplicationConstants.GetRXPFSurcharge(db, cotiza.company.Value, cotiza.currency);
      decimal percentage;
      switch (quote.cantidadDePagos)
      {
        case 1:
        default:
          percentage = 0;
          break;
        case 2:
          percentage = surcharge.biyearly;
          break;
        case 4:
          percentage = surcharge.quarterly;
          break;
      }

      decimal rxpfValue = (cotiza.NetPremium ?? 0M) * percentage;
      decimal taxPercentage = db.fGet_QuoteTaxPercentage(quoteid).Value;
      string premium = ",\"Premium\" : \"" + (cotiza.NetPremium ?? 0M ).ToString("C") + "\"";
      string fee = ",\"Fee\" : \"" + ApplicationConstants.Fee(quote.currency, quote.fee.Value).ToString("C") + "\""; ;
      string rxpfJson = ",\"RXPF\" : \"" + rxpfValue.ToString("C") + "\""; ;
      string tax = ",\"Tax\": \"" + ApplicationConstants.CalculateTax((cotiza.NetPremium ?? 0M), taxPercentage, percentage, quote.currency, quote.fee.Value).ToString("C") + "\""; ;
      string total = ",\"Total\": \"" + ApplicationConstants.CalculateTotal((cotiza.NetPremium ?? 0M), taxPercentage, percentage, quote.currency, quote.fee.Value).ToString("C") + "\""; ;
      context.Response.Write("{ \"Result\": true " + premium + fee + rxpfJson + tax + total + "}");
    } catch {
      context.Response.Write("{ \"Result\": false }");
    }
  }

  public bool IsReusable {
    get {
      return false;
    }
  }

}