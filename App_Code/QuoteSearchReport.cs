using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Admon;

public class QuoteSearchReport : SearchReportEngine
{
    public QuoteSearchReport(string agentes) :
        base(agentes)
    {

    }

    protected override IEnumerable<SearchResult> SearchById(string id)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var result = db.spList_SearchByQuoteID(int.Parse(id), m_agentes, ApplicationConstants.COMPANY).Single(); //solo puede haber uno con ese ID
            var fee = db.quotes.Where(m => m.id == int.Parse(id)).Select(m => m.fee).Single();
            HouseEstate estate = new HouseEstate(result.houseEstate);
            return new SearchResult[] {
                new SearchResult {
                    id = result.id,
                    city = estate.City.Name,
                    firstName = result.firstname,
                    lastName = result.lastName,
                    lastName2 = result.lastName2,
                    state = estate.City.State.Name,
                    street = result.street,
                    zipCode = estate.ZipCode,
                    interiorNo = result.InteriorNo,
                    exteriorNo = result.exteriorNo,
                    issueDate = result.issueDate.ToString(),
                    netPremium = result.netPremium ?? 0,
                    rxpf = result.recargoPagoFraccionado,
                    rxpfPercentage = result.porcentajeRecargoPagoFraccionado,
                    fee = ApplicationConstants.Fee(result.currency, fee.Value),
                    Currency = result.currency,
                    taxTotal = result.netPremium != null ?
                        ApplicationConstants.CalculateTax(
                            result.netPremium.Value,
                            result.taxPercentage.Value,
                            result.porcentajeRecargoPagoFraccionado,
                            result.currency,
                            fee.Value
                        ) : 0
                }
            };
        }
        catch
        {

        }
        return new SearchResult[] { };
    }

    protected override IEnumerable<SearchResult> SearchByName(string name)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var results = db.spList_SearchByClientName(name, m_agentes, ApplicationConstants.COMPANY);
            return results.Select(m =>
            {
                HouseEstate estate = new HouseEstate(m.houseEstate);
                return new SearchResult
                {
                    id = m.id,
                    city = estate.City.Name,
                    firstName = m.firstname,
                    lastName = m.lastName,
                    lastName2 = m.lastName2,
                    state = estate.City.State.Name,
                    street = m.street,
                    zipCode = estate.ZipCode,
                    interiorNo = m.InteriorNo,
                    exteriorNo = m.exteriorNo,
                    issueDate = m.issueDate.ToString(),
                    netPremium = m.netPremium ?? 0,
                    rxpf = m.recargoPagoFraccionado,
                    rxpfPercentage = m.porcentajeRecargoPagoFraccionado,
                    fee = ApplicationConstants.Fee(m.currency, db.quotes.Where(x => x.id == m.id).Select(x => x.fee).Single().Value),
                    Currency = m.currency,
                    taxTotal = m.netPremium != null ?
                        ApplicationConstants.CalculateTax(
                            m.netPremium.Value,
                            m.taxPercentage.Value,
                            m.porcentajeRecargoPagoFraccionado,
                            m.currency,
                            db.quotes.Where(x => x.id == m.id).Select(x => x.fee).Single().Value
                        ) : 0
                };
            });
        }
        catch
        {
        }
        return new SearchResult[] { };
    }

    protected override IEnumerable<SearchResult> SearchByDates(DateTime start, DateTime end)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var results = db.spList_SearchByIssueDate(start, end, m_agentes, ApplicationConstants.COMPANY);
            return results.Select(m =>
            {
                HouseEstate estate = new HouseEstate(m.houseEstate);
                return new SearchResult
                {
                    id = m.id,
                    city = estate.City.Name,
                    firstName = m.firstname,
                    lastName = m.lastName,
                    lastName2 = m.lastName2,
                    state = estate.City.State.Name,
                    street = m.street,
                    zipCode = estate.ZipCode,
                    interiorNo = m.InteriorNo,
                    exteriorNo = m.exteriorNo,
                    issueDate = m.issueDate.ToString(),
                    netPremium = m.netPremium ?? 0,
                    rxpf = m.recargoPagoFraccionado,
                    rxpfPercentage = m.porcentajeRecargoPagoFraccionado,
                    fee = ApplicationConstants.Fee(m.currency, db.quotes.Where(x => x.id == m.id).Select(x => x.fee).Single().Value),
                    Currency = m.currency,
                    taxTotal = m.netPremium != null ?
                        ApplicationConstants.CalculateTax(
                            m.netPremium.Value,
                            m.taxPercentage.Value,
                            m.porcentajeRecargoPagoFraccionado,
                            m.currency,
                            db.quotes.Where(x => x.id == m.id).Select(x => x.fee).Single().Value
                        ) : 0
                };
            });
        }
        catch
        {
        }
        return new SearchResult[] { };
    }

    protected override IEnumerable<SearchResult> SearchByAgent(string agent)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var results = db.spList_SearchByUser(int.Parse(agent), m_agentes, ApplicationConstants.COMPANY);
            return results.Select(m =>
            {
                HouseEstate estate = new HouseEstate(m.houseEstate);
                return new SearchResult
                {
                    id = m.id,
                    city = estate.City.Name,
                    firstName = m.firstname,
                    lastName = m.lastName,
                    lastName2 = m.lastName2,
                    state = estate.City.State.Name,
                    street = m.street,
                    zipCode = estate.ZipCode,
                    interiorNo = m.InteriorNo,
                    exteriorNo = m.exteriorNo,
                    issueDate = m.issueDate.ToString(),
                    netPremium = m.netPremium ?? 0,
                    rxpf = m.recargoPagoFraccionado,
                    rxpfPercentage = m.porcentajeRecargoPagoFraccionado,
                    fee = ApplicationConstants.Fee(m.currency, db.quotes.Where(x => x.id == m.id).Select(x => x.fee).Single().Value),
                    Currency = m.currency,
                    taxTotal = m.netPremium != null ?
                        ApplicationConstants.CalculateTax(
                            m.netPremium.Value,
                            m.taxPercentage.Value,
                            m.porcentajeRecargoPagoFraccionado,
                            m.currency,
                            db.quotes.Where(x => x.id == m.id).Select(x => x.fee).Single().Value
                        ) : 0
                };
            });
        }
        catch
        {
        }
        return new SearchResult[] { };
    }

    protected override IEnumerable<SearchResult> SearchByExpiring(DateTime start, DateTime end, bool includeRenewed)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<SearchResult> SearchLikePolicyNo(string policyNo)
    {
        throw new NotImplementedException();
    }
}
