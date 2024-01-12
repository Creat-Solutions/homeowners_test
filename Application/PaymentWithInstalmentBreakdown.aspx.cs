using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Application_PaymentWithInstalmentBreakdown : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int quoteid = 0, numOfPayments = 0;
        plhBreakdown.Controls.Clear();

        try {
            quoteid = int.Parse(Request.QueryString["id"]);
            numOfPayments = int.Parse(Request.QueryString["numOfPayments"]);
        } catch {
            quoteid = numOfPayments = 0;
        }

        if (quoteid == 0 || numOfPayments == 0) {
            plhBreakdown.Controls.Add(new Label {
                ID = "lblError",
                Text = "Insufficient data"
            });
        }

        if (numOfPayments != 1 && numOfPayments != 12 && numOfPayments != 4 && numOfPayments != 2) {
            plhBreakdown.Controls.Add(new Label {
                ID = "lblError",
                Text = "Incorrect data (payments)"
            });
        }

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        quote q = null;
        decimal netPremium = 0;
        decimal taxPercentage = 0;
        decimal rxpfP = 0;

        try {
            q = db.quotes.Single(m => m.id == quoteid);
            netPremium = db.quoteDetails.Where(m => m.quote == quoteid).Select(m => m.prima).Sum().Value;
            taxPercentage = db.fGet_QuoteTaxPercentage(quoteid).Value;
            rxpfSur sur = db.rxpfSurs.Single(m => m.currency == q.currency && m.company == q.company && m.month == DateTime.Now.Month);
            switch (numOfPayments) {
                case 1:
                default:
                    rxpfP = 0;
                    break;
                case 2:
                    rxpfP = sur.biyearly;
                    break;
                case 4:
                    rxpfP = sur.quarterly;
                    break;
                case 12:
                    rxpfP = sur.monthly;
                    break;
            }
        } catch {
            q = null;
        }

        if (q == null) {
            plhBreakdown.Controls.Add(new Label {
                ID = "lblError",
                Text = "Incorrect data (quoteid)"
            });
        }

        plhBreakdown.Controls.Add(GenerateBreakdown(q, netPremium, taxPercentage, rxpfP, numOfPayments));
    }

    private HtmlTable GenerateBreakdown(quote q, decimal netPremium, decimal taxPercentage, decimal rxpfP, int numOfPayments)
    {
        HtmlTable table = new HtmlTable {
            ID = "tblBreakdown",
            Border = 0,
            CellPadding = 5
        };
        table.Attributes.Add("class", "pagoFraccDesglose");

        table.Rows.Add(new HtmlTableRow());
        table.Rows.Add(new HtmlTableRow());
        table.Rows.Add(new HtmlTableRow());
        table.Rows.Add(new HtmlTableRow());
        table.Rows.Add(new HtmlTableRow());
        table.Rows.Add(new HtmlTableRow());

        decimal netPremiumPayments = netPremium / numOfPayments;
        decimal fee = ApplicationConstants.Fee(q.currency, q.fee.Value);
        decimal rxpfPayments = netPremiumPayments * rxpfP;
        
        for (int i = 1; i <= numOfPayments; i++) {
            HtmlTableCell cell = new HtmlTableCell("th");
            cell.InnerText = i.ToString();
            cell.Attributes.Add("class", "fullBorder");
            table.Rows[0].Cells.Add(cell);

            cell = new HtmlTableCell {
                Align = "right",
                InnerText = netPremiumPayments.ToString("C")
            };
            cell.Attributes.Add("class", "halfBorder");
            table.Rows[1].Cells.Add(cell);

            decimal feeCycle = i == 1 ? fee : 0M;
            cell = new HtmlTableCell {
                Align = "right",
                InnerText = feeCycle.ToString("C")
            };
            cell.Attributes.Add("class", "halfBorder");
            table.Rows[2].Cells.Add(cell);

            cell = new HtmlTableCell {
                Align = "right",
                InnerText = rxpfPayments.ToString("C")
            };
            cell.Attributes.Add("class", "halfBorder");
            table.Rows[3].Cells.Add(cell);

            decimal tax = (netPremiumPayments + rxpfPayments + feeCycle) * taxPercentage;
            cell = new HtmlTableCell {
                Align = "right",
                InnerText = tax.ToString("C")
            };
            cell.Attributes.Add("class", "halfBorder");
            table.Rows[4].Cells.Add(cell);

            cell = new HtmlTableCell {
                Align = "right",
                InnerText = (netPremiumPayments + rxpfPayments + feeCycle + tax).ToString("C")
            };
            cell.Attributes.Add("class", "fullBorder");
            table.Rows[5].Cells.Add(cell);
        }

        return table;
    }
}
