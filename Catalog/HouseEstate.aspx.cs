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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ME.Admon;
using Telepro.Conexion;

public partial class Catalog_HouseEstate : System.Web.UI.Page
{
    HomeServices home = new HomeServices();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void Page_PreRender(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void search_Click(object sender, EventArgs e)
    {
        var zip = txtZipCode.Text.Trim();

        if (zip.Length != 5)
            lblZipNotFound.Text = "Input a valid zipcode";
        else
        {
            lblZipNotFound.Text = "";
            LocationData locations = home.GetLocations(zip, null, null);

            if (locations.states.Count() >= 1 && locations.cities.Count() >= 1)
            {
                houseEstates.Visible = true;
                RefreshStates(locations);
                RefreshCities(locations);
                RefreshGrid();
            }
            else
            {
                lblZipNotFound.Text = "Doesn't exists the Zip Code in the database.";
                houseEstates.Visible = false;
            }
        }
    }

    private void RefreshStates(LocationData locations)
    {
        ddlState.Items.Clear();
        ddlState.DataSource = locations.states;
        ddlState.DataBind();

        if (locations.states.Count() == 1)
            ddlState.Enabled = false;
        else
            ddlState.Enabled = true;
    }

    private void RefreshCities(LocationData locations)
    {
        ddlCity.Items.Clear();
        ddlCity.DataSource = locations.cities;
        ddlCity.DataBind();

        if (locations.cities.Count() == 1)
            ddlCity.Enabled = false;
        else
            ddlCity.Enabled = true;
    }

    private List<HE> GetHouseEstateFromZipCode(string zipCode)
    {
        SearchParameter zipCodeParam = new SearchParameter
        {
            Operator = SearchOperator.EQUALS,
            Property = "ZipCode",
            Value = zipCode
        };
        var he = HouseEstate.Get(new List<SearchParameter>() { zipCodeParam }, ApplicationConstants.ADMINDB).Where(h => h.City.ID.Equals(int.Parse(ddlCity.SelectedValue)));

        return he.Select(h =>
        {
            return new HE
            {
                ID = h.ID,
                Name = h.Name,
                TEV = h.TEV,
                CityId = h.City.ID,
                City = h.City.Name,
                ZipCode = h.ZipCode
            };
        }).OrderBy(n => n.Name).ToList();
    }

    internal class HE
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public short TEV { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }

    private void RefreshGrid()
    {
        var houseEstates = GetHouseEstateFromZipCode(txtZipCode.Text.Trim());

        if (houseEstates.Count() >= 1)
        {
            grid.Ascendente = "yes";
            grid.SortExpression = "name";
            grid.GridData = ToDataSet(houseEstates);
            grid.CurrentPageIndex = 0;
            grid.ActualizarDataGrid();
            tableHouseEstate.Visible = true;
        }
        else
        {
            if (grid.GridData != null)
            {
                grid.GridData.Clear();
                grid.ActualizarDataGrid();
            }
            tableHouseEstate.Visible = false;
        }
    }

    private static DataSet ToDataSet<T>(IList<T> list)
    {
        Type elementType = typeof(T);
        DataSet ds = new DataSet();
        DataTable t = new DataTable();
        ds.Tables.Add(t);

        //add a column to table for each public property on T
        foreach (var propInfo in elementType.GetProperties())
        {
            t.Columns.Add(propInfo.Name, propInfo.PropertyType);
        }

        //go through each property on T and add each value to the table
        foreach (T item in list)
        {
            DataRow row = t.NewRow();
            foreach (var propInfo in elementType.GetProperties())
            {
                row[propInfo.Name] = propInfo.GetValue(item, null);
            }

            t.Rows.Add(row);
        }

        return ds;
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshGrid();
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshGrid();
    }

    public void UpdateCommand(Object sender, DataGridCommandEventArgs e)
    {
        DataTable dt = grid.GridData.Tables[0];
        DataRow drLast = dt.Rows[dt.Rows.Count - 1];

        dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);

        var Name = ((TextBox)e.Item.Cells[1].Controls[0]).Text.ToUpper();
        var TEV = ((TextBox)e.Item.Cells[2].Controls[0]).Text;
        short zonaTEV = 0;
        short.TryParse(TEV, out zonaTEV);
        int idUpd = 0;
        int.TryParse(grid.DataKeys[e.Item.ItemIndex].ToString(), out idUpd);

        if (idUpd == 0)
        {
            string sqlInsert = "INSERT INTO HouseEstate VALUES ('" + Name + "', " + zonaTEV + ", " + int.Parse(ddlCity.SelectedValue) + ", '" + txtZipCode.Text.Trim() + "', " + 0 + ")";
            con.ExecuteDS(sqlInsert);
        }
        else
        {
            string sqlUpdate = "UPDATE HouseEstate SET name = '" + Name + "', zonaTEV = " + zonaTEV + " where houseEstate = " + idUpd;
            con.ExecuteDS(sqlUpdate);
        }

        grid.EditItemIndex = -1;
        RefreshGrid();
    }

    public void Delete(Object sender, DataGridCommandEventArgs e)
    {

    }

    private void grid_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        // *****************************   F O O T E R   ***********************************			 
        if (e.Item.ItemType == ListItemType.Footer)
        {
            // buscamos el link de Unselect y lo ligamos con la funcion para que lo haga
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkBorrar");
            lb.Text = "Delete";

            lb = (LinkButton)e.Item.FindControl("lnkAgregar");
            lb.Text = "Add";

            lb = (LinkButton)e.Item.FindControl("lnkUnselect");
            lb.Text = "Unselect";
        }
    }

    override protected void OnInit(EventArgs e)
    {
        InitializeComponent();
        base.OnInit(e);
    }

    private void InitializeComponent()
    {
        this.Load += new System.EventHandler(this.Page_Load);
        this.PreRender += new System.EventHandler(this.Page_PreRender);
    }
}
