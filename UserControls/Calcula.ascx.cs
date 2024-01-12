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
using ME.Admon;

public partial class UserControls_Calcula : System.Web.UI.UserControl, IQuoteData
{

    private int _quoteid = 0;
    private HouseInsuranceDataContext db  = new HouseInsuranceDataContext();
    public int quoteid
    {
        get { return _quoteid; }
        set
        {
            _quoteid = value;
            odsCalcula.SelectParameters[0].DefaultValue = _quoteid.ToString();
            grdCalcula.DataBind();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (quoteid == 0)
            return;
        
        var dquote = db.quotes.Where(m => m.id == quoteid).Single();
        HouseEstate he = dquote.insuredPersonHouse1.HouseEstate;
        var codigos = db.spList_Calcula(quoteid, he.City.HMP).Single();
        decimal netPremium = codigos.netPremium ?? 0;
        decimal comision = codigos.comision ?? 0;
        if (netPremium == 0)
            return;
        decimal comporcentaje = comision / netPremium * 100;
        bool HMP = (bool)dquote.HMP, quake = (bool)dquote.quake;
        lblName.Text = "Nombre: " + codigos.name + " " + (codigos.lastName ?? string.Empty);
        lblDiscount.Text = "Descuento: " + dquote.descuento.ToString("P");
        lblCurrency.Text = "Moneda: " + (dquote.currency == ApplicationConstants.PESOS_CURRENCY ? "Pesos" : "Dólares");
        lblQuote.Text = "Cotizacion " + quoteid.ToString();
        lblZonaRobo.Text = he.City.State.TheftZone;
        lblTEV.Text = he.TEV.ToString();
        lblFHM.Text = he.City.HMP.ToString();
        lblClientType.Text = codigos.typeOfClient;
        lblType.Text = codigos.typeOfProperty;
        lblUse.Text = codigos.useOfProperty;
        lblDistanceSea.Text = codigos.distanceFromSea;
        lblAdjoining.Text = (bool)codigos.adjoiningEstates ? "Con colindantes sin fincar" : "Sin colindantes sin fincar";
        lblRoof.Text = codigos.roofType;
        lblWall.Text = codigos.wallType;
        lblGuards.Text = ((bool)codigos.guards24).ToString().ToUpper();
        lblCentralAlarm.Text = ((bool)codigos.centralAlarm).ToString().ToUpper();
        lblLocalAlarm.Text = ((bool)codigos.localAlarm).ToString().ToUpper();
        lblTVCircuit.Text = ((bool)codigos.tvCircuit).ToString().ToUpper();
        lblTVGuard.Text = ((bool)codigos.tvGuard).ToString().ToUpper();
        decimal fee = ApplicationConstants.Fee(dquote.currency, dquote.fee.Value);
        decimal rxpf = dquote.recargoPagoFraccionado;
        decimal rxpfPercentage = dquote.porcentajeRecargoPagoFraccionado;
        decimal taxPercentage = db.fGet_QuoteTaxPercentage(quoteid).Value;
        decimal tax = ApplicationConstants.CalculateTax(netPremium, taxPercentage, rxpfPercentage, dquote.currency, dquote.fee.Value);
        //decimal tax = (netPremium + fee) * ApplicationConstants.TAX;
        lblDerechos.Text = fee.ToString("C");
        lblPrima.Text = Math.Round(netPremium, 2).ToString("C");
        lblTax.Text = Math.Round(tax, 2).ToString("C");
        lblPorTax.Text = (taxPercentage * 100).ToString("F2");
        lblComision.Text = comision.ToString("C");
        lblPorComision.Text = comporcentaje.ToString("F2");
        lblRXPF.Text = rxpf.ToString("C");
        lblPorRXPF.Text = rxpfPercentage.ToString("P");
        lblTotal.Text = ApplicationConstants.CalculateTotal(netPremium, taxPercentage, rxpfPercentage, dquote.currency, dquote.fee.Value).ToString("C2");
        lblHMP.Text = HMP ? "Covered / Cubierto" : "Excluded / Exclu&iacute;do";
        lblQuake.Text = quake ? "Covered / Cubierto" : "Excluded / Exclu&iacute;do";
    }

    private bool edificios = true;
    private bool contenidos = true;
    private bool perdidaRentas = true;
    private bool gastosExtras = true;
    private bool remocionEscombros = true;
    private bool robo = true;

    private TableCell[] CreateCells(int code, string text, int cols, string cssClass)
    {
        TableCell[] cells = new TableCell[2];
        cells[0] = new TableCell();
        Label lbl = new Label();
        lbl.CssClass = cssClass;
        lbl.Text = code.ToString();
        cells[0].Controls.Add(lbl);

        cells[1] = new TableCell();
        cells[1].ColumnSpan = cols - 1;
        lbl = new Label();
        lbl.CssClass = cssClass;
        lbl.Text = text;
        cells[1].Controls.Add(lbl);
        return cells;
    }

    private int adder = 1;

    protected void grdCalcula_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView grdCalcula = sender as GridView;
        if (e.Row.RowType == DataControlRowType.DataRow) {
            int codigo = (int)DataBinder.Eval(e.Row.DataItem, "codigoCobertura");
            int numrows = grdCalcula.Rows.Count + adder;
            Table tblParent = e.Row.Parent as Table;
            int cols = grdCalcula.Columns.Count;
            if (codigo > 3000 && edificios) {
                adder++;
                edificios = false;
                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                row.Cells.AddRange(CreateCells(3000, "EDIFICIO", cols, "calculaSubHeader"));
                tblParent.Rows.AddAt(numrows, row);
                
            } else if (codigo > 3050 && contenidos) {
                adder++;
                contenidos = false;
                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                row.Cells.AddRange(CreateCells(3050, "CONTENIDOS", cols, "calculaSubHeader"));
                tblParent.Rows.AddAt(numrows, row);

            } else if (codigo > 3210 && remocionEscombros) {
                adder++;
                remocionEscombros = false;
                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                row.Cells.AddRange(CreateCells(3210, "REMOCI&Oacute;N ESCOMBROS", cols, "calculaSubHeader"));
                tblParent.Rows.AddAt(numrows, row);

            } else if (codigo > 3220 && gastosExtras) {
                adder++;
                gastosExtras = false;
                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                row.Cells.AddRange(CreateCells(3220, "GASTOS EXTRAS CASA HABITACI&Oacute;N", cols, "calculaSubHeader"));
                tblParent.Rows.AddAt(numrows, row);

            } else if (codigo > 3230 && perdidaRentas) {
                adder++;
                perdidaRentas = false;
                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                row.Cells.AddRange(CreateCells(3230, "P&Eacute;RDIDA DE RENTAS", cols, "calculaSubHeader"));
                tblParent.Rows.AddAt(numrows, row);
            } else if (codigo > 3500 && robo) {
                adder++;
                robo = false;
                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                row.Cells.AddRange(CreateCells(3500, "ROBO DE MENAJE", cols, "calculaSubHeader"));
                tblParent.Rows.AddAt(numrows, row);
            }
            Label lblCovered = e.Row.FindControl("lblCovered") as Label;
            decimal amount = (decimal)DataBinder.Eval(e.Row.DataItem, "monto");
            switch (codigo) {
                case 3910:
                case 3911:
                case 3920:
                case 3930:
                    bool covered = db.quotes.Where(m => m.id == quoteid).Select(m => m.familyAssistance).Single();
                    lblCovered.Text = covered ? "COVERED/CUBIERTO" : "EXCLUDED/EXCLUÍDO";
                    break;
                case 3940:
                case 3941:
                    lblCovered.Text = "EXCLUDED/EXCLUÍDO";
                    break;
                default:
                    lblCovered.Text = amount > 0M ? amount.ToString("C2") : "EXCLUDED/EXCLUÍDO";
                    break;
            }
        }
    }
}
