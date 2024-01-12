using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Protec.Authentication.Membership;
using ME.Admon;

public partial class UserControls_CalculaStep : System.Web.UI.UserControl, IApplicationStep
{
    protected int QuoteID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        int quoteid = (int)Session["quoteid"];
        QuoteID = quoteid;
    }

    public string Title()
    {
        return "Cotiza";
    }

    private bool ShowDiscount()
    {
        //si el usuario es administrador o pertenece al agente M&E
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        return user.UserType == SecurityManager.ADMINISTRATOR ||
               user.Agent == ApplicationConstants.MEAGENT;
    }

    private bool ShowSurcharge()
    {
        return (((UserInformation)HttpContext.Current.User.Identity).Agent == ApplicationConstants.MEAGENT || ((UserInformation)HttpContext.Current.User.Identity).UserID == ApplicationConstants.PTI_USER);
    }

    public bool SaveStep()
    {
        return true;
    }

    protected void valDiscountSurcharge_Validate(object sender, ServerValidateEventArgs e)
    {
        decimal descuento = 0;
        decimal recargo = 0;
        try {
            if (ShowDiscount()) {
                descuento = decimal.Parse(txtDescuento.Text);
            } else {
                descuento = GetDescuento();
            }
            if (ShowSurcharge()) {
                recargo = decimal.Parse(txtRecargo.Text);
            } else {
                recargo = GetRecargo();
            }
        } catch {
            
        }
        bool valid = (descuento == 0 && recargo == 0) ||
                     (descuento != 0 && recargo == 0) ||
                     (descuento == 0 && recargo != 0);
        e.IsValid = valid;
    }

    protected decimal GetDescuento()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int quoteid = (int)Session["quoteid"];
        return db.quotes.Where(m => m.id == quoteid).Select(m => m.descuento).Single();
    }

    protected decimal GetRecargo()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int quoteid = (int)Session["quoteid"];
        return db.quotes.Where(m => m.id == quoteid).Select(m => m.recargo).Single();
    }

    protected void btnRecalculate_Click(object sender, EventArgs e)
    {
        if (!(ShowDiscount() || ShowSurcharge())) {
            return;
        }
        if (!Page.IsValid) {
            IQuoteData calcula = Page.LoadControl("~/UserControls/Calcula.ascx") as IQuoteData;
            calcula.quoteid = (int)Session["quoteid"];
            plhCalcula.Controls.Clear();
            plhCalcula.Controls.Add(calcula as Control);
            return;
        }
        Control load;
        plhCalcula.Controls.Clear();
        try {
            int quoteID = (int)Session["quoteid"];
            decimal? descuento = null, recargo = null;
            if (ShowDiscount() && !string.IsNullOrEmpty(txtDescuento.Text.Trim())) {
                descuento = decimal.Parse(txtDescuento.Text) / 100;
            }
            if (ShowSurcharge() && !string.IsNullOrEmpty(txtRecargo.Text.Trim())) {
                recargo = decimal.Parse(txtRecargo.Text) / 100;
            }
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            var house = db.quotes.Single(m => m.id == quoteID).insuredPersonHouse1;
            HouseEstate he = house.HouseEstate;
            db.spUpd_V1_quoteDescuentoRecargo(quoteID, descuento, recargo, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
            IQuoteData calcula = Page.LoadControl("~/UserControls/Calcula.ascx") as IQuoteData;
            calcula.quoteid = (int)Session["quoteid"];
            load = (Control)calcula;
        } catch {
            Label lbl = new Label();
            lbl.Text = "There was an error while trying to load this screen.";
            load = lbl;
        }
        plhCalcula.Controls.Add(load);
    }

    protected override void OnPreRender(EventArgs e)
    {
        bool showDescuento = ShowDiscount();
        bool showRecargo = ShowSurcharge();
        divDescuento.Visible = divDiscount.Visible = lblDescuento.Visible = lblDiscount.Visible = txtDescuento.Visible = showDescuento;
        valDescuento.Visible = valDescuento.Enabled = showDescuento;
        rangeDescuento.Enabled = rangeDescuento.Visible = showDescuento;
        rangeDescuento2.Enabled = rangeDescuento2.Visible = showDescuento;

        divSurcharge.Visible = divRecargo.Visible = lblRecargo.Visible = lblSurcharge.Visible = showRecargo;
        txtRecargo.Visible = txtRecargo.Enabled = showRecargo;
        cmpRecargo.Enabled = cmpRecargo.Visible = showRecargo;
        cmpRecargo2.Enabled = cmpRecargo2.Visible = showRecargo;
        valRecargo.Enabled = valRecargo.Visible = showRecargo;

        btnRecalculate.Enabled = btnRecalculate.Visible = showDescuento || showRecargo;
    }

    public void LoadStep()
    {
        Control load;
        try {
            IQuoteData calcula = Page.LoadControl("~/UserControls/Calcula.ascx") as IQuoteData;
            calcula.quoteid = (int)Session["quoteid"];
            load = (Control)calcula;
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            var info = db.quotes.Select(m => new { 
                    m.descuento, 
                    m.recargo, 
                    m.id 
                }).Single(m => m.id == calcula.quoteid);
            if (ShowDiscount()) {
                txtDescuento.Text = (info.descuento * 100).ToString("F2");
            }
            if (ShowSurcharge()) {
                txtRecargo.Text = (info.recargo * 100).ToString("F2");
            }

        } catch {
            Label lbl = new Label();
            lbl.Text = "There was an error while trying to load this screen.";
            load = lbl;
        }
        plhCalcula.Controls.Add(load);
    }

}
