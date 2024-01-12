using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;

/// <summary>
/// Agrega metodos a la clase PdfContentByte para simplificar la generacion de Templates
/// </summary>
public static class PolizaPDFDrawers
{
    public static void DrawLine(this PdfContentByte cb, float x1, float y1, float x2, float y2)
    {
        cb.MoveTo(x1, y1);
        cb.LineTo(x2, y2);
        cb.Stroke();
    }

    public static void DrawBox(this PdfContentByte cb, float x, float y, float width, float height)
    {
        float xTL, yTL, xTR, yTR, xBR, yBR, xBL, yBL;
        xTL = x;
        yTL = y;
        xTR = x + width;
        yTR = y;
        xBR = x + width;
        yBR = y - height;
        xBL = x;
        yBL = y - height;
        cb.MoveTo(xTL, yTL);
        cb.LineTo(xTR, yTR);
        cb.LineTo(xBR, yBR);
        cb.LineTo(xBL, yBL);
        cb.LineTo(xTL, yTL);
        cb.Stroke();
    }

    public static void DrawCurvedBox(this PdfContentByte cb, float x, float y, float width, float height)
    {
        float xA1, xA2, yA1, yA2, xB1, xB2, yB1, yB2, xC1, xC2, yC1, yC2, xD1, xD2, yD1, yD2;
        float xTL, yTL, xTR, yTR, xBR, yBR, xBL, yBL;
        float radius = height * 0.1f;
        if (radius > 8.0f)
            radius = 8.0f;
        /* esquinas */
        xTL = x;
        yTL = y;
        xTR = x + width;
        yTR = y;
        xBR = x + width;
        yBR = y - height;
        xBL = x;
        yBL = y - height;

        /* puntos bezier */
        xA1 = xTL;
        xA2 = xTL + radius;
        yA1 = yTL - radius;
        yA2 = yTL;

        xB1 = xTR - radius;
        xB2 = xTR;
        yB1 = yTR;
        yB2 = yTR - radius;

        xC1 = xBR;
        xC2 = xBR - radius;
        yC1 = yBR + radius;
        yC2 = yBR;

        xD1 = xBL + radius;
        xD2 = xBL;
        yD1 = yBL;
        yD2 = yBL + radius;

        cb.MoveTo(xA1, yA1);
        cb.CurveTo(xTL, yTL, xA2, yA2);

        cb.LineTo(xB1, yB1);
        cb.CurveTo(xTR, yTR, xB2, yB2);

        cb.LineTo(xC1, yC1);
        cb.CurveTo(xBR, yBR, xC2, yC2);

        cb.LineTo(xD1, yD1);
        cb.CurveTo(xBL, yBL, xD2, yD2);

        cb.LineTo(xA1, yA1);
        cb.Stroke();
    }

    public static void DrawImage(this PdfContentByte cb, string virtualPath, float x, float y, float scale)
    {
        /* resolve virtual path */
        string path = HttpContext.Current.Server.MapPath(virtualPath);
        Image imageToDraw = Image.GetInstance(path);
        /* draw it using the transformation matrix */
        cb.AddImage(imageToDraw, imageToDraw.Width * scale, 0, 0, imageToDraw.Height * scale, x, y);
    }

    public static void DrawText(this PdfContentByte cb, string text, BaseFont pdfFont, float size, float x, float y, BaseColor color)
    {
        cb.BeginText();
        cb.SetFontAndSize(pdfFont, size);
        cb.SetColorFill(color);
        cb.SetTextMatrix(x, y);
        cb.ShowText(text);
        cb.EndText();
        cb.SetColorFill(BaseColor.BLACK);
    }

    public static void DrawText(this PdfContentByte cb, string text, BaseFont pdfFont, float size, float x, float y)
    {
        cb.DrawText(text, pdfFont, size, x, y, BaseColor.BLACK);
    }

    public static void DrawTextAligned(this PdfContentByte cb, int alignment, string text, BaseFont pdfFont, float size, float x, float y, BaseColor color)
    {
        cb.BeginText();
        cb.SetFontAndSize(pdfFont, size);
        cb.SetColorFill(color);
        cb.ShowTextAligned(alignment, text, x, y, 0);
        cb.EndText();
        cb.SetColorFill(BaseColor.BLACK);
    }

    public static void DrawTextAligned(this PdfContentByte cb, int alignment, string text, BaseFont pdfFont, float size, float x, float y)
    {
        cb.DrawTextAligned(alignment, text, pdfFont, size, x, y, BaseColor.BLACK);
    }

}
