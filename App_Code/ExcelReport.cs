using CarlosAg.ExcelXmlWriter;
using System;

public class ExcelRowBuilder : IRowBuilder
{
	public void BuildRow(RowReport row, SearchResult result)
	{
		row.Fields.Add(new FieldReport(result.id.ToString()));
		if (!string.IsNullOrEmpty(result.policyNo))
		{
			row.Fields.Add(new FieldReport(result.policyNo));
		}
		row.Fields.Add(new FieldReport(result.firstName));
		row.Fields.Add(new FieldReport(result.lastName));
		row.Fields.Add(new FieldReport(result.street));
		row.Fields.Add(new FieldReport(result.exteriorNo));
		row.Fields.Add(new FieldReport(result.interiorNo));
		row.Fields.Add(new FieldReport(result.city));
		row.Fields.Add(new FieldReport(result.state));
		row.Fields.Add(new FieldReport(result.zipCode));
		row.Fields.Add(new FieldReport(result.issueDate));
		if (result.outdoor)
		{
			row.Fields.Add(new FieldReport(result.outdoorDescription));
		}
		if (result.Currency != null)
		{
			string moneda = result.Currency.Value == 1 ? "Dolares" : "Pesos";
			row.Fields.Add(new FieldReport(moneda));
		}
		row.Fields.Add(new FieldReport(result.netPremium.ToString("F2"), true));
		row.Fields.Add(new FieldReport(result.fee.ToString("F2"), true));
		row.Fields.Add(new FieldReport(result.taxTotal.ToString("F2"), true));
		row.Fields.Add(new FieldReport(""));
	}
}

public class ExcelReportViewer : IResultViewer
{
	public object GetViewOfResult(ReportTable report)
	{
		Workbook workbook = new Workbook();
		workbook.Properties.Author = "Macafee and Edwards";
		workbook.Properties.Company = "Macafee and Edwards";
		workbook.Properties.Title = report.Title;
		workbook.Properties.Created = DateTime.Now;

		Worksheet sheet = workbook.Worksheets.Add("Report");
		WorksheetRow row = sheet.Table.Rows.Add();
		row.Cells.Add(report.Title);
		sheet.Table.Rows.Add();
		row = sheet.Table.Rows.Add();
		foreach (string title in report.RowTitles)
		{
			row.Cells.Add(title);
		}

		foreach (RowReport rowReport in report.Rows)
		{
			row = sheet.Table.Rows.Add();
			foreach (FieldReport field in rowReport.Fields)
			{
				string data = field.ToString().Trim();
				if (field.IsNumber)
				{
					try
					{
						decimal numericData = decimal.Parse(data);
						WorksheetCell cell = new WorksheetCell(data, DataType.Number);
						row.Cells.Add(cell);
					}
					catch
					{
						row.Cells.Add(data);
					}
				}
				else
				{
					row.Cells.Add(data);
				}
			}

			row.Cells.RemoveAt(row.Cells.Count - 1);
			WorksheetCell cellFinal = row.Cells.Add();
			cellFinal.Formula = "=SUM(RC[-3]:RC[-1])";
		}

		return workbook;
	}
}
