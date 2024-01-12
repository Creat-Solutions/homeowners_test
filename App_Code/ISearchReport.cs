using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Engine that gets the result of the reports
/// </summary>
public abstract class SearchReportEngine
{
    protected string m_agentes = null;

    public SearchReportEngine(string agentes)
    {
        m_agentes = agentes;
    }

    public IEnumerable<SearchResult> SearchExpiring(
        string startDate,
        string endDate,
        bool includeRenewed
    )
    {
        DateTime? start, end;
        if (string.IsNullOrEmpty(startDate)) {
            start = null;
        } else {
            try {
                start = DateTime.Parse(startDate);
            } catch {
                start = null;
            }
        }
        if (string.IsNullOrEmpty(endDate)) {
            end = start;
        } else {
            try {
                end = DateTime.Parse(endDate);
            } catch {
                end = null;
            }
        }
        if (start == null) {
            return null;
        }
        return SearchByExpiring(start.Value, end.Value, includeRenewed);
    }

    public IEnumerable<SearchResult> Search(
        string id,
        string name,
        string startDate,
        string endDate,
        string agent
    )
    {
        DateTime? start, end;
        if (string.IsNullOrEmpty(startDate)) {
            start = null;
        } else {
            try {
                start = DateTime.Parse(startDate);
            } catch {
                start = null;
            }
        }
        if (string.IsNullOrEmpty(endDate)) {
            end = null;
        } else {
            try {
                end = DateTime.Parse(endDate);
            } catch {
                end = null;
            }
        }

        if (!string.IsNullOrEmpty(id)) {
            if (name.Equals("ENDORSMENT"))
                return SearchLikePolicyNo(id);
            else
                return SearchById(id);
        } else if (!string.IsNullOrEmpty(name)) {
            return SearchByName(name);
        } else if (start != null) {
            if (end == null) {
                end = start;
            }
            return SearchByDates(start.Value, end.Value);
        } else if (!string.IsNullOrEmpty(agent)) {
            return SearchByAgent(agent);
        }
        return null;
    }

    protected abstract IEnumerable<SearchResult> SearchById(string id);
    protected abstract IEnumerable<SearchResult> SearchLikePolicyNo(string policyNo);
    protected abstract IEnumerable<SearchResult> SearchByName(string name);
    protected abstract IEnumerable<SearchResult> SearchByDates(DateTime start, DateTime end);
    protected abstract IEnumerable<SearchResult> SearchByAgent(string agent);
    protected abstract IEnumerable<SearchResult> SearchByExpiring(DateTime start, DateTime end, bool includeRenewed);
}

