using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Admon;

public class PolicySearchReport : SearchReportEngine
{
    private bool m_test;

    public PolicySearchReport(string agentes, bool test) :
        base(agentes)
    {
        m_test = test;
    }

    protected override IEnumerable<SearchResult> SearchById(string id)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var result = db.spList_SearchByPolicyNo(id, m_agentes, ApplicationConstants.COMPANY).Single(); //solo puede haber uno con ese ID
            HouseEstate estate = result.houseEstate == 0 ? null : new HouseEstate(result.houseEstate);
            return new SearchResult[] {
                new SearchResult {
                    id = result.id,
                    RFC = result.rfc,
                    policyNo = result.policyNo,
                    city = estate == null ? "" : estate.City.Name,
                    firstName = result.firstname,
                    lastName = result.lastName,
                    lastName2 = result.lastName2,
                    state = estate == null ? "" : estate.City.State.Name,
                    street = result.street,
                    zipCode = estate == null ? "" : estate.ZipCode,
                    interiorNo = result.InteriorNo,
                    exteriorNo = result.exteriorNo,
                    issueDate = result.issueDate.ToString(),
                    netPremium = result.netPremium,
                    fee = result.fee,
                    taxTotal = result.taxTotal
                }
            };
        }
        catch
        {

        }
        return new SearchResult[] { };
    }

    protected override IEnumerable<SearchResult> SearchLikePolicyNo(string policyNo)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var results = db.spList_SearchByPolicyNo(policyNo, m_agentes, ApplicationConstants.COMPANY);
            return results.Select(m =>
            {
                HouseEstate estate = m.houseEstate == 0 ? null : new HouseEstate(m.houseEstate);
                return new SearchResult
                {
                    id = m.id,
                    RFC = m.rfc,
                    city = estate == null ? "" : estate.City.Name,
                    policyNo = m.policyNo,
                    exteriorNo = m.exteriorNo,
                    interiorNo = m.InteriorNo,
                    firstName = m.firstname,
                    lastName = m.lastName,
                    lastName2 = m.lastName2,
                    issueDate = m.issueDate.ToString(),
                    state = estate == null ? "" : estate.City.State.Name,
                    street = m.street,
                    zipCode = estate == null ? "" : estate.ZipCode,
                    netPremium = m.netPremium,
                    fee = m.fee,
                    taxTotal = m.taxTotal,
                    PolicyNoRenewal = "ENDORSMENT"
                };
            });
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
            var results = db.spList_SearchPolicyByClientName(name, m_agentes, ApplicationConstants.COMPANY);
            return results.Select(m =>
            {
                HouseEstate estate = m.houseEstate == 0 ? null : new HouseEstate(m.houseEstate);
                return new SearchResult
                {
                    id = m.id,
                    RFC = m.rfc,
                    city = estate == null ? "" : estate.City.Name,
                    policyNo = m.policyNo,
                    exteriorNo = m.exteriorNo,
                    interiorNo = m.InteriorNo,
                    firstName = m.firstname,
                    lastName = m.lastName,
                    lastName2 = m.lastName2,
                    issueDate = m.issueDate.ToString(),
                    state = estate == null ? "" : estate.City.State.Name,
                    street = m.street,
                    zipCode = estate == null ? "" : estate.ZipCode,
                    netPremium = m.netPremium,
                    fee = m.fee,
                    taxTotal = m.taxTotal
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
            var results = db.spList_SearchPolicyByIssueDate(start, end, m_agentes, ApplicationConstants.COMPANY);
            return results.Select(m =>
            {
                HouseEstate estate = m.houseEstate == 0 ? null : new HouseEstate(m.houseEstate);
                return new SearchResult
                {
                    id = m.id,
                    RFC = m.rfc,
                    city = estate == null ? "" : estate.City.Name,
                    policyNo = m.policyNo,
                    exteriorNo = m.exteriorNo,
                    interiorNo = m.InteriorNo,
                    firstName = m.firstname,
                    lastName = m.lastName,
                    lastName2 = m.lastName2,
                    issueDate = m.issueDate.ToString(),
                    state = estate == null ? "" : estate.City.State.Name,
                    street = m.street,
                    zipCode = estate == null ? "" : estate.ZipCode,
                    netPremium = m.netPremium,
                    fee = m.fee,
                    taxTotal = m.taxTotal
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
            var results = db.spList_SearchPolicyByUser(int.Parse(agent), m_agentes, ApplicationConstants.COMPANY);
            return results.Select(m =>
            {
                HouseEstate estate = m.houseEstate == 0 ? null : new HouseEstate(m.houseEstate);
                return new SearchResult
                {
                    id = m.id,
                    RFC = m.rfc,
                    city = estate == null ? "" : estate.City.Name,
                    policyNo = m.policyNo,
                    exteriorNo = m.exteriorNo,
                    interiorNo = m.InteriorNo,
                    firstName = m.firstname,
                    lastName = m.lastName,
                    lastName2 = m.lastName2,
                    issueDate = m.issueDate.ToString(),
                    state = estate == null ? "" : estate.City.State.Name,
                    street = m.street,
                    zipCode = estate == null ? "" : estate.ZipCode,
                    netPremium = m.netPremium,
                    fee = m.fee,
                    taxTotal = m.taxTotal
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
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var results = db.spList_SearchPolicyByExpiringDate(start, end, m_agentes, ApplicationConstants.COMPANY, m_test, includeRenewed);
            return results.Select(m =>
            {
                HouseEstate estate = m.houseEstate == 0 ? null : new HouseEstate(m.houseEstate);
                return new SearchResult
                {
                    id = m.id,
                    city = estate == null ? "" : estate.City.Name,
                    RFC = m.rfc,
                    policyNo = m.policyNo,
                    exteriorNo = m.exteriorNo,
                    interiorNo = m.InteriorNo,
                    firstName = m.firstname,
                    lastName = m.lastName,
                    lastName2 = m.lastName2,
                    issueDate = m.issueDate.ToString(),
                    state = estate == null ? "" : estate.City.State.Name,
                    street = m.street,
                    zipCode = estate == null ? "" : estate.ZipCode,
                    netPremium = m.netPremium,
                    fee = m.fee,
                    taxTotal = m.taxTotal,
                    endDate = m.endDate.ToString(),
                    PolicyNoRenewal = m.policyRenewal ?? "-NOT RENEWED-"
                };
            });
        }
        catch
        {
        }
        return new SearchResult[] { };

    }
}
