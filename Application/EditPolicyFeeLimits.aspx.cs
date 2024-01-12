using ME.Admon;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Telepro.Conexion;

public partial class Application_EditPolicyFeeLimits : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void Page_PreRender(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlSuperProducer.DataSource = csSuperProducer.List(ApplicationConstants.ADMINDB);
            ddlSuperProducer.DataBind();

            if (Request["brID"] != null)
            {
                var branch = new csBranch(int.Parse(Request["brID"]), ApplicationConstants.ADMINDB);
                var ag = new csAgent(branch.agent, ApplicationConstants.ADMINDB);
                var producer = new csProducer(ag.producer, ApplicationConstants.ADMINDB);
                try
                {
                    ddlSuperProducer.Items.FindByValue(producer.superProducer.ToString()).Selected = true;
                    RefreshProducer(int.Parse(ddlSuperProducer.SelectedItem.Value));

                    ddlProducer.Items.FindByValue(producer.producer.ToString()).Selected = true;
                    RefreshAgent(int.Parse(ddlProducer.SelectedItem.Value));

                    ddlAgent.Items.FindByValue(branch.agent.ToString()).Selected = true;
                    RefreshBranch(int.Parse(ddlAgent.SelectedItem.Value));

                    RefreshGrid();
                }
                catch { }
            }
            else
            {
                RefreshProducer(int.Parse(ddlSuperProducer.SelectedItem.Value));
                RefreshAgent(int.Parse(ddlProducer.SelectedItem.Value));
                RefreshBranch(int.Parse(ddlAgent.SelectedItem.Value));
                RefreshGrid();
            }
        }
    }

    private void RefreshProducer(int superProducer)
    {
        ddlProducer.Items.Clear();
        ddlProducer.DataSource = csProducer.List(ApplicationConstants.ADMINDB, superProducer);
        ddlProducer.DataBind();
    }

    private void RefreshAgent(int producer)
    {
        ddlAgent.Items.Clear();
        ddlAgent.DataSource = csAgent.List(ApplicationConstants.ADMINDB, producer);
        ddlAgent.DataBind();
    }

    private void RefreshBranch(int agent)
    {
        ddlBranch.Items.Clear();
        ddlBranch.DataSource = csBranch.List(agent, ApplicationConstants.ADMINDB);
        ddlBranch.DataBind();
    }

    private void RefreshGrid()
    {
        lblError.Text = "";
        if (ddlBranch.Items.Count > 0)
        {
            string sqlBranch = "SELECT * FROM [admonPoliza].[dbo].[branchDetail] where product = " + ApplicationConstants.APPLICATION + " and company = " + ApplicationConstants.COMPANY + " and branch = " + ddlBranch.SelectedItem.Value;
            var con = new dbConexion(ApplicationConstants.DB);
            DataTable dtBranchDetail = con.ExecuteDS(sqlBranch).Tables[0];

            if (dtBranchDetail.Rows.Count == 0)
            {
                lblError.Text = "Branchdetail doesn't exists for this branch, try another one.";
                tableLimits.Visible = false;
                changeFee.Visible = false;
            }
            else
            {
                var policyFee = dtBranchDetail.Rows[0]["changePolicyFee"];
                chkFee.Checked = (bool) policyFee;
                changeFee.Visible = true;

                if (chkFee.Checked)
                {
                    string sql = "select pfl.* from policyFeeLimits pfl inner join [admonPoliza].[dbo].[branchDetail] bd on pfl.branchDetail = bd.branchDetail where bd.company = " + ApplicationConstants.COMPANY + " and bd.product = " + ApplicationConstants.APPLICATION + " and bd.branch = " + ddlBranch.SelectedItem.Value;
                    DataTable dt = con.ExecuteDS(sql).Tables[0];
                    grid.Ascendente = "yes";    // Si se debe de ordenar en forma Ascendente o no Default:Yes
                    grid.SortExpression = "policyFeeLimit";    // campo por el cual se debe ordenar el DataSet				
                    grid.GridData = dt.DataSet;
                    grid.CurrentPageIndex = 0;
                    grid.ActualizarDataGrid();
                    tableLimits.Visible = true;
                }
                else
                {
                    if (grid.GridData != null)
                    {
                        grid.GridData.Clear();
                        grid.ActualizarDataGrid();
                    }
                    tableLimits.Visible = false;
                }

            }
        }
        else
        {
            if (grid.GridData != null)
            {
                grid.GridData.Clear();
                grid.ActualizarDataGrid();
            }
            tableLimits.Visible = false;
        }

    }

    // que hacer a la hora de actualizar un registro
    public void UpdateCommand(Object sender, DataGridCommandEventArgs e)
    {
        lblError.Text = "";
        DataTable dt = grid.GridData.Tables[0];
        DataRow drLast = dt.Rows[dt.Rows.Count - 1];

        string sql = "SELECT * FROM productCompany where product = " + ApplicationConstants.APPLICATION + "and company = " + ApplicationConstants.COMPANY;
        var con = new dbConexion(ApplicationConstants.ADMINDB);
        DataTable dtLimit = con.ExecuteDS(sql).Tables[0];
        decimal limitFee = 0;
        decimal.TryParse(dtLimit.Rows[0]["LimitPolicyFee"].ToString(), out limitFee);
        decimal minPremium, maxPremium, minFee, maxFee = 0;

        var minimoPremium = ((TextBox) e.Item.Cells[1].Controls[0]).Text;
        var maximoPremium = ((TextBox) e.Item.Cells[2].Controls[0]).Text;
        var minimoFee = ((TextBox) e.Item.Cells[3].Controls[0]).Text;
        var maximoFee = ((TextBox) e.Item.Cells[4].Controls[0]).Text;

        decimal.TryParse(minimoPremium, out minPremium);
        decimal.TryParse(maximoPremium, out maxPremium);
        decimal.TryParse(minimoFee, out minFee);
        decimal.TryParse(maximoFee, out maxFee);

        if (maxFee > limitFee)
        {
            maxFee = limitFee;
        }

        con = new dbConexion(ApplicationConstants.DB);

        if (drLast.RowState == DataRowState.Added)
        {
            string sqlBranch = "SELECT * FROM [admonPoliza].[dbo].[branchDetail] where product = " + ApplicationConstants.APPLICATION + " and company = " + ApplicationConstants.COMPANY + " and branch = " + ddlBranch.SelectedItem.Value;
            DataTable dtBranchDetail = con.ExecuteDS(sqlBranch).Tables[0];
            if (dtBranchDetail.Rows.Count == 0)
            {
                lblError.Text = "Branchdetail doesn't exists for this branch, try another one.";
            }
            else
            {
                int branchDetail = 0;
                int.TryParse(dtBranchDetail.Rows[0]["branchDetail"].ToString(), out branchDetail);
                string sqlIns = "INSERT INTO policyFeeLimits VALUES (" + branchDetail + ", " + minPremium + ", " + maxPremium + ", " + minFee + ", " + maxFee + ")";
                con.ExecuteDS(sqlIns);
            }
        }
        else
        {
            // Codigo para manipular el registro editado		
            int idUpd = int.Parse(grid.DataKeys[e.Item.ItemIndex].ToString());
            string sqlUpd = "UPDATE policyFeeLimits SET lowLimitNetPremium = " + minPremium + " , highLimitNetPremium = " + maxPremium + " , lowLimitFee = " + minFee + " , highLimitFee = " + maxFee + " where policyFeeLimit = " + idUpd;
            try
            {
                con.ExecuteDS(sqlUpd);
            }
            catch
            {
            }
        }

        grid.EditItemIndex = -1;
        RefreshGrid();
    }

    public void Delete(Object sender, DataGridCommandEventArgs e)
    {
        string sql = "DELETE policyFeeLimits where policyFeeLimit = ";
        var con = new dbConexion(ApplicationConstants.DB);

        lblError.Visible = false;
        foreach (DataGridItem dgi in grid.SelectedItems)
        {
            string exeSQL = sql + grid.DataKeys[dgi.ItemIndex].ToString();
            con.ExecuteDS(exeSQL);
            RefreshGrid();
        }
    }

    private void grid_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        // *****************************   F O O T E R   ***********************************			 
        if (e.Item.ItemType == ListItemType.Footer)
        {
            // buscamos el link de Unselect y lo ligamos con la funcion para que lo haga
            LinkButton lb = (LinkButton) e.Item.FindControl("lnkBorrar");
            lb.Text = "Delete";

            lb = (LinkButton) e.Item.FindControl("lnkAgregar");
            lb.Text = "Add";
            //lb.Click += new EventHandler(Agregar);

            lb = (LinkButton) e.Item.FindControl("lnkUnselect");
            lb.Text = "Unselect";
        }
    }
    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        //this.ddlAgent.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
        //this.grid.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grid_ItemCreated);
        //this.grid.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grid_ItemCommand);
        //this.ID = "company";
        this.Load += new System.EventHandler(this.Page_Load);
        this.PreRender += new System.EventHandler(this.Page_PreRender);

    }
    #endregion

    //private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
    //{
    //    RefreshGrid();
    //}

    //private void grid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    //{
    //    //if (e.CommandName == "select")
    //    //    Response.Redirect("branchDetailv2.aspx?branch=" + grid.DataKeys[e.Item.ItemIndex].ToString() + "&agent=" + ddlAgent.SelectedItem.Value);
    //}

    //private void Agregar(object sender, System.EventArgs e)
    //{
    //    //Response.Redirect("branchDetailv2.aspx?agent=" + ddlAgent.SelectedItem.Value);
    //}

    protected void ddlSuperProducer_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshProducer(int.Parse(ddlSuperProducer.SelectedItem.Value));
        if (ddlProducer.Items.Count > 0)
        {
            RefreshAgent(int.Parse(ddlProducer.SelectedItem.Value));
            if (ddlAgent.Items.Count > 0)
            {
                RefreshBranch(int.Parse(ddlAgent.SelectedItem.Value));
            }
            else
            {
                ddlBranch.Items.Clear();
            }
        }
        else
        {
            ddlAgent.Items.Clear();
        }

        RefreshGrid();
    }

    protected void ddlProducer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProducer.Items.Count > 0)
        {
            RefreshAgent(int.Parse(ddlProducer.SelectedItem.Value));
            if (ddlAgent.Items.Count > 0)
            {
                RefreshBranch(int.Parse(ddlAgent.SelectedItem.Value));
            }
            else
            {
                ddlBranch.Items.Clear();
            }
        }
        RefreshGrid();
    }

    protected void ddlAgent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAgent.Items.Count > 0)
        {
            RefreshBranch(int.Parse(ddlAgent.SelectedItem.Value));
        }
        else
        {
            ddlBranch.Items.Clear();
        }

        RefreshGrid();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshGrid();
    }

    protected void chkFee_CheckedChanged(object sender, EventArgs e)
    {
        using (var conn = ApplicationConstants.GetHomeownerConnection())
        {
            string nameT = "update_change_fee";
            var trans = ApplicationConstants.BeginTransaction(conn, nameT);

            try
            {
                var command = new SqlCommand("spUpdChangePolicyFee", conn, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@product", ApplicationConstants.APPLICATION));
                command.Parameters.Add(new SqlParameter("@company", ApplicationConstants.COMPANY));
                command.Parameters.Add(new SqlParameter("@branch", int.Parse(ddlBranch.SelectedItem.Value)));
                command.Parameters.Add(new SqlParameter("@change", chkFee.Checked));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ApplicationConstants.RollbackTransaction(trans, nameT);
                trans = null;
                throw new Exception(ex.Message);
            }
            finally
            {
                if (trans != null)
                {
                    ApplicationConstants.CommitTransaction(trans);
                    trans = null;
                }
            }
        }

        RefreshGrid();
    }
}