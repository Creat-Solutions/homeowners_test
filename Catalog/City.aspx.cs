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
using System.Xml;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ME.Admon;

public partial class Catalog_City : System.Web.UI.Page
{
    private bool insertDisplay = false;

    protected void lnkInsert_Click(object sender, EventArgs e)
    {
        if (grdCity.EditIndex != -1)
        {
            grdCity.EditIndex = -1;
            grdCity.DataBind();
        }

        insertDisplay = true;
    }

    protected void frmCity_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        grdCity.DataBind();
    }

    protected void lnkDoInsert_Click(object sender, EventArgs e)
    {
        Validate();
        if (Page.IsValid)
            insertDisplay = false;
        else
            insertDisplay = true;
    }

    protected void Page_PreRender()
    {
        grdCity.Visible = !insertDisplay;
        lnkInsert.Visible = !insertDisplay;
        //lnkDelete.Visible = !insertDisplay;
        frmCity.Visible = insertDisplay;
        btnSearch.Visible = !insertDisplay;
        txtSearch.Visible = !insertDisplay;
        lblSearch.Visible = !insertDisplay;
        lblSName.Visible = !insertDisplay;
        lblSState.Visible = !insertDisplay;
        ddlStateSearch.Visible = !insertDisplay;
        //btnSync.Visible = !insertDisplay;
    }

    protected void search_Click(object sender, EventArgs e)
    {

    }

    //protected void lnkDelete_Click(object sender, EventArgs e)
    //{
    //    HouseInsuranceDataContext db = new HouseInsuranceDataContext();
    //    List<string> keys = new List<string>();
    //    for (int i = 0; i < grdCity.Rows.Count; i++) {
    //        GridViewRow row = grdCity.Rows[i];
    //        CheckBox deleteBox = row.FindControl("deleteBox") as CheckBox;
    //        if (deleteBox != null && deleteBox.Checked) {
    //            keys.Add(grdCity.DataKeys[i].Value.ToString());
    //        }
    //    }
    //    if (keys.Count > 0) {
    //        for (int i = 0; i < keys.Count; i++) {
    //            try {
    //                City city = new City(int.Parse(keys[i]));
    //                city.Delete();
    //            } catch {
    //                lblError.Text = "An error occurred while deleting the selected option.";
    //                break;
    //            }
    //        }

    //        grdCity.DataBind();
    //    }

    //}

    protected void grdCity_DataBound(object sender, EventArgs e)
    {
        GridView grid = sender as GridView;
        if (grid.HeaderRow == null)
            return;
        CheckBox checkAll = grid.HeaderRow.FindControl("checkAllBox") as CheckBox;
        if (checkAll != null)
        {
            ClientScript.RegisterArrayDeclaration("deleteBoxes", string.Concat("'", checkAll.ClientID, "'"));
            checkAll.Attributes["onclick"] = "ChangeDeleteBoxesState(this.checked);";
            foreach (GridViewRow row in grid.Rows)
            {
                CheckBox delBox = row.FindControl("deleteBox") as CheckBox;
                if (delBox != null)
                {
                    delBox.Attributes["onclick"] = "CheckHeaderForChanges();";
                    ClientScript.RegisterArrayDeclaration("deleteBoxes", string.Concat("'", delBox.ClientID, "'"));
                }
            }
        }
    }

    protected void ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        int state = int.Parse((frmCity.Row.FindControl("ddlState") as DropDownList).SelectedValue);
        e.Values["State"] = new State(state);
        e.Values["HMP"] = (frmCity.Row.FindControl("ddlInsertZonaFHM") as DropDownList).SelectedValue;
    }

    /*
    protected void btnSynchronize_Click(object sender, EventArgs e)
    {
        MapfreWebServices webservices = new MapfreWebServices();
        string cityTemplate = "<xml><data><valor cod_ramo='" + ApplicationConstants.COD_RAMO + "' cod_estado='{0}'/></data> </xml>";
        string estadosXml = "<xml><data><valor cod_ramo='" + ApplicationConstants.COD_RAMO + "'/></data> </xml>";
        try {
            XmlNode resultStateXml = webservices.WS_TW_Estados(estadosXml);
            XElement root = XElement.Load(new XmlNodeReader(resultStateXml));
            string resultState = root.Descendants(XName.Get("param")).Descendants().
                    Where(m => m.Name.LocalName == "result").Select(m => m.Value).Single();
            if (resultState.Equals("true", StringComparison.CurrentCultureIgnoreCase)) {
                var list = root.Descendants(XName.Get("lista")).
                    Select(m => (string)m.Element("COD_ESTADO"));
                string listOfCities = string.Empty;
                foreach (string stateCode in list) {
                    string cityXml = string.Format(cityTemplate, stateCode);
                    XmlNode resultCityXml = webservices.WS_TW_Poblaciones(cityXml);
                    XElement rootState = XElement.Load(new XmlNodeReader(resultCityXml));
                    var listData = rootState.Descendants(XName.Get("lista")).
                        Select(m => ((string)m.Element("COD_PROV")).Trim() + "," + ((string)m.Element("NOM_PROV")).Trim());
                    listOfCities += string.Join(";", listData.ToArray()) + ";";
                }
                HouseInsuranceDataContext db = new HouseInsuranceDataContext();
                listOfCities = listOfCities.Substring(0, listOfCities.Length - 1);
                db.spUpd_SynchronizeMapfreCityCodes(listOfCities);
            }
        } catch (Exception excep) {
            lblError.Text = "There was an error while processing data from Mapfre: " + excep.Message; 
        }
    }
    */
}
