<%@ WebHandler Language="C#" Class="ReportExcel" %>

using Protec.Authentication.Membership;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.SessionState;

public class ReportExcel : IHttpHandler, IReadOnlySessionState
{

	public void ProcessRequest(HttpContext context)
	{
		HttpRequest Request = context.Request;
		bool quote = Request["quote"] == "1";
		bool outdoor = Request["outdoor"] == "2";
		bool test = Request["test"] == "1";
		string agentes = BuildAgentesString(context.Response, context.Session);


		//choose the search engine
		SearchReportEngine engine = quote
						? new QuoteSearchReport(agentes)
						: outdoor ? new PolicyOutdoorPropertySearchReport(agentes, test) : (SearchReportEngine) new PolicySearchReport(agentes, test);

		//get the data
		IEnumerable<SearchResult> results = null;
		string startDate = Request["startDate"];
		string endDate = Request["endDate"];
		string id = Request["id"];
		string name = Request["name"];
		string agent = Request["agent"];
		bool expiring = !quote && Request["expiring"] == "1";
		bool includeRenewed = !quote && Request["includeRenewed"] == "1";

		string title = BuildTitle(quote, id, name, startDate, endDate, agent, expiring, outdoor);

		if (expiring)
		{
			//expiring policies
			results = engine.SearchExpiring(startDate, endDate, includeRenewed);
		}
		else
		{
			//quote or policies
			results = engine.Search(id, name, startDate, endDate, agent);
		}


		if (results == null)
		{
			context.Response.ContentType = "text/plain";
			string qOrP = quote ? "quote" : "policy";
			context.Response.Write("There is no such " + qOrP + " with such data");
			return;
		}

		IResultViewer viewer = new ExcelReportViewer();
		var searchResults = results.ToList();
		ReportTable report = ReportTable.Build(
						title,
						searchResults,
						RowTitles(quote, outdoor),
						new ExcelRowBuilder()
		);

		if (report == null)
		{
			context.Response.ContentType = "text/plain";
			string qOrP = quote ? "quote" : "policy";
			context.Response.Write("There is no such " + qOrP + " with such data");
			return;
		}

		string filename = GenerateFileName(quote, outdoor);

		//Workbook workbook = (Workbook) viewer.GetViewOfResult(report);
		//context.Response.ContentType = "application/vnd.ms-excel";
		//context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".xls");
		//workbook.Save(context.Response.OutputStream);

		//DataTable table = ExcelExportHelper.ToDataTable(searchResults);
		//var contentExcel = ExcelExportHelper.ExportExcel(table, title, remove);

		Dictionary<string, string> renameHeaders = new Dictionary<string, string>()
		{
			{ "id", "Quote"},
			{ "issuedBy", "Issued By"},
			{ "policyNo", "Policy No"},
			{ "city", "City"},
			{ "firstName", "First Name"},
			{ "lastName", "Last Name"},
			{ "lastName2", "Last Name (scnd)"},
			{ "RFC", "RFC"},
			{ "state", "State"},
			{ "street", "Street"},
			{ "zipCode", "Zip code"},
			{ "interiorNo", "Interior No"},
			{ "exteriorNo", "Exterior No"},
			{ "issueDate", "Issue Date"},
			{ "startDate", "Start Date"},
			{ "endDate", "End Date"},
			{ "netPremium", "Net Premium"},
			{ "rxpf", "RXPF"},
			{ "rxpfPercentage", "RXPF (Percentage)"},
			{ "taxTotal", "Tax"},
			{ "fee", "Fee"},
			{ "Currency", "Currency"},
			{ "TaxPercentage", "Tax (Percentage)"},
			{ "PolicyNoRenewal", "Policy No (Renewal)"},
			{ "outdoorDescription", "Outdoor Description"},
			{ "outdoor", "Outdoor"},
		};

		string[] remove = quote
			? (new[] { "issuedBy", "policyNo", "lastName2", "RFC", "startDate", "endDate", "Currency", "TaxPercentage", "PolicyNoRenewal", "outdoorDescription", "outdoor" })
			: (new[] { "id", "issuedBy", "lastName2", "RFC", "Currency", "TaxPercentage", "PolicyNoRenewal", "outdoor" });

		ExcelExportHelper.ExcelExportHelper excel = new ExcelExportHelper.ExcelExportHelper(Color.Black, Color.White, Color.DarkRed, Color.Black, "Sheet1", title);

		var contentExcel = excel.ExportExcel(searchResults, renameHeaders, remove);

		context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		context.Response.AddHeader("content-disposition", $"attachment;  filename=Report_{filename}.xlsx");
		context.Response.BinaryWrite(contentExcel);
	}

	private string GenerateFileName(bool quote, bool outdoor)
	{
		Random random = new Random();
		string filename = quote ? "Quotes" : outdoor ? "Policies_Outdoor" : "Policies";
		filename += (1 + random.Next(100)).ToString();
		return filename;
	}

	private string BuildTitle(bool quote, string id, string name, string startDate, string endDate, string agent, bool expiring, bool outdoor)
	{
		if (expiring)
		{
			return "Expiring policies " + startDate + " - " + endDate;
		}
		string title = quote ? "Quotes with " : outdoor ? "Policies with outdoor's description " : "Policies with ";
		if (!string.IsNullOrEmpty(id))
		{
			title += " number " + id;
		}
		else if (!string.IsNullOrEmpty(name))
		{
			title += " client's name of \"" + name + "\"";
		}
		else if (!string.IsNullOrEmpty(startDate))
		{
			title += " issue date between " + startDate.ToString() + " and ";
			if (!string.IsNullOrEmpty(endDate))
			{
				title += endDate.ToString();
			}
			else
			{
				title += startDate.ToString();
			}
		}
		else if (!string.IsNullOrEmpty(agent))
		{
			title += " agent number " + agent;
		}
		else
		{
			title = "NO TITLE";
		}
		return title;
	}

	private string[] RowTitles(bool quote, bool outdoor)
	{
		string[] rows = quote
						? (new string[] {
																"ID",
																"First Name",
																"Last Name",
																"Street",
																"Exterior Number",
																"Interior Number",
																"City",
																"State",
																"Zip Code",
																"Issue Date",
																"Currency",
																"Net Premium",
																"Fee",
																"Tax",
																"Total"
						})
						: outdoor
										? (new string[] {
																"ID",
																"Policy Number",
																"First Name",
																"Last Name",
																"Street",
																"Exterior Number",
																"Interior Number",
																"City",
																"State",
																"Zip Code",
																"Issue Date",
																"Outdoor Description",
																"Net Premium",
																"Fee",
																"Tax",
																"Total"
																		})
										: (new string[] {
																"ID",
																"Policy Number",
																"First Name",
																"Last Name",
																"Street",
																"Exterior Number",
																"Interior Number",
																"City",
																"State",
																"Zip Code",
																"Issue Date",
																"Net Premium",
																"Fee",
																"Tax",
																"Total"
																		});
		return rows;
	}

	private string BuildAgentesString(HttpResponse Response, HttpSessionState Session)
	{
		string agentes = string.Empty;
		try
		{
			UserInformation user = (UserInformation) HttpContext.Current.User.Identity;
			agentes = user.Agent == ApplicationConstants.MEAGENT || user.UserID == ApplicationConstants.PTI_USER
							? null
							: AdminGateway.Gateway.ListAllUsersFromAgent(user.Agent, user.UserID);
		}
		catch
		{
			Response.Redirect("~/Login.aspx", true);
			return null;
		}
		return agentes;
	}

	public bool IsReusable
	{
		get
		{
			return false;
		}
	}

}