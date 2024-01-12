using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text.RegularExpressions;
using ME.Admon;
using Protec.Authentication.Membership;

/// <summary>
/// Provee metodos para generar el PDF de la poliza
/// </summary>
public static class PolizaPDFFillerT1
{
    private static decimal getExchangeRate()
    {
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        return user.ExchangeRate;
    }

    private static int WordBoundaryBefore(string line, int length)
    {
        if (line.Length < length)
            return line.Length - 1;
        int index = length - 1;
        while (index > 0 && line[index] != ' ')
            index--;
        return index;
    }

    private static int WordBoundaryAfter(string line, int fromWhere)
    {
        int currentIndex = fromWhere;
        while (currentIndex < line.Length && char.IsWhiteSpace(line[currentIndex]))
            currentIndex++;
        return currentIndex;
    }

    private static bool AddedNewPage(ref int lineCount, int limitWatchdog, ref float cheight, TemplateValues values, 
                                     Document document, PdfContentByte cb, PdfContentByte cbUnder, PdfTemplate under,
                                     PdfTemplate template)
    {
        if (lineCount >= limitWatchdog) {
            document.NewPage();
            cheight = values.top - 50.0f - TemplateValues.boxheight * 5 - 3.0f * 6 - 10.0f;
            cb.AddTemplate(template, 0, 0);
            if (under != null)
                cbUnder.AddTemplate(under, 0, 0);
            lineCount = 0;
            return true;
        }
        return false;
    }

    public static void FillPDF(
        PdfWriter writer, 
        PdfTemplate template,
        PdfTemplate under,
        policy data, 
        decimal netPremium, 
        DeduciblesCoaseguros dataDeducibles, 
        TemplateValues values,
        PolizaStringFactory stringFactory,
        TemplateLanguage language
    ) {
        PdfContentByte cb = writer.DirectContent;
        PdfContentByte cbUnder = writer.DirectContentUnder;
        Document document = cb.PdfDocument;
        BaseFont pdfFont = values.pdfFont;
        bool english = language == TemplateLanguage.English;
        cb.AddTemplate(template, 0, 0);
        if (under != null)
            cbUnder.AddTemplate(under, 0, 0);
        float cheight = values.top - 50.0f - TemplateValues.boxheight * 5 - 3.0f * 6 - 10.0f;
        float diffP = TemplateValues.diffText + 8.0f;
        cb.DrawText(stringFactory.GetString("LblRiskAddress"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= diffP;
        insuredPersonHouse hData = data.insuredPersonHouse1;

        string direccion = hData.street + " " + hData.exteriorNo + (string.IsNullOrEmpty(hData.InteriorNo) ? " " : " - " + hData.InteriorNo + " ")
            + hData.HouseEstate.Name + ", " + hData.HouseEstate.City.Name + ", " + hData.HouseEstate.City.State.Name;
        direccion = stringFactory.GetString("Unique") + direccion;
        if (direccion.Length <= 100)
        {
            cb.DrawText(direccion, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
        }
        else
        {
            while (direccion.Length > 0)
            {
                int index = WordBoundaryBefore(direccion, 100);
                if (index != 0)
                {
                    cb.DrawText(direccion.Substring(0, index + 1), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                }
                else
                {
                    cb.DrawText(direccion, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                    break;
                }
                index = WordBoundaryAfter(direccion, index + 1);
                direccion = direccion.Substring(index);
            }
        }
        cb.DrawText(stringFactory.GetString("Zip") + data.insuredPersonHouse1.ZipCode, pdfFont, TemplateValues.textSize, values.left + 60.0f, cheight);
        cheight -= TemplateValues.diffText;
        string techo, pared, tipo, uso;
        if (english) {
            techo = data.roofType1.description;
            pared = data.wallType1.description;
            tipo = data.typeOfProperty1.description;
            uso = data.useOfProperty1.description;
        } else {
            techo = data.roofType1.descriptionEs;
            pared = data.wallType1.descriptionEs;
            tipo = data.typeOfProperty1.descriptionEs;
            uso = data.useOfProperty1.descriptionEs;
        }
        string caracteristicas = stringFactory.GetString("Characteristics") + tipo + ", " 
            + uso + ", " 
            + stringFactory.GetString("Stories") + data.stories.ToString() + ", " 
            + stringFactory.GetString("Construction") + pared + stringFactory.GetString("CRoof") + techo;
        caracteristicas = caracteristicas.ToUpper();
        if (caracteristicas.Length <= 100)
        {
            cb.DrawText(caracteristicas, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        }
        else
        {
            while (caracteristicas.Length > 0)
            {
                int index = WordBoundaryBefore(caracteristicas, 100);
                if (index != 0)
                {
                    cb.DrawText(caracteristicas.Substring(0, index + 1), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                }
                else
                {
                    cb.DrawText(caracteristicas, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                    break;
                }
                index = WordBoundaryAfter(caracteristicas, index + 1);
                caracteristicas = caracteristicas.Substring(index);
            }
        }
        cheight -= TemplateValues.diffText;
        string mseguridad = string.Empty;
        if ((bool)data.tvCircuit)
            mseguridad = stringFactory.GetString("TVCircuit");
        if ((bool)data.tvGuard)
            mseguridad += stringFactory.GetString("TVGuard");
        if ((bool)data.guards24)
            mseguridad += stringFactory.GetString("Guards24");
        if ((bool)data.localAlarm)
            mseguridad += stringFactory.GetString("LocalAlarm");
        if ((bool)data.centralAlarm)
            mseguridad += stringFactory.GetString("CentralAlarm");
        if (string.IsNullOrEmpty(mseguridad))
            mseguridad = stringFactory.GetString("NoSecurity");
        else {
            mseguridad = mseguridad.Substring(0, mseguridad.Length - 2);
            mseguridad = mseguridad.Trim();
        }
        mseguridad = stringFactory.GetString("SecurityMeasures") + mseguridad;
        mseguridad = mseguridad.ToUpper();
        if (mseguridad.Length <= 100) {
            cb.DrawText(mseguridad, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        } else {
            while (mseguridad.Length > 0) {
                int index = WordBoundaryBefore(mseguridad, 100);
                if (index != 0) {
                    cb.DrawText(mseguridad.Substring(0, index + 1), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                } else {
                    cb.DrawText(mseguridad, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                    break;
                }
                index = WordBoundaryAfter(mseguridad, index + 1);
                mseguridad = mseguridad.Substring(index);
            }
        }
        cheight -= TemplateValues.diffText;
        cb.DrawText(
            stringFactory.GetString("AdjacentStructures") + ((bool)data.adjoiningEstates ? stringFactory.GetString("Yes") : stringFactory.GetString("No")),
            pdfFont,
            TemplateValues.textSize,
            values.left + 30.0f,
            cheight
        );
        cheight -= diffP;

        cb.DrawText(stringFactory.GetString("Section"), pdfFont, TemplateValues.textSize, values.left + 70.0f, cheight);
        cb.DrawText(stringFactory.GetString("PropertyCoveredTitle"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, stringFactory.GetString("LimitsTitle"), pdfFont, TemplateValues.textSize, values.right - 70.0f, cheight);
        cheight -= 4.0f;
        cb.DrawText("--------", pdfFont, TemplateValues.textSize, values.left + 70.0f, cheight);
        cb.DrawText("-------------------------------", pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, "-----------------------", pdfFont, TemplateValues.textSize, values.right - 70.0f, cheight);
        cheight -= TemplateValues.diffText;
        string covered = stringFactory.GetString("Covered");
        string excluded = stringFactory.GetString("Excluded");
        string fireAdd = stringFactory.GetString("FireAdd");
        string allRiskAdd = stringFactory.GetString("AllRiskAdd");
        string lblFHM = stringFactory.GetString("LblHMP");
        string lblQuake = stringFactory.GetString("LblQuake");
        string lblOtherStructures = stringFactory.GetString("OtherStructures");
        string lblMainBuilding = stringFactory.GetString("MainBuilding");
        string incendio = (bool)data.fireAll ? covered : excluded;
        string quake = (bool)data.quake ? covered : excluded;
        string hmp = (bool)data.HMP ? covered : excluded;
        int lineCount = 0;
        //if (data.buildingLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " I", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("Building"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                (data.buildingLimit > 0) ? (data.buildingLimit + data.otherStructuresLimit).ToString("N2") : excluded,
                pdfFont,
                TemplateValues.textSize,
                values.right - 70.0f,
                cheight
            );
            cheight -= TemplateValues.diffText;

            lineCount++;

            cb.DrawText(fireAdd, pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.buildingLimit > 0) ? incendio : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            cb.DrawText(allRiskAdd, pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.buildingLimit > 0) ? covered : excluded, 
                pdfFont, TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount += 2;
            
            //if (data.outdoorLimit > 0) 
                cb.DrawText(stringFactory.GetString("DOutdoor"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
                cb.DrawTextAligned(
                    PdfContentByte.ALIGN_RIGHT,
                    (data.outdoorLimit > 0 && data.buildingLimit > 0) ? covered : excluded, 
                    pdfFont, 
                    TemplateValues.textSize, 
                    values.right - 70.0f, 
                    cheight
                );
                cheight -= TemplateValues.diffText;
                lineCount ++;
              
            
            //if ((bool)data.HMP) {
                cb.DrawText(lblFHM, pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawTextAligned(
                    PdfContentByte.ALIGN_RIGHT, 
                    (data.buildingLimit > 0) ? hmp : excluded, 
                    pdfFont, 
                    TemplateValues.textSize, 
                    values.right - 70.0f, 
                    cheight
                );
                cheight -= TemplateValues.diffText;
                lineCount++;
            

            //if ((bool)data.quake) {
                cb.DrawText(lblQuake, pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawTextAligned(
                    PdfContentByte.ALIGN_RIGHT, 
                    (data.buildingLimit > 0) ? quake: excluded, 
                    pdfFont, 
                    TemplateValues.textSize, 
                    values.right - 70.0f, 
                    cheight
                );
                cheight -= TemplateValues.diffText;
                lineCount++;
            
        
        //if (data.contentsLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " II", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("Contents"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.contentsLimit > 0) ? data.contentsLimit.ToString("N2") : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            cb.DrawText(fireAdd, pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, incendio, pdfFont, TemplateValues.textSize, values.right - 70.0f, cheight);
            cheight -= TemplateValues.diffText;
            cb.DrawText(allRiskAdd, pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.contentsLimit > 0) ? covered : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount += 3;
            //if ((bool)data.HMP) {
                cb.DrawText(lblFHM, pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, hmp, pdfFont, TemplateValues.textSize, values.right - 70.0f, cheight);
                cheight -= TemplateValues.diffText;
                lineCount++;
            
            //if ((bool)data.quake) {
                cb.DrawText(lblQuake, pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, quake, pdfFont, TemplateValues.textSize, values.right - 70.0f, cheight);
                cheight -= TemplateValues.diffText;
                lineCount++;
            
        
        //if (data.debriLimit + data.extraLimit + data.lossOfRentsLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " III", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("ConsLoss"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cheight -= TemplateValues.diffText;
            lineCount++;
            //if (data.debriLimit > 0) {
                cb.DrawText(stringFactory.GetString("Debri"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
                cb.DrawTextAligned(
                    PdfContentByte.ALIGN_RIGHT, 
                    (data.debriLimit > 0) ? data.debriLimit.ToString("N2") : excluded, 
                    pdfFont, 
                    TemplateValues.textSize, 
                    values.right - 70.0f, 
                    cheight
                );
                cheight -= TemplateValues.diffText;
                lineCount++;
            
            //if (data.extraLimit > 0) {
                cb.DrawText(stringFactory.GetString("ExtraLimit"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
                cb.DrawTextAligned(
                    PdfContentByte.ALIGN_RIGHT, 
                    (data.extraLimit > 0) ? data.extraLimit.ToString("N2") : excluded, 
                    pdfFont, 
                    TemplateValues.textSize, 
                    values.right - 70.0f, 
                    cheight
                );
                cheight -= TemplateValues.diffText;
                lineCount++;
            
            //if (data.lossOfRentsLimit > 0) {
                string asterisk = (data.HMP || data.quake) ? "*" : "";
                cb.DrawText(asterisk + stringFactory.GetString("LossRents"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
                cb.DrawTextAligned(
                    PdfContentByte.ALIGN_RIGHT, 
                    (data.lossOfRentsLimit > 0) ? data.lossOfRentsLimit.ToString("N2") : excluded, 
                    pdfFont, 
                    TemplateValues.textSize, 
                    values.right - 70.0f, 
                    cheight
                );
                cheight -= TemplateValues.diffText;
                lineCount++;
            
        
        //if (data.CSLLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " IV", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("CivilLiability"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.CSLLimit > 0) ? data.CSLLimit.ToString("N2") : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("DomesticWorkers"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                data.domesticWorkersLiability ? covered : excluded,
                pdfFont,
                TemplateValues.textSize,
                values.right - 70.0f,
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount += 2;
            if (data.typeOfClient == ApplicationConstants.RENTER)
            {
                cb.DrawText(stringFactory.GetString("TenantLiability"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, 
                    covered,
                    pdfFont,
                    TemplateValues.textSize,
                    values.right - 70.0f,
                    cheight
                );
                cheight -= TemplateValues.diffText;
                lineCount ++;
            }

        //if (data.burglaryLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " V", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("Burglary"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.burglaryLimit > 0) ? data.burglaryLimit.ToString("N2") : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount++;
            //if (data.burglaryJewelryLimit > 0) {
                cb.DrawText(stringFactory.GetString("JewelryBurglary"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
                cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
                cb.DrawTextAligned(
                    PdfContentByte.ALIGN_RIGHT, 
                    (data.burglaryJewelryLimit > 0) ? data.burglaryJewelryLimit.ToString("N2") : excluded, 
                    pdfFont, 
                    TemplateValues.textSize, 
                    values.right - 70.0f, 
                    cheight
                );
                cheight -= TemplateValues.diffText;
                lineCount++;
            
        
        //if (data.moneySecurityLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " VI", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("MoneyLimit"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.moneySecurityLimit > 0) ? data.moneySecurityLimit.ToString("N2") : excluded, 
                pdfFont, 
                TemplateValues.textSize,
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount++;
        
        //if (data.accidentalGlassBreakageLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " VIII", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("CrystalLimit"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.accidentalGlassBreakageLimit > 0) ? data.accidentalGlassBreakageLimit.ToString("N2") : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount++;
        
        //if (data.electronicEquipmentLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " IX", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("ElectronicLimit"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.electronicEquipmentLimit > 0) ? data.electronicEquipmentLimit.ToString("N2") : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount++;
        
        //if (data.personalItemsLimit > 0) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " X", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("PersonalLimit"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 145.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT, 
                (data.personalItemsLimit > 0) ? data.personalItemsLimit.ToString("N2") : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount++;
        
        //if (data.familyAssistance) {
            cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, " XI", pdfFont, TemplateValues.textSize, values.left + 75.0f, cheight);
            cb.DrawText(stringFactory.GetString("FamilyAssistance"), pdfFont, TemplateValues.textSize, values.left + 150.0f, cheight);
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("TravelAssistance"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                (data.familyAssistance) ? covered : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("WarrantyTravel"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                (data.familyAssistance) ? covered : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("Meditel"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                (data.familyAssistance) ? covered : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("LegalAssistance"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                (data.familyAssistance) ? covered : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount += 5;
        

        //if (data.techSupport) {
            cb.DrawText(stringFactory.GetString("TechSupport"), pdfFont, TemplateValues.textSize, values.left + 175.0f, cheight);
            cb.DrawTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                (data.techSupport) ? covered : excluded, 
                pdfFont, 
                TemplateValues.textSize, 
                values.right - 70.0f, 
                cheight
            );
            cheight -= TemplateValues.diffText;
            lineCount++;
        

        //si se cubre 3931 y 3932 tendria que aparecer aqui en el pdf

            cheight -= TemplateValues.diffText;
        if ((data.HMP || data.quake) && data.lossOfRentsLimit > 0)
        {
            cb.DrawText(stringFactory.GetString("LossRentsDisaster"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight, BaseColor.RED);
            cheight -= TemplateValues.diffText;
        }

        cb.DrawText(stringFactory.GetString("AutoRenewal"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText;
        cb.DrawText(stringFactory.GetString("NoValueUpdate"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText;

        int limitWatchdog = 1 + (string.IsNullOrEmpty(data.insuredPersonHouse1.bankName) ? 0 : 2) +
            (data.outdoorLimit > 0M ? 0 : 1);
        limitWatchdog = 28 - limitWatchdog;

        bool newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);

        string[] abreviaturas =  { 
            "R.F.C. REGISTRO FEDERAL DE CAUSANTES",
            "C.P. CÓDIGO POSTAL",
            "COL. COLONIA",
            "R.C. RESPONSABILIDAD CIVIL",
            "INT. INTERNACIONALES",
            "FIN. FINANCIAMIENTO",
            "FRAC. FRACCIONADO"
        };
        if (language == TemplateLanguage.Spanish)
        {
            foreach (string linea in abreviaturas)
            {
                cb.DrawText(linea, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
                lineCount += 1;
                newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
            }
            
        }

        cheight -= diffP;
        lineCount += 1;

        string distanceSea = english ? data.distanceFromSea1.description : data.distanceFromSea1.descriptionEs;
        cb.DrawText(stringFactory.GetString("DistanceSea") + distanceSea.ToUpper(), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        lineCount += 3;

        newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
        if (!string.IsNullOrEmpty(data.insuredPersonHouse1.bankName)) {
            cheight -= diffP;
            string bankName = stringFactory.GetString("BankName") + data.insuredPersonHouse1.bankName.ToUpper().Trim();
            while (bankName.Length > 0) {
                if (!newPage) {
                    newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
                }
                int index = WordBoundaryBefore(bankName, 100);
                if (index != 0) {
                    cb.DrawText(bankName.Substring(0, index + 1), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                } else {
                    cb.DrawText(bankName, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                    break;
                }
                lineCount++;
                index = WordBoundaryAfter(bankName, index + 1);
                bankName = bankName.Substring(index);

            }
            
            if (!string.IsNullOrEmpty(data.insuredPersonHouse1.loanNumber)) {
                if (!newPage) {
                    newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
                }
                cb.DrawText(stringFactory.GetString("LoanNumber") + data.insuredPersonHouse1.loanNumber.ToUpper(), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                lineCount++;
                cheight -= TemplateValues.diffText;
            }
            if (!string.IsNullOrEmpty(data.insuredPersonHouse1.bankAddress)) {
                if (!newPage) {
                    newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
                }
                string bankAddress = data.insuredPersonHouse1.bankAddress;
                while (bankAddress.Length > 0) {
                    if (!newPage) {
                        newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
                    }
                    int index = WordBoundaryBefore(bankAddress, 100);
                    if (index != 0) {
                        cb.DrawText(bankAddress.Substring(0, index + 1), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                        cheight -= TemplateValues.diffText;
                    } else {
                        cb.DrawText(bankAddress, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                        cheight -= TemplateValues.diffText;
                        break;
                    }
                    lineCount++;
                    index = WordBoundaryAfter(bankAddress, index + 1);
                    bankAddress = bankAddress.Substring(index);

                }
            }
        }
        if (data.outdoorLimit > 0M) {
            cheight -= diffP;
            cb.DrawText(stringFactory.GetString("OutdoorLimit") + data.outdoorLimit.ToString("N2"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= diffP;

            string outdoorDescription = stringFactory.GetString("OutdoorPropertyInfo") + data.outdoorDescription;
            Regex newLines = new Regex(@"\.?\s*\n");
            outdoorDescription = newLines.Replace(outdoorDescription, ". ").ToUpper().Trim();
            if (outdoorDescription.Length <= 100) {
                cb.DrawText(outdoorDescription, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            } else {
                while (outdoorDescription.Length > 0) {
                    if (!newPage) {
                        newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
                    }
                    int index = WordBoundaryBefore(outdoorDescription, 100);
                    if (index != 0) {
                        cb.DrawText(outdoorDescription.Substring(0, index + 1), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                        cheight -= TemplateValues.diffText;
                    } else {
                        cb.DrawText(outdoorDescription, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                        cheight -= TemplateValues.diffText;
                        break;
                    }
                    lineCount++;
                    index = WordBoundaryAfter(outdoorDescription, index + 1);
                    outdoorDescription = outdoorDescription.Substring(index);
                    
                }
            }
        }

        if (data.otherStructuresLimit > 0) {
            cheight -= diffP;
            string otherStructuresDescription = stringFactory.GetString("OtherStructuresInfo") + data.otherStructuresDescription;
            Regex newLines = new Regex(@"\.?\s*\n");
            otherStructuresDescription = newLines.Replace(otherStructuresDescription, ". ").ToUpper().Trim();
            if (otherStructuresDescription.Length <= 100) {
                cb.DrawText(otherStructuresDescription, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            } else {
                while (otherStructuresDescription.Length > 0) {
                    if (!newPage) {
                        newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
                    }
                    int index = WordBoundaryBefore(otherStructuresDescription, 100);
                    if (index != 0) {
                        cb.DrawText(otherStructuresDescription.Substring(0, index + 1), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                        cheight -= TemplateValues.diffText;
                    } else {
                        cb.DrawText(otherStructuresDescription, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                        cheight -= TemplateValues.diffText;
                        break;
                    }
                    lineCount++;
                    index = WordBoundaryAfter(otherStructuresDescription, index + 1);
                    otherStructuresDescription = otherStructuresDescription.Substring(index);
                }
            }
        }

        if (!string.IsNullOrEmpty(data.additionalNotes))
        {
            cheight -= diffP;
            string additionalNotes = stringFactory.GetString("AdditionalNotes") + data.additionalNotes;
            Regex newLines = new Regex(@"\.?\s*\n");
            additionalNotes = newLines.Replace(additionalNotes, ". ").ToUpper().Trim();
            if (additionalNotes.Length <= 100)
            {
                cb.DrawText(additionalNotes, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            }
            else
            {
                while (additionalNotes.Length > 0)
                {
                    if (!newPage)
                    {
                        newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
                    }
                    int index = WordBoundaryBefore(additionalNotes, 100);
                    if (index != 0)
                    {
                        cb.DrawText(additionalNotes.Substring(0, index + 1), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                        cheight -= TemplateValues.diffText;
                    }
                    else
                    {
                        cb.DrawText(additionalNotes, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                        cheight -= TemplateValues.diffText;
                        break;
                    }
                    lineCount++;
                    index = WordBoundaryAfter(additionalNotes, index + 1);
                    additionalNotes = additionalNotes.Substring(index);
                }
            }
        }

        cheight -= diffP * 2;
        if (!newPage) {
            newPage = AddedNewPage(ref lineCount, limitWatchdog, ref cheight, values, document, cb, cbUnder, under, template);
        }
        cb.DrawText(stringFactory.GetString("RestConditions"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= diffP * 2;
        if (lineCount < limitWatchdog && !newPage) {
            document.NewPage();
            cheight = values.top - 50.0f - TemplateValues.boxheight * 5 - 3.0f * 6 - 10.0f;
            cb.AddTemplate(template, 0, 0);
            if (under != null)
                cbUnder.AddTemplate(under, 0, 0);
        }
       
        /* especificacion de deducibles */
        float itextSize = TemplateValues.textSize;
        cb.DrawText(stringFactory.GetString("DeductibleTitle"), pdfFont, itextSize, values.left + 30.0f, cheight);
        cheight -= diffP;
        string terremoto = string.Empty;
        if (dataDeducibles.DeducibleTerremoto != null)
            terremoto += dataDeducibles.DeducibleTerremoto + stringFactory.GetString("OverPremium");
        if (dataDeducibles.CoaseguroTerremoto != null) {
            if (terremoto.Length > 11)
                terremoto += stringFactory.GetString("AndWS");
            terremoto += stringFactory.GetString("CoinsuranceOf") + dataDeducibles.CoaseguroTerremoto + stringFactory.GetString("OverLoss");
        }
        if (terremoto.Length > 0) {
            cb.DrawText(stringFactory.GetString("DQuake") + terremoto, pdfFont, itextSize, values.left + 30.0f, cheight);
            cheight -= diffP;
        }
        string fhm = string.Empty;
        if (dataDeducibles.DeducibleFHM != null)
            fhm += dataDeducibles.DeducibleFHM + stringFactory.GetString("OverPremium");
        if (dataDeducibles.CoaseguroFHM != null) {
            if (fhm.Length > 31) {
                fhm += stringFactory.GetString("AndWS");
            }
            fhm += stringFactory.GetString("CoinsuranceOf") + dataDeducibles.CoaseguroFHM + stringFactory.GetString("OverLoss");
        }
        if (fhm.Length > 0) {
            cb.DrawText(stringFactory.GetString("DHMP") + fhm, pdfFont, 7.8f, values.left + 30.0f, cheight);
            cheight -= diffP;
        }
        string intemperie = string.Empty;
        if (dataDeducibles.DeducibleIntemperie != null) {
            intemperie += dataDeducibles.DeducibleIntemperie + stringFactory.GetString("OverPremium");
        }
        if (dataDeducibles.CoaseguroIntemperie != null) {
            if (intemperie.Length > 24) {
                intemperie += stringFactory.GetString("AndWS");
            }
            intemperie += stringFactory.GetString("CoinsuranceOf") + dataDeducibles.CoaseguroIntemperie + stringFactory.GetString("OverLoss");
        }
        if (intemperie.Length > 0) {
            cb.DrawText(stringFactory.GetString("OutdoorPropertyInfo") + intemperie, pdfFont, 7.8f, values.left + 30.0f, cheight);
            cheight -= diffP;
        }

        if (data.electronicEquipmentLimit > 0) {
            if (data.currency == ApplicationConstants.DOLLARS_CURRENCY) {
                cb.DrawText(stringFactory.GetString("ElectronicFranchiseDollars"), pdfFont, itextSize, values.left + 30.0f, cheight);
            } else if (data.currency == ApplicationConstants.PESOS_CURRENCY) {
                cb.DrawText(stringFactory.GetString("ElectronicFranchisePesos"), pdfFont, itextSize, values.left + 30.0f, cheight);
            }
            cheight -= diffP;
        }

        cb.DrawText(stringFactory.GetString("RestNoDed"), pdfFont, itextSize, values.left + 30.0f, cheight);
        cheight -= diffP;
        if (english) {
            cb.DrawText("SPECIAL CONDITIONS APPLICABLE TO THIS POLICY:", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
            cb.DrawText("GENERAL CONDITIONS FORM DAHO-001-C", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
            if (data.quake) {
                cb.DrawText("EARTHQUAKE AND VOLCANIC ERUPTION ENDORSEMENT", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
            if (data.HMP) {
                cb.DrawText("HYDRO-METEOROLOGICAL PHENOMENON ENDORSEMENT", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
            cb.DrawText("EXCLUSION OF CYBERNETIC RISK ENDORSEMENT", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
            if (data.currency != ApplicationConstants.PESOS_CURRENCY) {
                cb.DrawText("FOREIGN CURRENCY ENDORSEMENT", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
        }

        cb.DrawText(stringFactory.GetString("BuildingBreakdown"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText;
        lineCount++;

        cb.DrawText(lblMainBuilding, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 175.0f, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, (data.buildingLimit).ToString("N2"), pdfFont, TemplateValues.textSize, values.right - 100.0f, cheight);
        cheight -= TemplateValues.diffText;
        lineCount++;

        if (data.otherStructuresLimit > 0)
        {
            cb.DrawText(lblOtherStructures, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cb.DrawText("$", pdfFont, TemplateValues.textSize, values.right - 175.0f, cheight);
            cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, data.otherStructuresLimit.ToString("N2"), pdfFont, TemplateValues.textSize, values.right - 100.0f, cheight);
            cheight -= TemplateValues.diffText;
            lineCount ++;
        }

        if (english)
        {
            decimal firstRiskAt80Limit = data.buildingLimit +
            data.contentsLimit +
            data.debriLimit +
            data.extraLimit;

            if
            (
                (data.currency == ApplicationConstants.DOLLARS_CURRENCY && firstRiskAt80Limit > 5000000M)
                ||
                (data.currency == ApplicationConstants.PESOS_CURRENCY && firstRiskAt80Limit > (5000000M * getExchangeRate()))
            )
            {
                cheight -= TemplateValues.diffText;
                cb.DrawText(stringFactory.GetString("RiskAt80Line1"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
                cb.DrawText(stringFactory.GetString("RiskAt80Line2"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
                cb.DrawText(stringFactory.GetString("RiskAt80Line3"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
                cb.DrawText(stringFactory.GetString("RiskAt80Line4"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
                cb.DrawText(stringFactory.GetString("RiskAt80Line5"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
        }
    }
}
