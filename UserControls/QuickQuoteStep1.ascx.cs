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
using System.Text.RegularExpressions;
using Protec.Authentication.Membership;


public partial class UserControls_QuickQuoteStep1 : System.Web.UI.UserControl, IPolicyLoadable
{
    private string _title = "About the Insured";
    private bool m_loadFromPolicy = false;
    private bool loaded = false;
    public bool LoadFromPolicy
    {
        get
        {
            return m_loadFromPolicy;
        }
        set
        {
            m_loadFromPolicy = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        //txtAreaPhone.Attributes.Add("onkeyup", string.Format("return keyLimit('{0}', '{1}', 3);", txtAreaPhone.ClientID, txtPhone1.ClientID));
        //txtPhone1.Attributes.Add("onkeyup", string.Format("return keyLimit('{0}', '{1}', 3);", txtPhone1.ClientID, txtPhone2.ClientID));

        //txtAreaFax.Attributes.Add("onkeyup", string.Format("return keyLimit('{0}', '{1}', 3);", txtAreaFax.ClientID, txtFax1.ClientID));
        //txtFax1.Attributes.Add("onkeyup", string.Format("return keyLimit('{0}', '{1}', 3);", txtFax1.ClientID, txtFax2.ClientID));

        chkFire.Enabled = false;

        ddlFloors.Items.AddRange(Enumerable.Range(1, 5).Select(m => new ListItem { Text = m.ToString(), Value = m.ToString() }).ToArray());

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        ddlClientType.DataSource = from cType in db.clientTypes
                                   join cTypeSur in db.clientTypeSurs on cType.type equals cTypeSur.clientType
                                   where cTypeSur.company == ApplicationConstants.COMPANY
                                   select new {
                                       type = cType.type,
                                       description = cType.description + "/" + cType.descriptionEs
                                   };
        ddlClientType.DataBind();

        ddlHouseType.DataSource = from pType in db.typeOfProperties
                                  join pTypeSur in db.typeOfPropertySurs on pType.type equals pTypeSur.typeOfProperty
                                  where pTypeSur.company == ApplicationConstants.COMPANY
                                  orderby pType.order
                                  select new {
                                      type = pType.type,
                                      description = pType.description + "/" + pType.descriptionEs
                                  };
        ddlHouseType.DataBind();

        ddlUse.DataSource = from pUse in db.useOfProperties
                            join pUseSur in db.useOfPropertySurs on pUse.use equals pUseSur.useOfProperty
                            where pUseSur.company == ApplicationConstants.COMPANY
                            orderby pUse.order
                            select new {
                                use = pUse.use,
                                description = pUse.description + "/" + pUse.descriptionEs
                            };
        ddlUse.DataBind();

        ddlRoof.DataSource = from rType in db.roofTypes
                             join rTypeSur in db.roofTypeSurs on rType.type equals rTypeSur.roofType
                             where rTypeSur.company == ApplicationConstants.COMPANY
                             select new {
                                 type = rType.type,
                                 description = rType.description + "/" + rType.descriptionEs
                             };
        ddlRoof.DataBind();

        ddlWall.DataSource = from wType in db.wallTypes
                             join wTypeSur in db.wallTypeSurs on wType.type equals wTypeSur.wallType
                             where wTypeSur.company == ApplicationConstants.COMPANY
                             select new {
                                 type = wType.type,
                                 description = wType.description + "/" + wType.descriptionEs
                             };
        ddlWall.DataBind();

        LoadStep();
    }

    private int GetKey()
    {
        if (LoadFromPolicy) {
            return (int)Session["policyid"];
        } else {
            if (Session["quoteid"] == null) {
                return 0;
            }
            return (int)Session["quoteid"];
        }
    }

    public void LoadStep()
    {
        if (loaded) {
            return;
        }
        int key;
        try {
            key = GetKey();
        } catch {
            return;
        }
        LoadData(key);
        loaded = true;
    }

    public bool SaveStep()
    {
        if (Page.IsValid) {
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            quote q = null;
            UserInformation userinfo = (UserInformation)Page.User.Identity;
            bool centralAlarm = ddlAlarm.Enabled && ddlAlarm.SelectedValue == "0";
            bool localAlarm = ddlAlarm.Enabled && ddlAlarm.SelectedValue == "1";
            if (Session["quoteid"] == null) {
                q = new quote();
                q.issuedBy = userinfo.UserID;
                q.insuredPerson1 = new insuredPerson {
                    firstname = string.Empty,
                    legalPerson = false
                };
                db.quotes.InsertOnSubmit(q);

            } else {
                q = db.quotes.SingleOrDefault(m => m.id == (int)Session["quoteid"]);
                if (q == null) {
                    return false;
                }
            }

            q.issueDate = DateTime.Now;
            q.typeOfClient = int.Parse(ddlClientType.SelectedValue);
            q.typeOfProperty = int.Parse(ddlHouseType.SelectedValue);
            q.useOfProperty = int.Parse(ddlUse.SelectedValue);
            q.wallType = int.Parse(ddlWall.SelectedValue);
            q.roofType = int.Parse(ddlRoof.SelectedValue);
            q.centralAlarm = centralAlarm;
            q.localAlarm = localAlarm;
            q.HMP = chkHMP.Checked;
            q.quake = chkQuake.Checked;
            q.fireAll = chkFire.Checked;
            q.adjoiningEstates = rbnAdjYes.Checked;
            q.distanceFromSea = int.Parse(rblDistanceSea.SelectedValue);
            q.stories = int.Parse(ddlFloors.SelectedValue);
            q.underground = 0;
            q.tvCircuit = q.tvGuard = q.guards24 = false;
            q.company = ApplicationConstants.COMPANY;
            q.shutters = rbWithShutter.Checked;

            db.SubmitChanges();

            Session["quoteid"] = q.id;

            return true;
        }
        return false;
    }
    public string Title()
    {
        return _title;
    }

    protected void rblDistanceSea_DataBound(object sender, EventArgs e)
    {
        if (GetKey() <= 0) {
            rblDistanceSea.SelectedIndex = rblDistanceSea.Items.Count - 1;
        }
    }

    private void LoadData(int key)
    {
        if (key <= 0) {
            rbnAdjYes.Checked = true;
            rbNoShutter.Checked = rbNoAlarm.Checked = true;
            ddlAlarm.Enabled = false;
            return;
        }

        LoadDataFromQuote(key);
    }

    private void LoadDataFromQuote(int key)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var q = db.quotes.SingleOrDefault(m => m.id == key && m.company == ApplicationConstants.COMPANY);
        if (q == null) {
            rbNoShutter.Checked = rbNoAlarm.Checked = true;
            return;
        }

        bool isAdministrator = ((UserInformation)HttpContext.Current.User.Identity).UserType == SecurityManager.ADMINISTRATOR;
        bool visibleHMP = db.spIsValidHMPDate() == 1 || isAdministrator;
        chkHMP.Visible = chkHMP.Enabled = visibleHMP;

        chkHMP.Checked = q.HMP ?? false;
        chkFire.Checked = q.fireAll ?? true;
        chkQuake.Checked = q.quake ?? false;

        ddlClientType.SelectedValue = q.typeOfClient.ToString();
        ddlUse.SelectedValue = q.useOfProperty.ToString();
        ddlHouseType.SelectedValue = q.typeOfProperty.ToString();
        ddlRoof.SelectedValue = q.roofType.ToString();
        ddlWall.SelectedValue = q.wallType.ToString();
        ddlFloors.SelectedValue = q.stories.ToString();

        rbWithAlarm.Checked = (q.localAlarm ?? false) || (q.centralAlarm ?? false);
        ddlAlarm.Enabled = rbWithAlarm.Checked;
        rbNoAlarm.Checked = !rbWithAlarm.Checked;
        if (q.localAlarm ?? false) {
            ddlAlarm.SelectedValue = "1";
        } else if (q.centralAlarm ?? false) {
            ddlAlarm.SelectedValue = "0";
        }

        if (q.shutters ?? false) {
            rbWithShutter.Checked = true;
            rbNoShutter.Checked = false;
        } else {
            rbNoShutter.Checked = true;
            rbWithShutter.Checked = false;
        }

        if (q.adjoiningEstates ?? true) {
            rbnAdjYes.Checked = true;
            rbnAdjNo.Checked = false;
        } else {
            rbnAdjNo.Checked = true;
            rbnAdjYes.Checked = false;
        }

        if (q.distanceFromSea != null) {
            rblDistanceSea.SelectedValue = q.distanceFromSea.ToString();
        }
    }

    private void LoadDataFromPolicy(int key)
    {

    }

    protected void rbAlarm_CheckedChanged(object sender, EventArgs e)
    {
        ddlAlarm.Enabled = sender == rbWithAlarm;
    }

}
