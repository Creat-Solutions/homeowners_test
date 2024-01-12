using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Globalization;

/// <summary>
/// StringExtender
/// </summary>
public static class StringExtender
{
    public static string RemoveAccents(this string original)
    {
        string normalizedString = original.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder(normalizedString);
        for (int i = 0; i < sb.Length; i++) {
            if (CharUnicodeInfo.GetUnicodeCategory(sb[i]) == UnicodeCategory.NonSpacingMark) {
                sb.Remove(i, 1);
            }
        }
        return sb.ToString();
    }
}
