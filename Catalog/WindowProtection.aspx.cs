﻿using System;
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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

public partial class Catalog_WindowProtection : System.Web.UI.Page
{
    private bool insertDisplay = false;
    protected void RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer) {
            int m = e.Row.Cells.Count;
            for (int i = m - 1; i >= 1; i--) {
                e.Row.Cells.RemoveAt(i);
            }
            e.Row.Cells[0].ColumnSpan = m;
        }
    }
    protected void lnkInsert_Click(object sender, EventArgs e)
    {
        if (grdWindowProtection.EditIndex != -1) {
            grdWindowProtection.EditIndex = -1;
            grdWindowProtection.DataBind();
        }
        insertDisplay = true;
    }

    protected void frmWindowProtection_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        grdWindowProtection.DataBind();
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
        grdWindowProtection.Visible = !insertDisplay;
        lnkInsert.Visible = !insertDisplay;
        lnkDelete.Visible = !insertDisplay;
        frmWindowProtection.Visible = insertDisplay;
    }

    private void ErrorMessage(string message)
    {
        lblError.Visible = true;
        lblError.Text = message;
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        List<string> keys = new List<string>();
        for (int i = 0; i < grdWindowProtection.Rows.Count; i++) {
            GridViewRow row = grdWindowProtection.Rows[i];
            CheckBox deleteBox = row.FindControl("deleteBox") as CheckBox;
            if (deleteBox != null && deleteBox.Checked) {
                keys.Add(grdWindowProtection.DataKeys[i].Value.ToString());
            }
        }
        if (keys.Count > 0) {
            for (int i = 0; i < keys.Count; i++) {
                try {
                    db.spDel_V1_windowProtection(int.Parse(keys[i]));
                } catch {
                    ErrorMessage("An error ocurred. Maybe a quote/policy is already using data selected for deletion.");
                }
            }
            windowProtection.InvalidateCache();
            grdWindowProtection.DataBind();
        }
    }

    protected void grdWindowProtection_DataBound(object sender, EventArgs e)
    {
        GridView grid = sender as GridView;
        if (grid.HeaderRow == null)
            return;
        CheckBox checkAll = grid.HeaderRow.FindControl("checkAllBox") as CheckBox;
        if (checkAll != null) {
            ClientScript.RegisterArrayDeclaration("deleteBoxes", string.Concat("'", checkAll.ClientID, "'"));
            checkAll.Attributes["onclick"] = "ChangeDeleteBoxesState(this.checked);";
            foreach (GridViewRow row in grid.Rows) {
                CheckBox delBox = row.FindControl("deleteBox") as CheckBox;
                if (delBox != null) {
                    delBox.Attributes["onclick"] = "CheckHeaderForChanges();";
                    ClientScript.RegisterArrayDeclaration("deleteBoxes", string.Concat("'", delBox.ClientID, "'"));
                }
            }
        }
    }
}