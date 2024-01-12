using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.IO;
using System.Globalization;
using System.Text;

/// <summary>
/// Obtener el xml del archivo default
/// </summary>
public class MapfrePolicyXMLGenerator
{
    private const string XMLCACHEKEY = "xmlCache";
    private const string XMLACCESSKEY = "xmlAccessCache";
    private const string DEFAULTPOLIZAGENERATORFILE = "~/PolizaGenerator.xml";
    private const string XMLSIGNATURE = "1234XMLSIGN";
    private int m_meagent;
    private string m_cuadroCom = "191";
    private string m_comEmisora = "1169";
    private string m_nombreRiesgo = "Riesgo No. 1 (temporal)";

    private string m_filename;
    private quote m_data;
    private bool m_isTest;
    private HouseInsuranceDataContext m_connection;

    private class RFCData
    {
        public string RFCInsured { get; set; }
        public string RFCBank { get; set; }
        public string RFCAdditional { get; set; }

        public string[] ToArray()
        {
            List<string> packed = new List<string>();
            packed.Add(RFCInsured);
            if (!string.IsNullOrEmpty(RFCBank))
            {
                packed.Add(RFCBank);
            }

            if (!string.IsNullOrEmpty(RFCAdditional))
            {
                packed.Add(RFCAdditional);
            }

            return packed.ToArray();
        }
    }

    private int GetCurrency()
    {
        return m_data.currency;
    }

    public MapfrePolicyXMLGenerator(HouseInsuranceDataContext db, quote data, bool test, string filename)
    {
        m_filename = filename;
        m_data = data;
        m_connection = db;
        m_isTest = test;
        m_meagent = test ? ApplicationConstants.MAPFREMEAGENTTEST : ApplicationConstants.MAPFREMEAGENT;
    }

    public MapfrePolicyXMLGenerator(HouseInsuranceDataContext db, quote data, bool test)
        : this(db, data, test, DEFAULTPOLIZAGENERATORFILE)
    {
    }

    private string GetGenericXml()
    {
        Cache currentCache = HttpRuntime.Cache;

        string filename = HostingEnvironment.MapPath(m_filename);
        long modificationTime = new FileInfo(filename).LastWriteTime.Ticks;

        /* todo: test para velocidad */
        if (currentCache[XMLCACHEKEY] == null || currentCache[XMLACCESSKEY] == null ||
            modificationTime > (long)currentCache[XMLACCESSKEY] ||
            ((string)currentCache[XMLCACHEKEY]).Substring(0, XMLSIGNATURE.Length) != XMLSIGNATURE)
        {
            currentCache[XMLACCESSKEY] = modificationTime;
            StreamReader reader = File.OpenText(filename);
            StringBuilder builder = new StringBuilder(XMLSIGNATURE);
            string line = reader.ReadLine();
            while (line != null)
            {
                builder.Append(line.Trim());
                line = reader.ReadLine();
            }
            currentCache[XMLCACHEKEY] = builder.ToString();
        }
        return ((string)currentCache[XMLCACHEKEY]).Substring(XMLSIGNATURE.Length);
    }

    private string varRow = "<ROW num=\"{0}\"><COD_CAMPO>{1}</COD_CAMPO>" +
        "<VAL_CAMPO>{2}</VAL_CAMPO><VAL_COR_CAMPO>{3}</VAL_COR_CAMPO><COD_RAMO>310</COD_RAMO><NUM_RIESGO>{4}</NUM_RIESGO>" +
        "<TIP_NIVEL>{5}</TIP_NIVEL><COD_CIA>1</COD_CIA><NUM_POLIZA></NUM_POLIZA><NUM_SPTO>0</NUM_SPTO><NUM_APLI>0</NUM_APLI>" +
        "<NUM_SPTO_APLI>0</NUM_SPTO_APLI><NUM_PERIODO>1</NUM_PERIODO><MCA_BAJA_RIESGO>N</MCA_BAJA_RIESGO>" +
        "<MCA_VIGENTE>S</MCA_VIGENTE><MCA_VIGENTE_APLI>S</MCA_VIGENTE_APLI></ROW>";

    private string GetSecurityRows(quote data, int rowNumber)
    {
        string result = string.Empty;

        /*
         * 10   GUARDIAS 24 HORAS
         * 11   ALARMA CENTRAL
         * 12   ALARMA LOCAL
         * 13   CIRCUITO DE TV
         * 14   VIDEO PORTERO
         */
        bool security = false;
        security = security || (data.guards24 ?? false);
        security = security || (data.centralAlarm ?? false);
        security = security || (data.localAlarm ?? false);
        security = security || (data.tvCircuit ?? false);
        security = security || (data.tvGuard ?? false);

        if (security)
        {
            result += string.Format(varRow, rowNumber, "COD_MEDIDA_SEG_ROBO", 10, 0, 1, 1);
            rowNumber++;
        } /*else {
            result += string.Format(varRow, rowNumber, "COD_MEDIDA_SEG_ROBO", 99999, 0, 1, 1);
            rowNumber++;
        }*/

        return result;
    }

    private string GetVariableRows(quote data)
    {
        int rowNumber = 18;
        string result = string.Empty;

        if (data.descuento > 0)
        {
            result += string.Format(varRow, rowNumber, "COD_DESCUENTO", 20, 0, 0, 1); //20 = 10%
            rowNumber++;
        }

        /* La cobertura de Equipo Electronico cuando se contrata esta, lleva un dato variable llamado 
         * IMP_FRANQUICIA_EE a nivel de riesgo con valor en moneda nacional de 1,500 y de 150 en dolares. */
        if (data.electronicEquipmentLimit > 0)
        {
            int cantidad = 150;
            if (data.currency == ApplicationConstants.PESOS_CURRENCY)
            {
                cantidad = 1500;
            }
            result += string.Format(varRow, rowNumber, "IMP_FRANQUICIA_EE", cantidad, 0, 0, 1);
            rowNumber++;
        }

        if (data.outdoorLimit > 0)
        {
            string outdoorLimitStr = data.outdoorLimit.GetFormattedNumber();
            result += string.Format(varRow, rowNumber, "IMP_LIM_INSTALACION_FIJA", outdoorLimitStr, outdoorLimitStr, 1, 3);
            rowNumber++;
        }

        if (data.fireAll ?? false)
        {
            result += string.Format(varRow, rowNumber, "COD_MEDIDA_SEG_INCENDIO", 99999, 0, 1, 1);
            rowNumber++;
        }

        result += GetSecurityRows(data, rowNumber);

        return result;

    }

    private string row2000060 = "<ROW num=\"{0}\"><TIP_BENEF>{1}</TIP_BENEF><NUM_RIESGO>{2}</NUM_RIESGO><NUM_SECU>1</NUM_SECU>" +
        "<TIP_DOCUM>RFC</TIP_DOCUM><COD_DOCUM>{3}</COD_DOCUM><COD_CIA>1</COD_CIA><NUM_POLIZA></NUM_POLIZA>" +
        "<NUM_SPTO>0</NUM_SPTO><NUM_APLI>0</NUM_APLI><NUM_SPTO_APLI>0</NUM_SPTO_APLI><MCA_PRINCIPAL>N</MCA_PRINCIPAL>" +
        "<MCA_CALCULO>N</MCA_CALCULO><MCA_BAJA>N</MCA_BAJA><MCA_VIGENTE>S</MCA_VIGENTE><PCT_PARTICIPACION>0</PCT_PARTICIPACION>" +
        "<FEC_VCTO_CESION>{4}</FEC_VCTO_CESION><IMP_CESION>0</IMP_CESION></ROW>";

    private string GetRowsTerceros(string fecVcto, string[] rfcs)
    {
        string rowsResult = string.Empty;
        if (rfcs.Length > 1)
        {
            for (int i = rfcs.Length - 1; i > 0; i--)
            {
                rowsResult += string.Format(row2000060, 1, 1, 0, rfcs[i], fecVcto);
            }
        }
        rowsResult += string.Format(row2000060, rfcs.Length > 1 ? 2 : 1, 4, 1, rfcs[0], fecVcto);
        return rowsResult;
    }

    private const string primaRow = "<ROW num=\"{0}\"><COD_CIA>1</COD_CIA><NUM_POLIZA></NUM_POLIZA><NUM_SPTO>0</NUM_SPTO>" +
        "<NUM_APLI>0</NUM_APLI><NUM_SPTO_APLI>0</NUM_SPTO_APLI><NUM_RIESGO>{1}</NUM_RIESGO><NUM_PERIODO>1</NUM_PERIODO>" +
        "<COD_COB>{2}</COD_COB><COD_DESGLOSE>{3}</COD_DESGLOSE><COD_ECO>{4}</COD_ECO><NUM_BLOQUE_ESTUDIO>0</NUM_BLOQUE_ESTUDIO>" +
        "<IMP_ACUMULADO_ANUAL>0</IMP_ACUMULADO_ANUAL><IMP_SPTO>{5}</IMP_SPTO><IMP_NO_CONSUMIDO>0</IMP_NO_CONSUMIDO>" +
        "<IMP_ANUAL>0</IMP_ANUAL><COD_RAMO>310</COD_RAMO></ROW>";

    private string GetPrimasRows(quote data, MapfreModelPolicy dataCoverages)
    {
        string result = "<TABLE NAME=\"P2100170\"><ROWSET>";
        int rowNumber = 1;

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var primas = dataCoverages.GetPrimeRows(data.id);
        string cod_eco = "1", cod_desglose = "1", cod_riesgo = "1";

        bool row3423Set = false;
        var buildingsLimit = data.buildingLimit + data.otherStructuresLimit;

        foreach (var p in primas)
        {

            if (p.CoverageCode > 3500 && buildingsLimit == 0 && !row3423Set)
            {
                result += string.Format(primaRow, rowNumber, cod_riesgo, 3423, cod_desglose, cod_eco, 1);
                row3423Set = true;
                rowNumber++;
            }

            /*Las coberturas 3503 Joyas y 3504 Gastos Medicos son dependientes de la cobertura 3500, por lo que si no se contrata la cobertura 3500 no se envian estas */
            if ((p.CoverageCode == 3503 || p.CoverageCode == 3504) && data.burglaryLimit == 0)
            {
                continue;
            }

            if (p.Prime == null || p.Prime == 0)
            {
                continue;
            }
            result += string.Format(primaRow, rowNumber, cod_riesgo, p.CoverageCode, cod_desglose, cod_eco, ((decimal)p.Prime).GetFormattedNumber());
            rowNumber++;
        }

        if (buildingsLimit == 0 && !row3423Set)
        {
            result += string.Format(primaRow, rowNumber, cod_riesgo, 3423, cod_desglose, cod_eco, 1);
            row3423Set = true;
            rowNumber++;
        }

        //fee
        result += string.Format(primaRow, rowNumber, 0, 3990, 1, 1, ApplicationConstants.Fee(data.currency, data.fee.Value).GetFormattedNumber());
        result += "</ROWSET></TABLE>";
        return result;
    }

    private void RegulateNameLength(ref string data)
    {
        if (data.Length > ApplicationConstants.MAPFRENAMEMAXLENGTH)
        {
            data = data.Substring(0, 30);
        }
    }

    private string row1001331 = "<ROW num=\"{0}\"><TIP_DOCUM>RFC</TIP_DOCUM><NUM_SECU>1</NUM_SECU><FEC_TRATAMIENTO>" +
        "{1}</FEC_TRATAMIENTO><NOM_LOCALIDAD>{2}</NOM_LOCALIDAD><TLF_NUMERO>{3}</TLF_NUMERO><NOM_DOMICILIO1_COM>" +
        "{4}</NOM_DOMICILIO1_COM><TXT_AUX3>DJEIT</TXT_AUX3><TXT_AUX2>ALGUNA</TXT_AUX2><NUM_RIESGO>1</NUM_RIESGO>" +
        "<MCA_SEXO>{5}</MCA_SEXO><APE2_TERCERO>{6}</APE2_TERCERO><COD_ESTADO>{7}</COD_ESTADO><COD_PROV>{8}</COD_PROV>" +
        "<NOM_TERCERO>{9}</NOM_TERCERO><COD_POSTAL>{10}</COD_POSTAL><EMAIL>{11}</EMAIL><COD_PAIS>{12}</COD_PAIS>" +
        "<FEC_NACIMIENTO>{13}</FEC_NACIMIENTO><NOM_DOMICILIO2>{14}</NOM_DOMICILIO2><APE1_TERCERO>{15}</APE1_TERCERO>" +
        "<NOM_DOMICILIO1>{16}</NOM_DOMICILIO1><COD_DOCUM>{17}</COD_DOCUM><MCA_FISICO>{18}</MCA_FISICO><COD_CIA>1</COD_CIA>" +
        "<TIP_MVTO_BATCH>3</TIP_MVTO_BATCH><COD_ACT_TERCERO>1</COD_ACT_TERCERO><COD_IDIOMA>{19}</COD_IDIOMA>" +
        "<TXT_AUX1>NR</TXT_AUX1><NOM_DOMICILIO3>{20}</NOM_DOMICILIO3></ROW>";

    private string GetRowsDatosTerceros(string today, quote data, RFCData rfcs, string codEstadoDefault, string codPoblacionDefault)
    {
        bool mismadireccion = string.IsNullOrEmpty(data.insuredPerson1.street);
        string aPaterno = string.Empty;
        string aMaterno = string.Empty;
        if (!data.insuredPerson1.legalPerson)
        {
            string lastName = data.insuredPerson1.lastName.Trim();
            string lastName2 = string.IsNullOrEmpty(data.insuredPerson1.lastName2) ? string.Empty : data.insuredPerson1.lastName2.Trim();

            if (!string.IsNullOrEmpty(lastName2))
            {
                aPaterno = lastName.ToUpper();
                aMaterno = lastName2.ToUpper();
            }
            else
            {
                int aindex = lastName.IndexOf(' ');
                if (aindex > 0)
                {
                    aPaterno = data.insuredPerson1.lastName.Substring(0, aindex).ToUpper();
                    aMaterno = data.insuredPerson1.lastName.Substring(aindex + 1).ToUpper();
                }
                else
                    aPaterno = data.insuredPerson1.lastName.ToUpper();
            }

        }

        string rfcnumber = rfcs.RFCInsured, rfcAdicional = null, rfcBank = null;
        if (!string.IsNullOrEmpty(rfcs.RFCBank))
        {
            rfcBank = rfcs.RFCBank;
        }

        if (!string.IsNullOrEmpty(rfcs.RFCAdditional))
        {
            rfcAdicional = rfcs.RFCAdditional;
        }

        string addPaterno = string.Empty;
        string addMaterno = string.Empty;
        string addNombre = string.Empty;

        if (!string.IsNullOrEmpty(data.insuredPerson1.additionalInsured))
        {
            if (!data.insuredPerson1.legalPerson)
            {
                string[] namesplit = data.insuredPerson1.additionalInsured.Split(' ');
                addNombre = namesplit[0].ToUpper();

                if (namesplit.Length >= 3)
                {
                    addPaterno = namesplit[1];
                    if (namesplit.Length > 3)
                    {
                        string[] restOfData = new string[namesplit.Length - 2];
                        Array.Copy(namesplit, 2, restOfData, 0, namesplit.Length - 2);
                        addMaterno = string.Join(" ", restOfData).ToUpper();
                    }
                    else
                    {
                        addMaterno = namesplit[2].ToUpper();
                    }
                }
                else if (namesplit.Length == 2)
                {
                    addPaterno = namesplit[1].ToUpper();
                }
            }
            else
            {
                addNombre = data.insuredPerson1.additionalInsured.ToUpper();
            }
        }

        string nombre = data.insuredPerson1.firstname.ToUpper();
        string email = data.insuredPerson1.email ?? string.Empty;
        string personaFisica = MapfreRelations.PersonaFisica(!data.insuredPerson1.legalPerson);
        string fechaNacimiento = string.Empty;
        string nombreLocalidad = string.Empty;
        string telefono = data.insuredPerson1.phone != null ? data.insuredPerson1.phone.Trim() : "000000000";
        string nom_domicilio1_com = "ALGUNO";
        string idioma = "ES";

        string nombreCalle = data.insuredPersonHouse1.street;
        string numExterior = data.insuredPersonHouse1.exteriorNo;
        string numInterior = data.insuredPersonHouse1.InteriorNo != null ? " int " + data.insuredPersonHouse1.InteriorNo : string.Empty;
        string codPais = "MEX";

        string codPoblacion = codPoblacionDefault;
        //string codPostal = data.insuredPersonHouse1.ZipCode;
        string codPostal = data.insuredPersonHouse1.ZipCode;
        string codEstado = codEstadoDefault;
        string sexo = "1"; //MapfreRelations.Sexo((int)data.insuredPerson1.sex);




        string nomDomicilio;
        string nomDomicilio2;
        string nomDomicilio3 = string.Empty;

        /* always send mexican address */
        nomDomicilio = nombreCalle + " " + numExterior + numInterior;
        nomDomicilio2 = data.insuredPersonHouse1.HouseEstate.Name + ", " + data.insuredPersonHouse1.HouseEstate.City.Name;
        nombreLocalidad = data.insuredPersonHouse1.HouseEstate.City.Name;

        if (nomDomicilio.Length > 40)
        {
            nomDomicilio = nomDomicilio.Substring(0, 40);
        }
        if (nomDomicilio2.Length > 80)
        {
            nomDomicilio3 = nomDomicilio2.Substring(40, 40);
            nomDomicilio2 = nomDomicilio2.Substring(0, 40);
        }
        else if (nomDomicilio2.Length > 40)
        {
            int indexSeparator = nomDomicilio2.IndexOf(", ");
            if (indexSeparator == -1 || indexSeparator >= 40)
            {
                nomDomicilio3 = nomDomicilio2.Substring(40);
                nomDomicilio2 = nomDomicilio2.Substring(0, 40);
            }
            else
            {
                nomDomicilio3 = nomDomicilio2.Substring(indexSeparator + 2);
                nomDomicilio2 = nomDomicilio2.Substring(0, indexSeparator);
            }
        }

        string bankName = data.insuredPersonHouse1.bankName ?? string.Empty;
        string bankAddress = data.insuredPersonHouse1.bankAddress ?? string.Empty;

        if (bankAddress.Length > 40)
        {
            bankAddress = bankAddress.Substring(0, 40);
        }

        RegulateNameLength(ref nombre);
        RegulateNameLength(ref aPaterno);
        RegulateNameLength(ref aMaterno);
        RegulateNameLength(ref addNombre);
        RegulateNameLength(ref addPaterno);
        RegulateNameLength(ref addMaterno);
        RegulateNameLength(ref bankName);
        RegulateNameLength(ref nombreLocalidad);

        string resultRows = string.Empty;
        int rownumber = 1;

        if (!string.IsNullOrEmpty(rfcBank))
        {
            resultRows += string.Format(row1001331, rownumber, today, nombreLocalidad, telefono, nom_domicilio1_com, sexo, string.Empty,
                                        codEstado, codPoblacion, bankName, codPostal, email, codPais, fechaNacimiento, string.Empty,
                                        string.Empty, bankAddress, rfcBank, MapfreRelations.PersonaFisica(false), idioma, string.Empty);
            rownumber++;
        }

        if (!string.IsNullOrEmpty(rfcAdicional))
        {
            resultRows += string.Format(row1001331, rownumber, today, nombreLocalidad, telefono, nom_domicilio1_com, sexo, addMaterno,
                                        codEstado, codPoblacion, addNombre, codPostal, email, codPais, fechaNacimiento, nomDomicilio2,
                                        addPaterno, nomDomicilio, rfcAdicional, personaFisica, idioma, nomDomicilio3);
            rownumber++;
        }

        resultRows += string.Format(row1001331, rownumber, today, nombreLocalidad, telefono, nom_domicilio1_com, sexo, aMaterno,
                                    codEstado, codPoblacion, nombre, codPostal, email, codPais, fechaNacimiento, nomDomicilio2,
                                    aPaterno, nomDomicilio, rfcnumber, personaFisica, idioma, nomDomicilio3);
        return resultRows;
    }

    private const string coberturaRow = "<ROW num=\"{0}\"><COD_COB>{1}</COD_COB><SUMA_ASEG>{2}</SUMA_ASEG>" +
        "<SUMA_ASEG_SPTO>{2}</SUMA_ASEG_SPTO><COD_FRANQUICIA>{3}</COD_FRANQUICIA><COD_RAMO>310</COD_RAMO><COD_CIA>1</COD_CIA>" +
        "<NUM_POLIZA></NUM_POLIZA><NUM_SPTO>0</NUM_SPTO><NUM_APLI>0</NUM_APLI><NUM_SPTO_APLI>0</NUM_SPTO_APLI>" +
        "<NUM_RIESGO>1</NUM_RIESGO><NUM_PERIODO>1</NUM_PERIODO><NUM_SECU>0</NUM_SECU><COD_MON_CAPITAL>{4}</COD_MON_CAPITAL>" +
        "<TASA_COB>0</TASA_COB><MCA_BAJA_RIESGO>N</MCA_BAJA_RIESGO><MCA_VIGENTE>S</MCA_VIGENTE><MCA_VIGENTE_APLI>S</MCA_VIGENTE_APLI>" +
        "<MCA_BAJA_COB>N</MCA_BAJA_COB><COD_SECC_REAS>0</COD_SECC_REAS><IMP_AGR_SPTO>0</IMP_AGR_SPTO>" +
        "<IMP_AGR_REL_SPTO>0</IMP_AGR_REL_SPTO></ROW>";

    private bool CanSkipCobertura(int num)
    {
        return num == 3020 || num == 3021 || num == 3070 ||
               num == 3071 || num == 3222 || num == 3223 ||
               num == 3212 || num == 3213 || num == 3400;
    }

    private int GetDeductible(int num, MapfreDeductibles data)
    {
        switch (num)
        {
            /* FHM */
            case 3020:
            case 3070:
                return data.FHM;

            /* TEV */
            case 3021:
            case 3071:
                return data.TEV;

            default:
                return 0;
        }
    }

    private string GetCoberturasRows(quote data, MapfreModelPolicy dataCoverages)
    {
        string result = "<TABLE NAME=\"P2000040\"><ROWSET>";
        int rowNumber = 1;
        bool row3441Set = false;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int[] coberturasFijas = { 3000, 3050, 3080, 3230, 3220, 3210, 3530, 3500 };


        int cod_money = GetCurrency();

        decimal[] montosFijos = {
            data.buildingLimit + data.otherStructuresLimit,
            data.contentsLimit,
            data.contentsLimit,
            data.lossOfRentsLimit,
            data.extraLimit,
            data.debriLimit,
            data.accidentalGlassBreakageLimit,
            data.burglaryLimit
        };

        for (int i = 0; i < montosFijos.Length; i++)
        {
            /*
            if (montosFijos[i] == 0 && CanSkipCobertura(coberturasFijas[i])) {
                continue;
            }
            */
            result += string.Format(coberturaRow, rowNumber, coberturasFijas[i], montosFijos[i].GetFormattedNumber(), 0, cod_money);
            rowNumber++;
        }

        var coberturas = dataCoverages.GetCoverageRows(data.id);

        int cantidad3441 = cod_money == ApplicationConstants.PESOS_CURRENCY ? 28500 : 2850;
        /* La cobertura de R.C. Arrendatario se debe incluir cuando la cobertura de edificio este excluida. */
        var buildingsLimit = data.buildingLimit + data.otherStructuresLimit;
        int cantidad3423 = buildingsLimit == 0 ? 1 : 0;
        bool row3423Set = false;
        MapfreDeductibles deduciblesInfo = dataCoverages.GetDeductiblesData(data.id);

        foreach (var c in coberturas)
        {

            if (c.CoverageCode > 3500 && !row3423Set)
            {
                result += string.Format(coberturaRow, rowNumber, 3423, cantidad3423, 0, cod_money);
                row3423Set = true;
                rowNumber++;
            }

            if (c.CoverageCode > 3500 && !row3441Set)
            {
                result += string.Format(coberturaRow, rowNumber, 3441, cantidad3441, 0, cod_money);
                row3441Set = true;
                rowNumber++;
            }

            if ((c.Amount == null || c.Amount == 0) && CanSkipCobertura(c.CoverageCode))
            {
                continue;
            }

            decimal amount = (decimal)c.Amount;

            result += string.Format(
                coberturaRow,
                rowNumber,
                c.CoverageCode,
                amount.GetFormattedNumber(),
                GetDeductible(c.CoverageCode, deduciblesInfo),
                cod_money
            );
            rowNumber++;
        }

        if (!row3423Set)
        {
            result += string.Format(coberturaRow, rowNumber, 3423, cantidad3423, 0, cod_money);
            rowNumber++;
        }

        if (!row3441Set)
        {
            result += string.Format(coberturaRow, rowNumber, 3441, cantidad3441, 0, cod_money);
            rowNumber++;
        }

        //agregar 3940 excluyendole
        result += string.Format(coberturaRow, rowNumber, 3940, 0, 0, cod_money);

        result += "</ROWSET></TABLE>";
        return result;
    }


    private string FillXmlDocument(string genericxml, MapfreModelPolicy dataCoverages, bool generateDates)
    {
        string aPaterno = string.Empty;
        string aMaterno = string.Empty;
        if (!m_data.insuredPerson1.legalPerson)
        {
            string lastName = m_data.insuredPerson1.lastName.Trim();
            string lastName2 = string.IsNullOrEmpty(m_data.insuredPerson1.lastName2) ? string.Empty : m_data.insuredPerson1.lastName2.Trim();

            if (!string.IsNullOrEmpty(lastName2))
            {
                aPaterno = lastName.ToUpper();
                aMaterno = lastName2.ToUpper();
            }
            else
            {
                int aindex = lastName.IndexOf(' ');
                if (aindex > 0)
                {
                    aPaterno = m_data.insuredPerson1.lastName.Substring(0, aindex).ToUpper();
                    aMaterno = m_data.insuredPerson1.lastName.Substring(aindex + 1).ToUpper();
                }
                else
                    aPaterno = m_data.insuredPerson1.lastName.ToUpper();
            }

        }
        string rfcnumber;
        RFCData rfcs = new RFCData();
        int rfcIndex = 0;
        if (!string.IsNullOrEmpty(m_data.insuredPerson1.rfc))
        {
            rfcnumber = m_data.insuredPerson1.rfc.Trim().ToUpper();
        }
        else
        {
            rfcnumber = MapfreRelations.GenerateRFC(aPaterno, aMaterno, m_data.insuredPerson1.firstname, m_data.insuredPerson1.legalPerson, rfcIndex);
            rfcIndex++;
        }

        rfcs.RFCInsured = rfcnumber;

        if (!string.IsNullOrEmpty(m_data.insuredPerson1.additionalInsured))
        {
            string[] namesplit = m_data.insuredPerson1.additionalInsured.Split(' ');
            string addNombre, addPaterno = null, addMaterno = null, rfcAdicional;
            addNombre = namesplit[0].ToUpper();
            if (namesplit.Length >= 3)
            {
                addPaterno = namesplit[2].ToUpper();
                addMaterno = namesplit[1].ToUpper();
            }
            else if (namesplit.Length == 2)
            {
                addPaterno = namesplit[1].ToUpper();
            }
            rfcAdicional = MapfreRelations.GenerateRFC(addPaterno, addMaterno, addNombre, m_data.insuredPerson1.legalPerson, rfcIndex);
            rfcIndex++;

            rfcs.RFCAdditional = rfcAdicional;
        }

        if (!string.IsNullOrEmpty(m_data.insuredPersonHouse1.bankName))
        {
            rfcs.RFCBank = MapfreRelations.GenerateRFC(null, null, null, true, rfcIndex);
        }

        string codArea = MapfreRelations.TipoPropiedadToCodArea((int)m_data.typeOfProperty, (int)m_data.useOfProperty);
        string riesgomismadireccion = "N"; //MapfreRelations.MismaDireccionRiesgo(mismadireccion);
        //Clasificacion siempre 1
        string tipoClasificion = MapfreRelations.TipoClasificacion(1);
        string tipoCalle = "1";
        string codBeachFront = MapfreRelations.BeachFront((int)m_data.distanceFromSea);
        string numPisos = m_data.stories.ToString();
        string numSotanos = m_data.underground.ToString();
        string tipoMuro = MapfreRelations.TipoMuro((int)m_data.wallType);
        string tipoTecho = MapfreRelations.TipoTecho((int)m_data.roofType);
        string colindanciaFincada = MapfreRelations.ColindanciaFincada((bool)m_data.adjoiningEstates);
        string nombreCalle = m_data.insuredPersonHouse1.street;
        string numExterior = m_data.insuredPersonHouse1.exteriorNo;

        CultureInfo culture = new CultureInfo("pt-BR");
        string fechaInicio = string.Empty;
        string fechaFin = string.Empty;
        if (m_data.startDate != null)
        {
            fechaInicio = ((DateTime)m_data.startDate).ToString("d", culture);
            fechaFin = ((DateTime)m_data.endDate).ToString("d", culture);
        }
        else if (generateDates)
        {
            DateTime inicio = DateTime.Now.AddDays(1);
            DateTime fin = inicio.AddYears(1).AddSeconds(-1);
            fechaInicio = inicio.ToString("d", culture);
            fechaFin = fin.ToString("d", culture);
        }
        else
        {
            throw new InvalidOperationException();
        }
        string fechaEmision = DateTime.Today.ToString("d", culture);
        string usuario = "GMAC0100";
        string codPais = "MEX";

        string fechaValidez = m_isTest ? "5/26/2008" : "01/04/2005";

        string codEstado = MapfreRelations.GetStateCode(m_data.insuredPersonHouse1.HouseEstate.City.State).ToString();
        string codPoblacion = MapfreRelations.GetCityCode(m_data.insuredPersonHouse1.HouseEstate.City).ToString();
        string codPostal = m_data.insuredPersonHouse1.ZipCode;

        string coberturas = GetCoberturasRows(m_data, dataCoverages);
        string primas = GetPrimasRows(m_data, dataCoverages);
        string terceros = GetRowsTerceros(fechaFin, rfcs.ToArray());
        string datosTerceros = GetRowsDatosTerceros(fechaEmision, m_data, rfcs, codEstado, codPoblacion);
        string variableRows = GetVariableRows(m_data);
        string codFraccPago = MapfreRelations.CodigoPagoFraccionado(m_data.cantidadDePagos);

        string result = string.Format(genericxml, fechaValidez, fechaEmision, fechaInicio, fechaFin, GetCurrency(), rfcnumber, m_cuadroCom,
            m_meagent, usuario, m_comEmisora, fechaEmision, m_nombreRiesgo, codArea, tipoClasificion, riesgomismadireccion,
            tipoCalle, nombreCalle, numExterior, codPais, codEstado, codPoblacion, codPostal, codBeachFront, numPisos, numSotanos,
            tipoMuro, tipoTecho, colindanciaFincada, coberturas, primas, terceros, datosTerceros, variableRows, codFraccPago);
        return result;
    }

    public string GetXml()
    {
        return GetXml(false);
    }

    public string GetXml(bool generateDates)
    {
        string genericXml = GetGenericXml();
        MapfreModelPolicy dataCoverages = new MapfreModelPolicy(m_connection);
        string resultXml = FillXmlDocument(genericXml, dataCoverages, generateDates);

        return resultXml;
    }
}
