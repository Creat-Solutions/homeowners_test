using System.Collections.Generic;
using System.Linq;

public interface IResultViewer
{
	object GetViewOfResult(ReportTable table);
}

public interface IRowBuilder
{
	void BuildRow(RowReport row, SearchResult result);
}

public class SearchResult
{
	public int id { get; set; }
	public int issuedBy { get; set; }
	public string policyNo { get; set; }
	public string city { get; set; }
	public string firstName { get; set; }
	public string lastName { get; set; }
	public string lastName2 { get; set; }
	public string RFC { get; set; }
	public string state { get; set; }
	public string street { get; set; }
	public string zipCode { get; set; }
	public string interiorNo { get; set; }
	public string exteriorNo { get; set; }
	public string issueDate { get; set; }
	public string startDate { get; set; }
	public string endDate { get; set; }
	public decimal netPremium { get; set; }
	public decimal rxpf { get; set; }
	public decimal rxpfPercentage { get; set; }
	public decimal taxTotal { get; set; }
	public decimal fee { get; set; }
	public int? Currency { get; set; }
	public decimal? TaxPercentage { get; set; }
	public string PolicyNoRenewal { get; set; }
	public string outdoorDescription { get; set; }
	public bool outdoor { get; set; }
}

/// <summary>
/// Neutral reports results (from which it can be created an excel or html report
/// </summary>
public class ReportTable
{
	public ICollection<RowReport> Rows { get; private set; }
	public ICollection<string> RowTitles { get; private set; }
	public string Title { get; private set; }

	private IRowBuilder m_rowBuilder;

	public ReportTable(string title, IRowBuilder builder)
	{
		Title = title;
		Rows = new List<RowReport>();
		RowTitles = new List<string>();
		m_rowBuilder = builder;
	}

	public void AddRow(SearchResult result)
	{
		RowReport row = new RowReport();
		m_rowBuilder.BuildRow(row, result);
		Rows.Add(row);
	}

	public static ReportTable Build(string title, IEnumerable<SearchResult> results, IEnumerable<string> rowTitles, IRowBuilder builder)
	{
		ReportTable table = new ReportTable(title, builder);

		int titles = 0;
		foreach (var s in rowTitles)
		{
			table.RowTitles.Add(s);
			titles++;
		}

		int res = 0;
		foreach (var result in results)
		{
			table.AddRow(result);
			res++;
		}

		if (res == 0)
		{
			return null;
		}

		foreach (RowReport row in table.Rows)
		{
			if (row.Length != titles)
			{
				return null;
			}
		}

		return table;
	}
}

public class RowReport
{
	public ICollection<FieldReport> Fields { get; private set; }

	public RowReport()
	{
		Fields = new List<FieldReport>();
	}

	public int Length
	{
		get
		{
			return Fields.Count;
		}
	}

}

public class FieldReport
{
	public ICollection<string> Lines { get; private set; }
	public bool IsNumber { get; set; }

	public FieldReport()
	{
		Lines = new List<string>();
	}

	public FieldReport(string uniqueLine, bool number) :
		this()
	{
		Lines.Add(uniqueLine);
		IsNumber = number;
	}

	public FieldReport(string uniqueLine) :
		this(uniqueLine, false)
	{
	}

	public override string ToString()
	{
		return string.Join(" ", Lines.ToArray());
	}

}