using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Admon;

public class MapfreCoverageRow
{
    public decimal? Amount { get; set; }
    public int CoverageCode { get; set; }
}

public class MapfrePrimeRow
{
    public decimal? Prime { get; set; }
    public int CoverageCode { get; set; }
}

public class MapfreDeductibles
{
    public int FHM { get; private set; }
    public int TEV { get; private set; }

    public MapfreDeductibles(decimal fhm, decimal tev)
    {
        FHM = TransformData(fhm);
        TEV = TransformData(tev);
    }

    public MapfreDeductibles(int fhm, int tev)
    {
        FHM = TransformData(fhm);
        TEV = TransformData(tev);
    }

    private int TransformData(int value)
    {
        /* Mapfre necesita que se le sume 300 */
        if (value >= 0 && value <= 100) {
            return (int)value + 300;
        }
        return 0;
    }

    private int TransformData(decimal value)
    {
        /* Mapfre necesita que se le sume 300 */
        if (value >= 0 && value <= 100) {
            return (int)value + 300;
        } else if (value >= 0 && value <= 1) {
            return (int)(value * 100) + 300;
        }
        return 0;
    }
}

public class MapfreModelPolicy
{
    private HouseInsuranceDataContext m_dataContext;

    private int m_coverageQuoteCache, m_primeQuoteCache;
    private IEnumerable<MapfreCoverageRow> m_coverageCache;
    private IEnumerable<MapfrePrimeRow> m_primeCache;

	public MapfreModelPolicy(HouseInsuranceDataContext db)
	{
        m_dataContext = db;
        m_coverageQuoteCache = -1;
        m_coverageCache = null;
        m_primeCache = null;
        m_primeQuoteCache = -1;
	}

    public IEnumerable<MapfreCoverageRow> GetCoverageRows(int quoteID)
    {
        if (m_coverageQuoteCache == -1 || quoteID != m_coverageQuoteCache) {
            m_coverageQuoteCache = quoteID;
            m_coverageCache = m_dataContext.quoteDetails.Where(
                m => m.quote == quoteID && (
                        m.codigoCobertura == 3030 ||
                        m.codigoCobertura == 3020 ||
                        m.codigoCobertura == 3021 ||
                        m.codigoCobertura == 3070 ||
                        m.codigoCobertura == 3071 ||
                        m.codigoCobertura == 3231 ||
                        m.codigoCobertura == 3221 ||
                        m.codigoCobertura == 3222 ||
                        m.codigoCobertura == 3223 ||
                        m.codigoCobertura == 3211 ||
                        m.codigoCobertura == 3212 ||
                        m.codigoCobertura == 3213 ||
                        //m.codigoCobertura == 3400 || R.C. Inmuebles y actividades
                        m.codigoCobertura == 3401 ||
                        m.codigoCobertura == 3501 ||
                        m.codigoCobertura == 3502 ||
                        m.codigoCobertura == 3503 ||
                        m.codigoCobertura == 3504 ||
                        m.codigoCobertura == 3520 ||
                        m.codigoCobertura == 3550 ||
                        m.codigoCobertura == 3660
                    )
                )
                .Select(m => new MapfreCoverageRow {
                    Amount = m.prima == null || m.prima == 0 ? 0 : m.monto,
                    CoverageCode = m.codigoCobertura,
                }).ToList().Union(
                    m_dataContext.quoteDetails.Where(
                        m => m.quote == quoteID && m.prima != 0 && (
                            m.codigoCobertura == 3910 ||
                            m.codigoCobertura == 3911 ||
                            m.codigoCobertura == 3930 ||
                            m.codigoCobertura == 3920 ||
                            m.codigoCobertura == 3921 || // asistencia informatica (opcional a partir de nov 2011)
                            m.codigoCobertura == 3931 || // medico a domicilio (opcional)
                            m.codigoCobertura == 3932    // ambulancia a domicilio (opcional)
                        )
                    )
                    .Select(m => new MapfreCoverageRow {
                        Amount = 0,
                        CoverageCode = m.codigoCobertura
                    })
                )
                .OrderBy(m => m.CoverageCode);
        }
        return m_coverageCache;
    }

    public IEnumerable<MapfrePrimeRow> GetPrimeRows(int quoteID)
    {
        if (m_primeQuoteCache == -1 || quoteID != m_primeQuoteCache) {
            m_primeQuoteCache = quoteID;
            m_primeCache = m_dataContext.quoteDetails.Where(
                m => m.quote == quoteID && (
                        m.codigoCobertura == 3001 ||
                        m.codigoCobertura == 3002 ||
                        m.codigoCobertura == 3003 ||
                        m.codigoCobertura == 3004 ||
                        m.codigoCobertura == 3030 ||
                        m.codigoCobertura == 3021 ||
                        m.codigoCobertura == 3051 ||
                        m.codigoCobertura == 3052 ||
                        m.codigoCobertura == 3053 ||
                        m.codigoCobertura == 3054 ||
                        m.codigoCobertura == 3080 ||
                        m.codigoCobertura == 3070 ||
                        m.codigoCobertura == 3071 ||
                        m.codigoCobertura == 3231 ||
                        m.codigoCobertura == 3221 ||
                        m.codigoCobertura == 3222 ||
                        m.codigoCobertura == 3223 ||
                        m.codigoCobertura == 3211 ||
                        m.codigoCobertura == 3212 ||
                        m.codigoCobertura == 3213 ||
                        m.codigoCobertura == 3401 ||
                        m.codigoCobertura == 3441 ||
                        m.codigoCobertura == 3530 ||
                        m.codigoCobertura == 3501 ||
                        m.codigoCobertura == 3502 ||
                        m.codigoCobertura == 3503 ||
                        m.codigoCobertura == 3504 ||
                        m.codigoCobertura == 3520 ||
                        m.codigoCobertura == 3550 ||
                        m.codigoCobertura == 3660 ||
                        m.codigoCobertura == 3910 ||
                        m.codigoCobertura == 3911 ||
                        m.codigoCobertura == 3930 ||
                        m.codigoCobertura == 3920 ||
                        m.codigoCobertura == 3921 ||
                        m.codigoCobertura == 3931 ||
                        m.codigoCobertura == 3932
                    )
                ).Select(m => new MapfrePrimeRow {
                    Prime = m.prima,
                    CoverageCode = m.codigoCobertura
                }).ToList().Union(
                    new MapfrePrimeRow [] {
                        new MapfrePrimeRow {
                            Prime = m_dataContext.quoteDetails.Where(
                                m => m.quote == quoteID && m.prima != null && (
                                    m.codigoCobertura == 3020 ||
                                    m.codigoCobertura == 4000
                                )
                            ).Sum(m => m.prima),
                            CoverageCode = 3020
                        }
                    }
                )
                .OrderBy(m => m.CoverageCode);
        }
        return m_primeCache;
    }

    public MapfreDeductibles GetDeductiblesData(int quoteID)
    {
        City city = m_dataContext.quotes.Single(m => m.id == quoteID).insuredPersonHouse1.HouseEstate.City;
        return new MapfreDeductibles(city.DeductibleHMP.Value, city.DeductibleQuake.Value);
    }

}
