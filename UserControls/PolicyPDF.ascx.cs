using clArticle140;
using ME.Admon;
using Protec.Authentication.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_PolicyPDF : System.Web.UI.UserControl
{
    private const int USA_ID = 1;
    private const int MEXICO_ID = 2;
    private bool SkipPayment()
    {
        return Session["skipPayment"] != null && (bool)Session["skipPayment"];
    }

    private string GetPolicyNo()
    {
        return Session["policyno"] != null && !string.IsNullOrEmpty((string)Session["policyno"])
            ? (string)Session["policyno"]
            : "-INVALID NUMBER (PLEASE CONTACT US)-";
    }

    private void GetFEPolicy(int id)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        Session["fepadre"] = db.policies.Single(m => m.id == id).cantidadDePagos > 1;
    }

    private void HandleNullSession()
    {
        // For some strange reasons session may become invalid between the buy click and this step
        // (possibly due to the server killing the service thread). We assume that the last policy
        // made by this user is the correct, but we send an e-mail warning about this behavior.

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        UserInformation user = (UserInformation)Page.User.Identity;

        policy p = db.policies
                        .Where(m => m.issuedBy == user.UserID)
                        .OrderByDescending(m => m.id)
                        .Take(1)
                        .SingleOrDefault();

        // this condition is very likely to be true
        if (p != null)
        {
            // set the policyid and policyno
            Session["policyid"] = p.id;
            Session["policyno"] = p.policyNo;
        }

        // now send the email
        string[] tos = ApplicationConstants.GetDebugEmails();
        if (tos.Length > 0)
        {
            //EmailService service = new EmailService(tos[0], false);
            //for (int i = 1; i < tos.Length; i++) {
            //    service.AddCC(tos[i]);
            //}
            //PolicyCacheData data = Session["PolicyCheckerCache"] as PolicyCacheData;
            //string policyno = p == null ? "--no se encontro poliza--" : p.policyNo;
            //string quoteno = Session["quoteid"] == null ? "NULL" : Session["quoteid"].ToString();

            //service.Subject = "HO: Sesion invalida despues de comprar la poliza";
            //service.Body = "La sesion (probablemente) expiro para el usuario: " + user.Name + " con ID: " + user.UserID +
            //               " al comprar una poliza. La poliza mas probable que haya comprado es " + policyno + ".\n" +
            //               "Session[quoteid] (este dato no debe existir si expiro la sesion, de manera contraria es" +
            //               "muy probable que sea otro error) = " + quoteno + "\n" + "policy cache data: " +
            //               data == null ? "NULL" : (data.Data == null ? "Data null" : data.Data.PolicyNo);
            //service.Send();
        }
    }

    private void LoadArticulo140(int policyID)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        policy p = db.policies.Single(m => m.id == policyID);
        csArticle140 art140 = new csArticle140();
        PartialArt140 partial140 = (PartialArt140)Session["partial140"];
        art140.policy.policyno = p.policyNo;
        art140.policy.product = ApplicationConstants.APPLICATION;
        art140.policy.idOrigin = 1;
        art140.policy.company = (short)p.company;
        art140.policy.currency = (byte)p.currency;
        art140.policy.expiration = p.endDate;
        art140.policy.inception = p.startDate;
        art140.policy.insuredCountry = MEXICO_ID.ToString();
        art140.policy.issueDate = p.issueDate;
        art140.policy.policyDescription = "Hogar Mapfre " + p.policyNo + " " + p.days + " " + p.insuredPerson1.firstname + " " + p.insuredPerson1.lastName;
        art140.policy.totalPremium = ApplicationConstants.CalculateTotal(p.netPremium, p.taxP, p.porcentajeRecargoPagoFraccionado, p.currency, p.fee);
        art140.insured.DOB = p.insuredPerson1.DOB.Value;
        art140.insured.taxID = p.insuredPerson1.rfc == null ? "" : p.insuredPerson1.rfc;
        art140.insured.nationality = p.insuredPerson1.citizenship == null ? "" : p.insuredPerson1.citizenship;
        art140.insured.typeOperation = p.insuredPerson1.occupation == null ? "" : p.insuredPerson1.occupation;
        art140.insured.email = p.insuredPerson1.email;
        art140.insured.isCompany = p.insuredPerson1.legalPerson;
        art140.insured.name = p.insuredPerson1.firstname;
        art140.insured.lastName = p.insuredPerson1.lastName == null ? "" : p.insuredPerson1.lastName;
        art140.insured.phone = p.insuredPerson1.phone;
        art140.insured.fielNo = partial140.FIEL == null ? "" : partial140.FIEL;
        art140.insured.identificationNo = partial140.IDNumber == null ? "" : partial140.IDNumber;
        art140.insured.identificationType = partial140.IDType == null ? "" : partial140.IDType;
        art140.insured.incorporationNo = partial140.IncorporationNumber == null ? "" : partial140.IncorporationNumber;
        art140.insured.legalRepresentative = partial140.LegalRepresentative == null ? "" : partial140.LegalRepresentative;
        art140.insured.mercantileNo = partial140.MercantileNumber == null ? "" : partial140.MercantileNumber;

        if (p.insuredPerson1.street != null)
        {
            try
            {
                string[] address = p.insuredPerson1.street.Split(',');
                art140.address.idCountry = partial140.CountryID;
                art140.address.state = p.insuredPerson1.stateText;
                art140.address.city = p.insuredPerson1.city;
                art140.address.street = address[0].Trim();
                art140.address.extNo = address[1].Trim();
                art140.address.intNo = address.Length == 2 ? "" : address[2].Trim();
                art140.address.zipCode = p.insuredPerson1.zipCode;
                art140.address.colony = string.Empty;
            }
            catch
            {
                throw new Exception("Missing Article 140 Information");
            }
        }
        else
        {
            art140.address.idCountry = MEXICO_ID;
            art140.address.state = p.insuredPersonHouse1.HouseEstate.City.State.Name;
            art140.address.city = p.insuredPersonHouse1.HouseEstate.City.Name;
            art140.address.street = p.insuredPersonHouse1.street;
            art140.address.extNo = p.insuredPersonHouse1.exteriorNo;
            art140.address.intNo = p.insuredPersonHouse1.InteriorNo == null ? "" : p.insuredPersonHouse1.InteriorNo;
            art140.address.zipCode = p.insuredPersonHouse1.HouseEstate.ZipCode;
            art140.address.colony = p.insuredPersonHouse1.HouseEstate.Name;
        }
        try
        {
            art140.saveAll();
        }
        catch
        {
            //fail silently
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["policyid"] == null)
        {
            HandleNullSession();
        }
        lblWarningSkip.Visible = SkipPayment();
        lblPolicyNumber.Text = GetPolicyNo();
        GetFEPolicy((int)Session["policyid"]);
        LoadArticulo140((int)Session["policyid"]);
    }
}
