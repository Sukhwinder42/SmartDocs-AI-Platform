using SmartDocs.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace SmartDocs.Infrastructure.Services
{
    public class OcrService : IOcrService
    {
        public async Task<string> ExtractTextAsync(string filePath)
        {
            var extension =
                Path.GetExtension(filePath).ToLower();

            // PDF extraction
            if (extension == ".pdf")
            {
                return await ExtractPdfTextAsync(filePath);
            }

            if (extension == ".docx")
            {
                return await ExtractDocxTextAsync(filePath);
            }

            // Image OCR
            return await ExtractImageTextAsync(filePath);
        }

        // PDF TEXT EXTRACTION
        private async Task<string> ExtractPdfTextAsync(
            string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var document =
                        PdfDocument.Open(filePath);

                    string text = "";

                    foreach (var page in document.GetPages())
                    {
                        text += page.Text + "\n";
                    }

                    return text;
                }
                catch (Exception ex)
                {
                    return $"PDF Extraction Error: {ex.Message}";
                }
            });
        }

        private async Task<string> ExtractDocxTextAsync(
    string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var document =
                        WordprocessingDocument.Open(
                            filePath,
                            false);

                    var body =
                        document.MainDocumentPart
                            .Document
                            .Body;

                    return body.InnerText;
                }
                catch (Exception ex)
                {
                    return $"DOCX Extraction Error: {ex.Message}";
                }
            });
        }

        // IMAGE OCR
        private async Task<string> ExtractImageTextAsync(
            string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var engine =
                        new TesseractEngine(
                            @"./tessdata",
                            "eng",
                            EngineMode.Default);

                    using var img =
                        Pix.LoadFromFile(filePath);

                    using var page =
                        engine.Process(img);

                    return page.GetText();
                }
                catch (Exception ex)
                {
                    return $"OCR Error: {ex.Message}";
                }
            });
        }
    }
}