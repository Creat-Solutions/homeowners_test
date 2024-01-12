using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using ME.Admon;

public static class MapfreRelations
{
    public static int GetStateCode(State state)
    {
        return state.MapfreCode;
        /*
        int code = 0;
        try {
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            code = db.spList_MapfreStateCode(state.id).Single().stateCode;
        } catch {
            code = 0;
        }
        return code;
        */
    }

    public static int GetCityCode(City city)
    {
        return int.Parse(city.Code);
        //HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        /*
        int retTemp = db.spList_MapfreCityCode(city.id).Single().cityCode;
        if (retTemp == 0) {
            retTemp = db.spList_MapfreStateCode(city.state).Single().stateCode * 1000 + 999;
        }
        return retTemp;
        */
    }

    public static string TipoMuro(int tipoMuro)
    {
        if (tipoMuro == 4) {
            /* concreto */
            return "1";
        } else if (tipoMuro == 5) {
            /* madera */
            return "2";
        }
        return "1";
    }

    public static string TipoTecho(int tipoTecho)
    {
        if (tipoTecho == 1) {
            /* concreto */
            return "1";
        } else if (tipoTecho == 2) {
            /* madero */
            return "2";
        }
        return "1";
    }

    public static string ColindanciaFincada(bool col)
    {
        if (col)
            return "S";
        return "N";
    }

    public static string MismaDireccionRiesgo(bool direccion)
    {
        if (direccion)
            return "S";
        return "N";
    }

    public static string BeachFront(int front)
    {
        //HARD CODED
        if (front == 2) { /* Menos de 500 metros */
            return "1";
        } else { /* mas de 500 metros */
            return "2";
        }
    }

    public static string Sexo(int sexo)
    {
        return sexo.ToString();
    }

    public static string PersonaFisica(bool fisica)
    {
        if (fisica)
            return "S";
        return "N";
    }

    private static int[] convTipoTable = { 
        4, /*CABIN/CABANA */
        -1, /*INVALID */
        8, /*CONDOMINIO HORIZONTAL */
        7,/*CONDOMINIO VERTICAL*/
        -1 /* INCOMPLETE HOUSE/CASA */ 
                                         };
    private static int[] convUsoTable = {
                                            3, /* RESIDENCE */
                                            2, /* VACATION USE (WEEKENDS?) */
                                            2 /* WEEKENDS */
                                         };

    public static string TipoPropiedadToCodArea(int tipoPropiedad, int usoPropiedad)
    {
        /* la siguiente tabla de conversion es altamente dependiente de los indices de
         * dbo.v1_typeOfProperty en la base de datos y dbo.v1_useOfProperty */
        int returnData = -1;
        if (tipoPropiedad < 2 || tipoPropiedad > 6)
            returnData = -1;
        returnData = convTipoTable[tipoPropiedad - 2];
        if (returnData == -1) {
            if (usoPropiedad < 1 || usoPropiedad > 3)
                returnData = -1;
            else
                returnData = convUsoTable[usoPropiedad - 1];
        }
        return returnData.ToString();
    }

    public static string TipoClasificacion(int tipo)
    {
        return tipo.ToString();
    }

    public static string CodigoPagoFraccionado(int numOfPayments)
    {
        switch (numOfPayments) {
            case 1:
            default:
                return "1";
            case 2:
                return "2";
            case 4:
                return "3";
            case 12:
                return "4";
        }
    }


    private static string ABCUpper = "ABCDEFGHJKLMNPQRSTWXYZ";
    private static Random m_generator = new Random();
    public static string GenerateRFC(string apaterno, string amaterno, string nombre, bool legalPerson, int index)
    {
        // de acuerdo a lo enviado por Jose Miguel Gomez Juarez el 11/julio/2012
        if (index == 0) {
            return "XEXX010101AAA";
        } else {
            return string.Format("XEXX010101AA{0}", index);
        }
        // de acuerdo a lo enviado por Jose Miguel Gomez Juarez el 10/julio/2012
        //return "XEXX010101000";
        /*
        return "RAXT880101II1";
        
        if (string.IsNullOrEmpty(apaterno) || apaterno.Length < 2) {
            apaterno = "xx";
        }
        if (string.IsNullOrEmpty(amaterno)) {
            amaterno = "x";
        }
        if (string.IsNullOrEmpty(nombre)) {
            nombre = "x";
        }
        string rfc = string.Empty;
        rfc += apaterno.Substring(0, legalPerson ? 1 : 2);
        rfc += amaterno[0];
        rfc += nombre[0];

        rfc += (m_generator.Next(39) + 50).ToString(); //50-88
        rfc += (m_generator.Next(12) + 1).ToString("D2");
        rfc += (m_generator.Next(12) + 1).ToString("D2");

        rfc += ABCUpper[m_generator.Next(ABCUpper.Length)];
        rfc += ABCUpper[m_generator.Next(ABCUpper.Length)];
        rfc += (m_generator.Next(9) + 1).ToString();
        rfc = rfc.RemoveAccents();

        string regex = @"^[A-Za-z]{4}\d{6}[A-Za-z0-9]{3}$";
        if (legalPerson) {
            regex = @"^[A-Za-z]{3}\d{6}[A-Za-z0-9]{3}$";
        }

        if (!Regex.IsMatch(rfc, regex)) {
            rfc = legalPerson ? "ABC880101II1" : "XXXX880101II1";
        }

        return rfc.ToUpper();
        */
    }
}
