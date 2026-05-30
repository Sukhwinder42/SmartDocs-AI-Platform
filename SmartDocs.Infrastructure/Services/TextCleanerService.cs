using SmartDocs.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SmartDocs.Infrastructure.Services
{
    public class TextCleanerService
        : ITextCleanerService
    {
        public string CleanText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // Normalize line endings
            text = text.Replace("\r\n", "\n");

            // Remove tabs
            text = text.Replace("\t", " ");

            // Remove non-ASCII OCR garbage
            text = Regex.Replace(
                text,
                @"[^\u0000-\u007F]+",
                " ");

            // Add missing space after punctuation
            text = Regex.Replace(
                text,
                @"([.,!?])([A-Za-z])",
                "$1 $2");

            // Add space between lowercase-uppercase joins
            text = Regex.Replace(
                text,
                @"([a-z])([A-Z])",
                "$1 $2");

            // Preserve paragraph spacing
            text = Regex.Replace(
                text,
                @"\n{3,}",
                "\n\n");

            // Remove repeated spaces
            text = Regex.Replace(
                text,
                @"[ ]{2,}",
                " ");

            // Clean spaces around newlines
            text = Regex.Replace(
                text,
                @"\s*\n\s*",
                "\n");

            // Fix numbered headings
            text = Regex.Replace(
                text,
                @"(\d+)\.",
                "\n$1.");

            return text.Trim();
        }
    }
}