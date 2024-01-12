using ME.Admon;
using Protec.Authentication.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

public class HtmlRowBuilder : IRowBuilder
{
	private UserInformation UserInfo;
	private List<int> usersInAgent = new List<int>();
	private string startDate, endDate;
	private bool test;
	private bool includeRenewed;
	public HtmlRowBuilder()
	{
		UserInfo = (UserInformation) HttpContext.Current.User.Identity;
		usersInAgent.AddRange(UserInfo.ListUsersInAgent());
		test = false;
	}
	public HtmlRowBuilder(string startDate, string endDate, bool test) :
		this(startDate, endDate, test, false)
	{
	}

	public HtmlRowBuilder(string startDate, string endDate, bool test, bool includeRenewed) :
		this()
	{
		this.startDate = startDate;
		this.endDate = endDate;
		this.test = test;
		this.includeRenewed = includeRenewed;
	}

	private bool MustDisplayGenerateButton(SearchResult result)
	{
		IList<int> branchIDs = HierarchyExceptionRepository.Repository.ListHierarchyIDExceptions(
						"branch",
						ApplicationConstants.APPLICATION,
						"AllowRenew"
					);

		int monthsAvailable = (UserInfo.Agent == ApplicationConstants.MEAGENT || UserInfo.UserID == ApplicationConstants.PTI_USER) ? 2 : 1;
		return UserInfo.UserType == SecurityManager.ADMINISTRATOR ||
			   ((UserInfo.Agent == ApplicationConstants.MEAGENT ||
				(UserInfo.UserType == SecurityManager.ADMINAG && usersInAgent.Contains(result.issuedBy)) ||
			   result.issuedBy == UserInfo.UserID || UserInfo.UserID == ApplicationConstants.PTI_USER || branchIDs.Contains(UserInfo.Branch)) &&
			   DateTime.Parse(result.endDate) <= DateTime.Today.AddMonths(monthsAvailable));
	}

	public void BuildRow(RowReport row, SearchResult result)
	{
		FieldReport field = new FieldReport();
		if (string.IsNullOrEmpty(result.policyNo))
		{
			field.Lines.Add("Quote number: " + result.id);
		}
		else
		{
			field.Lines.Add("Policy: " + result.id);
			field.Lines.Add("Policy number: " + result.policyNo);
		}
		field.Lines.Add("Issue Date: " + result.issueDate);

		if (!string.IsNullOrEmpty(result.startDate) && !string.IsNullOrEmpty(result.endDate))
		{
			field.Lines.Add("Validity: " + (result.startDate ?? "N/A") + " - " + (result.endDate ?? "N/A"));
		}

		string lastName = string.IsNullOrEmpty(result.lastName) ? string.Empty : " " + result.lastName.ToUpper();
		string lastName2 = string.IsNullOrEmpty(result.lastName2) ? string.Empty : " " + result.lastName2.ToUpper();

		field.Lines.Add("Client's name: " + result.firstName.ToUpper() + lastName + lastName2);
		string interior = string.IsNullOrEmpty(result.interiorNo) ? string.Empty : "-" + result.interiorNo;
		field.Lines.Add("Risk Address: " + result.street + " " + result.exteriorNo + interior);
		field.Lines.Add(result.city + ", " + result.state + " " + result.zipCode);
		if (!string.IsNullOrEmpty(result.outdoorDescription) && result.outdoor)
		{
			field.Lines.Add("Outdoor Description: " + result.outdoorDescription);
		}
		row.Fields.Add(field);

		field = new FieldReport();
		field.Lines.Add("Net Premium: " + result.netPremium.ToString("C"));
		field.Lines.Add("Fee: " + (result.netPremium > 0 ? result.fee.ToString("C") : "N/A"));
		field.Lines.Add("RXPF: " + (result.netPremium > 0 ? result.rxpf.ToString("C") : "N/A"));
		field.Lines.Add("Tax: " + (result.netPremium > 0 ? result.taxTotal.ToString("C") : "N/A"));
		decimal total = 0;
		if (result.netPremium > 0)
		{
			total = ApplicationConstants.CalculateTotalWithData(result.netPremium, result.taxTotal, result.rxpf, result.fee);
		}
		field.Lines.Add("Total: " + (total > 0 ? total.ToString("C") : "N/A"));
		row.Fields.Add(field);

		if (includeRenewed)
		{
			field = new FieldReport();
			field.Lines.Add(result.PolicyNoRenewal);
			row.Fields.Add(field);
		}

		field = new FieldReport();
		if (string.IsNullOrEmpty(result.policyNo))
		{
			string link = VirtualPathUtility.ToAbsolute("~/Application/Load.aspx") + "?option=quote&id=" + result.id;
			field.Lines.Add("<a href=\"" + link + "\">Load Quote</a>");
		}
		else
		{
			string link = "location.href = '" + VirtualPathUtility.ToAbsolute("~/GetPoliza.ashx") + "?poliza=" + result.id + "'";
			string englishLink = "location.href = '" + VirtualPathUtility.ToAbsolute("~/GetPoliza.ashx") + "?eng=1&poliza=" + result.id + "'";
			string genQuoteLink = "location.href = '" + VirtualPathUtility.ToAbsolute("~/GenerateQuote.ashx") + "?id=" + result.id;
			string invoiceLink = "location.href = '" + VirtualPathUtility.ToAbsolute("~/GetInvoice.ashx") + "?id=" + result.id + "'";
			string endorsmentLink = "location.href = '" + VirtualPathUtility.ToAbsolute("~/Application/DirectCaptureEndorsmentInput.aspx") + "?policyNo=" + result.policyNo + "'";
			if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
			{
				genQuoteLink += "&startDateExp=" + startDate;
				genQuoteLink += "&endDateExp=" + endDate;
				genQuoteLink += test ? "&test=1" : "&test=0";
			}
			result.PolicyNoRenewal = string.IsNullOrEmpty(result.PolicyNoRenewal) ? "" : result.PolicyNoRenewal;
			genQuoteLink += "'";
			if (!result.PolicyNoRenewal.Equals("ENDORSMENT"))
			{
				field.Lines.Add("<input type=\"button\" onclick=\"" + link + "\" value=\"Get Policy\" />");
				field.Lines.Add("<input type=\"button\" onclick=\"" + englishLink + "\" value=\"Get Policy (English version)\" />");
				if (!string.IsNullOrEmpty(result.RFC))
				{
					field.Lines.Add("<input type=\"button\" onclick=\"" + invoiceLink + "\" value=\"Invoice PDF\" />");
				}
				if (result.endDate != null && MustDisplayGenerateButton(result))
				{
					field.Lines.Add("<input type=\"button\" onclick=\"" + genQuoteLink + "\" value=\"Generate Quote\" />");
				}
			}
			else
			{
				field.Lines.Add("<input type=\"button\" onclick=\"" + endorsmentLink + "\" value=\"Add Endorsment\" />");
			}
		}
		row.Fields.Add(field);
	}
}

public class HtmlReportViewer : IResultViewer
{
	public object GetViewOfResult(ReportTable report)
	{
		HtmlTable table = new HtmlTable();
		HtmlTableRow row = new HtmlTableRow();
		foreach (string header in report.RowTitles)
		{
			HtmlTableCell cellTh = new HtmlTableCell("th");
			cellTh.Attributes.Add("class", "calculaTableHeader");
			cellTh.InnerHtml = header;

			row.Cells.Add(cellTh);
		}
		table.Rows.Add(row);
		bool gray = true;
		foreach (RowReport rowReport in report.Rows)
		{
			row = new HtmlTableRow();
			row.Attributes.Add("class", gray ? "catalogTable" : "catalogTableAlternate");

			foreach (FieldReport field in rowReport.Fields)
			{
				HtmlTableCell cell = new HtmlTableCell();
				cell.InnerHtml = string.Join("<br />", field.Lines.ToArray());
				row.Cells.Add(cell);
			}
			row.Cells[row.Cells.Count - 1].Align = "center";
			gray = !gray;
			table.Rows.Add(row);
		}

		return table;
	}
}

