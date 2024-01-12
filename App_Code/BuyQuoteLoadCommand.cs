using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Protec.Authentication.Membership;

/// <summary>
/// Comando para cargar cotizaciones que van a ser para compra
/// </summary>
public class BuyQuoteLoadCommand : AbstractLoadCommand
{
    private Label lblError = null;
	public BuyQuoteLoadCommand(HttpContext context, Label lblError) :
        base(context)
	{
        this.lblError = lblError;
	}

    private bool m_pesos = false;
    private decimal m_tipoCambio = 0;

    private bool CheckBetweenLimits(decimal value, decimal minimum, decimal maximum, bool percentage, decimal sum)
    {
        if (value == 0)
            return true;
        decimal min = minimum, max = maximum;

        if (percentage) {
            min = sum * minimum / 100;
            max = sum * maximum / 100;
        } else if (m_pesos) {
            min *= m_tipoCambio;
            max *= m_tipoCambio;
        }

        return value >= min && value <= max;
    }

    private bool CheckBetweenLimits(decimal value, decimal minimum, decimal maximum)
    {
        return CheckBetweenLimits(value, minimum, maximum, false, 0);
    }

    private bool CheckQuoteForPolicy(HouseInsuranceDataContext db, quote data)
    {
        decimal sum = db.quoteDetails.Where(m => m.quote == data.id).Sum(m => m.prima) ?? 0;
        m_pesos = data.currency == 1;
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        m_tipoCambio = user.ExchangeRate;
        var limites = db.valueSublimits.Select(m => new { m.minimum, m.maximum, m.id }).OrderBy(m => m.id).ToArray();
        var addCoverages = db.additionalCoverages.Select(m => new { m.minimum, m.maximum, m.id }).OrderBy(m => m.id).ToArray();
        bool value = sum != 0;

        decimal sumLimits = data.buildingLimit + data.contentsLimit;
        value = value && CheckBetweenLimits(data.buildingLimit, limites[0].minimum, limites[0].maximum);
        value = value && CheckBetweenLimits(data.outdoorLimit, limites[1].minimum, limites[1].maximum);
        value = value && CheckBetweenLimits(data.contentsLimit, limites[2].minimum, limites[2].maximum);
        value = value && CheckBetweenLimits(data.debriLimit, limites[3].minimum, limites[3].maximum, true, sumLimits);
        value = value && CheckBetweenLimits(data.extraLimit, limites[4].minimum, limites[4].maximum, true, sumLimits);
        value = value && CheckBetweenLimits(data.lossOfRentsLimit, limites[5].minimum, limites[5].maximum);

        value = value && CheckBetweenLimits(data.CSLLimit, (decimal)addCoverages[0].minimum, (decimal)addCoverages[0].maximum);
        value = value && CheckBetweenLimits(data.burglaryLimit, (decimal)addCoverages[2].minimum, (decimal)addCoverages[2].maximum, true, data.contentsLimit);
        value = value && CheckBetweenLimits(data.burglaryJewelryLimit, (decimal)addCoverages[3].minimum, (decimal)addCoverages[3].maximum, true, data.contentsLimit);
        value = value && CheckBetweenLimits(data.moneySecurityLimit, (decimal)addCoverages[4].minimum, (decimal)addCoverages[4].maximum);
        value = value && CheckBetweenLimits(data.accidentalGlassBreakageLimit, (decimal)addCoverages[5].minimum, (decimal)addCoverages[5].maximum);
        value = value && CheckBetweenLimits(data.electronicEquipmentLimit, (decimal)addCoverages[6].minimum, (decimal)addCoverages[6].maximum, true, data.contentsLimit);
        value = value && CheckBetweenLimits(data.personalItemsLimit, (decimal)addCoverages[7].minimum, (decimal)addCoverages[7].maximum);

        return value;
    }

    public override void Init()
    {
        string key = Request.QueryString["id"];
        if (string.IsNullOrEmpty(key)) {
            return;
        }

        try {
            Load(key);
        } catch {
            lblError.Text = "There is no quote with such id: " + key;
        }
    }

    public override void Load(string id)
    {
        try {
            int key = int.Parse(id);
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
            quote data = null;
            if (user.Agent == ApplicationConstants.MEAGENT || 
                user.UserType == SecurityManager.ADMINISTRATOR || user.UserID == ApplicationConstants.PTI_USER) {
                data = db.quotes.Where(m => m.id == key).Single();
            } else {
                var usuarios = AdminGateway.Gateway.GetListOfUsersFromAgent(user.Agent, user.UserID);
                data = db.quotes.Where(m => m.id == key && usuarios.Contains(m.issuedBy)).Single();
            }
            if (data.id != key)
                throw new Exception();
            Session["quoteid"] = key;
            Session["StepIndex"] = 3;
            Session["PolicyStep"] = null;

            //check for consistency
            if (!data.limitsEnabled) {
                lblError.Text = "The quote you are trying to buy is pending authorization.";
            } else if (!CheckQuoteForPolicy(db, data)) {
                lblError.Text = "The quote you are trying to buy is not complete or has invalid data.";
            } else {
                //all good, proceed to load the quote
                Session["quoteidBuy"] = key;
                Response.Redirect("~/Application/GenerateQuote.aspx", true);
            }
        } catch {
            lblError.Text = "There is no quote with such id: " + id;
        }
    }

}
