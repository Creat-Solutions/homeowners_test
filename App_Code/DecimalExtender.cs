using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Add extender methods to decimal
/// </summary>
public static class DecimalExtender
{
    public static string GetFormattedNumber(this decimal number)
    {
        if (decimal.Floor(number) == number)
            return number.ToString("F0");
        return number.ToString("F2");
    }

    public static string ToSpanishWrittenString(this decimal number)
    {
        return number.ToSpanishWrittenString(null);
    }

    public static string ToSpanishWrittenString(this decimal number, string units)
    {
        if (number == 0) {
            return simplesMap[0];
        }
        Stack<string> parts = new Stack<string>();
        decimal decimalPart = Math.Round(number - Math.Truncate(number), 2) * 100;
        if (!string.IsNullOrEmpty(units)) {
            parts.Push(units);
        }
        parts.Push(((int)decimalPart).ToString() + "/100");
        int integralPart = (int)Math.Truncate(number);
        int indexGroup = 0;
        while (integralPart > 0) {
            int group = integralPart % 1000;
            integralPart = integralPart / 1000;
            string processedGroup = GetThreeGroup(group);
            if (processedGroup != null) {
                // si es millon o billon se lee un millon o un billon
                string tituloGrupo = gruposMap[indexGroup];
                if (processedGroup == string.Empty && indexGroup > 0 && indexGroup % 2 == 0) {
                    processedGroup = "un";
                } else if (indexGroup > 1) {
                    tituloGrupo = tituloGrupo.Replace('ó', 'o') + "es";
                }
                parts.Push((processedGroup + " " + tituloGrupo).Trim());
            }
            indexGroup++;
        }
        return string.Join(" ", parts.ToArray());
    }

    private static string GetThreeGroup(int group)
    {
        if (group == 0) {
            return null;
        }
        if (group == 1) {
            return string.Empty;
        }
        string result = string.Empty;
        int hundreds = group / 100;
        int tens = group % 100;
        if (hundreds > 0) {
            result += centenasMap[hundreds - 1];
            if (hundreds == 1 && tens > 0) {
                //ciento instead of cien
                result += "to";
            }
        }
        if (tens > 0) {
            if (tens < 20) {
                result += " " + simplesMap[tens];
            } else {
                result += " " + decenasMap[tens / 10 - 2];
                int unidades = tens % 10;
                if (unidades > 0) {
                    result += " y " + simplesMap[unidades];
                }
            }
        }
        return result;
    }

    private static string[] simplesMap = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete",
        "ocho", "nueve", "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete",
        "dieciocho", "diecinueve"
    };

    private static string[] decenasMap = { "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta",
        "ochenta", "noventa"
    };

    private static string[] centenasMap = { "cien", "doscientos", "trescientos", "cuatrocientos", "quinientos",
        "seiscientos", "setecientos", "ochocientos", "novecientos"
    };

    private static string[] gruposMap = { string.Empty, "mil", "millón", "mil millones", "billón" };

}
