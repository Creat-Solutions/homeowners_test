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

public partial class Catalog_State : System.Web.UI.Page
{
    private bool insertDisplay = false;
    protected void RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            int m = e.Row.Cells.Count;
            for (int i = m - 1; i >= 1; i--)
            {
                e.Row.Cells.RemoveAt(i);
            }
            e.Row.Cells[0].ColumnSpan = m;
        }
    }

    protected void lnkInsert_Click(object sender, EventArgs e)
    {
        if (grdState.EditIndex != -1)
        {
            grdState.EditIndex = -1;
            grdState.DataBind();
        }

        insertDisplay = true;
    }

    protected void frmState_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        grdState.DataBind();
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
        grdState.Visible = !insertDisplay;
        lnkInsert.Visible = !insertDisplay;
        //lnkDelete.Visible = !insertDisplay;
        lblSCountry.Visible = !insertDisplay;
        ddlSCountry.Visible = !insertDisplay;
        frmState.Visible = insertDisplay;
        //btnSync.Visible = !insertDisplay;
    }

    //protected void lnkDelete_Click(object sender, EventArgs e)
    //{
    //    HouseInsuranceDataContext db = new HouseInsuranceDataContext();
    //    List<string> keys = new List<string>();
    //    for (int i = 0; i < grdState.Rows.Count; i++) {
    //        GridViewRow row = grdState.Rows[i];
    //        CheckBox deleteBox = row.FindControl("deleteBox") as CheckBox;
    //        if (deleteBox != null && deleteBox.Checked) {
    //            keys.Add(grdState.DataKeys[i].Value.ToString());
    //        }
    //    }
    //    if (keys.Count > 0) {
    //        for (int i = 0; i < keys.Count; i++) {
    //            try {
    //                // TODO: DELETE
    //            } catch {
    //                lblError.Text = "An error occurred while deleting the selected option.";
    //                break;
    //            }
    //        }
    //        grdState.DataBind();
    //    }

    //}

    protected void grdState_DataBound(object sender, EventArgs e)
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
        int country = int.Parse((frmState.Row.FindControl("ddlCountry") as DropDownList).SelectedValue);
        e.Values["Country"] = new Country(country);
    }

    /*
    protected void btnSynchronize_Click(object sender, EventArgs e)
    {
        MapfreWebServices webservices = new MapfreWebServices();
        string estadosXml = "<xml><data><valor cod_ramo='" + ApplicationConstants.COD_RAMO + "'/></data> </xml>";
        try {
            XmlNode resultXml = webservices.WS_TW_Estados(estadosXml);
            XElement root = XElement.Load(new XmlNodeReader(resultXml));
            string resultState = root.Descendants(XName.Get("param")).Descendants().
                    Where(m => m.Name.LocalName == "result").Select(m => m.Value).Single();
            if (resultState.Equals("true", StringComparison.CurrentCultureIgnoreCase)) {
                var list = root.Descendants(XName.Get("lista")).
                    Select(m => ((string)m.Element("COD_ESTADO")).Trim() + "," + ((string)m.Element("NOM_ESTADO")).Trim());
                var updateData = string.Join(";", list.ToArray());
                HouseInsuranceDataContext db = new HouseInsuranceDataContext();
                db.spUpd_SynchronizeMapfreStateCodes(updateData);
            }
        } catch (Exception excep) {
            lblError.Text = "There was an error while processing data from Mapfre: " + excep.Message; 
        }
    }
    */
}
