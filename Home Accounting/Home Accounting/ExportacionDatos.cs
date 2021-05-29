using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class ExportacionDatos
{
    public PdfPTable tblGastosTotales;
    public PdfPTable tblGastosMensuales;
    public Font fuente;

    public ExportacionDatos()
    {
        fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL,
            BaseColor.BLACK);
    }

    public void GenerarCsv(List<Gasto> gastos, string nombreFichero)
    {
        StringBuilder sb = new StringBuilder();
        string rutaInicial = AppDomain.CurrentDomain.BaseDirectory;
        string rutaFinal = Path.Combine(rutaInicial, nombreFichero + ".csv");
        string encabezado = "";
        PropertyInfo[] tipos = typeof(Gasto).GetProperties();

        if (!File.Exists(rutaFinal))
        {
            var file = File.Create(rutaFinal);
            file.Close();
            foreach (var prop in tipos)
            {
                if(prop.Name != "Id" && prop.Name != "Estado")
                    encabezado += "\"" + prop.Name + "\";";
            }
            encabezado = encabezado.Substring(0, encabezado.Length - 1);
            sb.AppendLine(encabezado);
            TextWriter sw = new StreamWriter(rutaFinal, true);
            sw.Write(sb.ToString());
            sw.Close();
        }
        foreach (Gasto obj in gastos)
        {
            sb = new StringBuilder();
            string linea = "";
            foreach (var prop in tipos)
            {
                if (prop.Name != "Id" && prop.Name != "Estado")
                {
                    if (prop.Name == "Fecha")
                    {
                        string fecha = obj.ToStringFecha();
                        linea += "\"" + fecha + "\";";
                    }
                    
                    else
                        linea += "\"" + prop.GetValue(obj, null) + "\";";
                }
            }
            linea = linea.Substring(0, linea.Length - 1);
            sb.AppendLine(linea);
            TextWriter sw = new StreamWriter(rutaFinal, true);
            sw.Write(sb.ToString());
            sw.Close();
        }
    }

    public void GenerarPDFGTotales(List<Gasto> gastos, string nombreFichero)
    {
        Document doc = new Document(PageSize.LETTER);
        PdfWriter writer = PdfWriter.GetInstance(doc, 
            new FileStream(@".\" + nombreFichero + ".pdf", 
                FileMode.Create));

        doc.AddTitle("Gastos");
        doc.AddCreator("AR Software");

        doc.Open();
        doc.Add(new Paragraph("Gastos totales"));
        doc.Add(Chunk.NEWLINE);

        tblGastosTotales = new PdfPTable(5);
        tblGastosTotales.WidthPercentage = 100;

        InsertarEncabezadoTablaPDF();

        RellenarTablaPDF(gastos);

        doc.Add(tblGastosTotales);

        doc.Close();
        writer.Close();
    }

    public void InsertarEncabezadoTablaPDF()
    {
        PdfPCell clFecha = new PdfPCell(new Phrase("Fecha", fuente));
        clFecha.BorderWidth = 0;
        clFecha.BorderWidthBottom = 0.75f;

        PdfPCell clConcepto = new PdfPCell(new Phrase("Concepto", fuente));
        clConcepto.BorderWidth = 0;
        clConcepto.BorderWidthBottom = 0.75f;

        PdfPCell clImporte = new PdfPCell(new Phrase("Importe", fuente));
        clImporte.BorderWidth = 0;
        clImporte.BorderWidthBottom = 0.75f;

        PdfPCell clCategoria = new PdfPCell(new Phrase("Categoría", fuente));
        clCategoria.BorderWidth = 0;
        clCategoria.BorderWidthBottom = 0.75f;

        PdfPCell clCuenta = new PdfPCell(new Phrase("Cuenta", fuente));
        clCuenta.BorderWidth = 0;
        clCuenta.BorderWidthBottom = 0.75f;

        tblGastosTotales.AddCell(clFecha);
        tblGastosTotales.AddCell(clConcepto);
        tblGastosTotales.AddCell(clImporte);
        tblGastosTotales.AddCell(clCategoria);
        tblGastosTotales.AddCell(clCuenta);
    }

    public void RellenarTablaPDF(List<Gasto> gastos)
    {
        PdfPCell clFecha;
        PdfPCell clConcepto;
        PdfPCell clImporte;
        PdfPCell clCategoria;
        PdfPCell clCuenta;

        for (int i = 0; i < gastos.Count; i++)
        {
            clFecha = new PdfPCell(new Phrase(gastos[i].ToStringFecha(), fuente));
            clFecha.BorderWidth = 0;
            clConcepto = new PdfPCell(new Phrase(gastos[i].Concepto, fuente));
            clConcepto.BorderWidth = 0;
            clImporte = new PdfPCell(new Phrase(gastos[i].Importe.ToString(), fuente));
            clImporte.BorderWidth = 0;
            clCategoria = new PdfPCell(new Phrase(gastos[i].Categoria, fuente));
            clCategoria.BorderWidth = 0;
            clCuenta = new PdfPCell(new Phrase(gastos[i].Cuenta, fuente));
            clCuenta.BorderWidth = 0;

            tblGastosTotales.AddCell(clFecha);
            tblGastosTotales.AddCell(clConcepto);
            tblGastosTotales.AddCell(clImporte);
            tblGastosTotales.AddCell(clCategoria);
            tblGastosTotales.AddCell(clCuenta);
        }
    }

    public void GenerarPDFGMensuales(List<Gasto> gastos, string nombreFichero)
    {
        Document doc = new Document(PageSize.LETTER);
        PdfWriter writer = PdfWriter.GetInstance(doc,
            new FileStream(@".\" + nombreFichero + ".pdf",
                FileMode.Create));

        doc.AddTitle("Gastos");
        doc.AddCreator("AR Software");

        doc.Open();
        doc.Add(new Paragraph("Gastos Mensuales"));
        doc.Add(Chunk.NEWLINE);

        tblGastosMensuales = new PdfPTable(2);
        tblGastosMensuales.WidthPercentage = 50;

        InsertarEncabezadoSimple();

        RellenarTablaSimple(gastos);

        doc.Add(tblGastosMensuales);

        doc.Close();
        writer.Close();
    }

    public void InsertarEncabezadoSimple()
    {
        PdfPCell clTitulo1 = new PdfPCell(new Phrase("Campo", fuente));
        clTitulo1.BorderWidth = 0;
        clTitulo1.BorderWidthBottom = 0.75f;

        PdfPCell clTitulo2 = new PdfPCell(new Phrase("Valor", fuente));
        clTitulo2.BorderWidth = 0;
        clTitulo2.BorderWidthBottom = 0.75f;

        tblGastosMensuales.AddCell(clTitulo1);
        tblGastosMensuales.AddCell(clTitulo2);
    }

    public void RellenarTablaSimple(List<Gasto> gastos)
    {
        PdfPCell clCampo;
        PdfPCell clValor;

        for (int i = 0; i < gastos.Count; i++)
        {
            clCampo = new PdfPCell(new Phrase("Fecha", fuente));
            clCampo.BorderWidth = 0;
            clValor = new PdfPCell(new Phrase(gastos[i].ToStringFecha(), 
                fuente));
            clValor.BorderWidth = 0;
            tblGastosMensuales.AddCell(clCampo);
            tblGastosMensuales.AddCell(clValor);
            clCampo = new PdfPCell(new Phrase("Concepto", fuente));
            clCampo.BorderWidth = 0;
            clValor = new PdfPCell(new Phrase(gastos[i].Concepto, 
                fuente));
            clValor.BorderWidth = 0;
            tblGastosMensuales.AddCell(clCampo);
            tblGastosMensuales.AddCell(clValor);
            clCampo = new PdfPCell(new Phrase("Importe", fuente));
            clCampo.BorderWidth = 0;
            clValor = new PdfPCell(new Phrase(gastos[i].Importe.ToString(), 
                fuente));
            clValor.BorderWidth = 0;
            tblGastosMensuales.AddCell(clCampo);
            tblGastosMensuales.AddCell(clValor);
            clCampo = new PdfPCell(new Phrase("Cuenta", fuente));
            clCampo.BorderWidth = 0;
            clValor = new PdfPCell(new Phrase(gastos[i].Cuenta, fuente));
            clValor.BorderWidth = 0;
            tblGastosMensuales.AddCell(clCampo);
            tblGastosMensuales.AddCell(clValor);
            clCampo = new PdfPCell(new Phrase("Categoria", fuente));
            clCampo.BorderWidth = 0;
            clValor = new PdfPCell(new Phrase(gastos[i].Categoria, 
                fuente));
            clValor.BorderWidth = 0;
            tblGastosMensuales.AddCell(clCampo);
            tblGastosMensuales.AddCell(clValor);
            clCampo = new PdfPCell(new Phrase("-----------", fuente));
            clCampo.BorderWidth = 0;
            clValor = new PdfPCell(new Phrase("-----------", fuente));
            clValor.BorderWidth = 0;
            tblGastosMensuales.AddCell(clCampo);
            tblGastosMensuales.AddCell(clValor);
        }
    }

    public void GenerarExcel(List<Gasto> gastos, string nombreFichero)
    {
        IWorkbook libro = new XSSFWorkbook();
        ISheet hojaDeCalculo = libro.CreateSheet("Gastos");

        int indiceFila = 0;
        IRow fila = hojaDeCalculo.CreateRow(indiceFila);
        fila.CreateCell(0).SetCellValue("Fecha");
        fila.CreateCell(1).SetCellValue("Concepto");
        fila.CreateCell(2).SetCellValue("Importe");
        fila.CreateCell(3).SetCellValue("Categoria");
        fila.CreateCell(4).SetCellValue("Cuenta");
        indiceFila++;

        foreach (Gasto g in gastos)
        {
            fila = hojaDeCalculo.CreateRow(indiceFila);
            fila.CreateCell(0).SetCellValue(g.ToStringFecha());
            fila.CreateCell(1).SetCellValue(g.Concepto);
            fila.CreateCell(2).SetCellValue(Math.Round(g.Importe,2));
            fila.CreateCell(3).SetCellValue(g.Categoria);
            fila.CreateCell(4).SetCellValue(g.Cuenta);
            indiceFila++;
        }

        for (int i = 0; i <= gastos.Count; i++)
            hojaDeCalculo.AutoSizeColumn(i);

        using (FileStream fileData = new FileStream(nombreFichero + ".xlsx"
            , FileMode.Create))
        {
            libro.Write(fileData);
        } 
    }
}