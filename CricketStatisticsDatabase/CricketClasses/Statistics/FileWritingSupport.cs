using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cricket.Statistics
{
    public enum HtmlTag
    {
        h1,
        h2,
        h3,
        p
    }

    public static class FileWritingSupport
    {
        public static void WriteParagraph(this StreamWriter writer, ExportType exportType, string[] sentence, HtmlTag tag = HtmlTag.p)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    string toOutput = string.Join(",", sentence);
                    writer.WriteLine(toOutput);
                    break;
                }
                case (ExportType.Html):
                {
                    string toOutput = string.Join(" ", sentence);
                    writer.WriteLine($"<{tag}>{toOutput}</{tag}>");
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public static void WriteTitle(this StreamWriter writer, ExportType exportType, string title, HtmlTag tag = HtmlTag.h1)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    writer.WriteLine("");
                    writer.WriteLine(title);
                    writer.WriteLine("");
                    break;
                }
                case (ExportType.Html):
                {
                    writer.WriteLine($"<{tag}>{title}</{tag}>");
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public static void WriteTable<T>(this StreamWriter writer, ExportType exportType, IEnumerable<T> values)
        {
            T forTypes = default(T);
            foreach (var value in values)
            {
                if (value != null)
                {
                    forTypes = value;
                    break;
                }
            }

            if (forTypes == null)
            {
                return;
            }

            writer.WriteTable(exportType, forTypes.GetType().GetProperties().Select(type => type.Name), values);
        }

        public static void WriteTable<T>(this StreamWriter writer, ExportType exportType, IEnumerable<string> headerValues, IEnumerable<T> rowValues)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    WriteTableHeader(writer, exportType, headerValues);
                    foreach (var value in rowValues)
                    {
                        var row = value.GetType().GetProperties();
                        WriteTableRow(writer, exportType, row.Select(ro => ro.GetValue(value)?.ToString()));
                    }
                    break;
                }
                case (ExportType.Html):
                {
                    writer.WriteLine("<table>");
                    writer.WriteLine("<thead><tr>");
                    WriteTableHeader(writer, exportType, headerValues);
                    writer.WriteLine("</tr></thead>");
                    writer.WriteLine("<tbody>");

                    foreach (var value in rowValues)
                    {
                        if (value != null)
                        {
                            var row = value.GetType().GetProperties();
                            writer.WriteLine("<tr>");
                            WriteTableRow(writer, exportType, row.Select(ro => ro.GetValue(value)?.ToString()));
                            writer.WriteLine("</tr>");
                        }
                    }

                    writer.WriteLine("</tbody>");
                    writer.WriteLine("</table>");
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public static void WriteTableHeader(this StreamWriter writer, ExportType exportType, IEnumerable<string> valuesToWrite)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    string toWrite = string.Join(",", valuesToWrite);
                    writer.WriteLine(toWrite);
                    break;
                }
                case (ExportType.Html):
                {
                    string htmlHeader = "<th scope=\"col\">";
                    int i = 0;
                    foreach (var property in valuesToWrite)
                    {
                        if (i != 0)
                        {
                            htmlHeader += "<th>";
                        }

                        htmlHeader += property;
                        htmlHeader += "</th>";
                        i++;
                    }

                    writer.WriteLine(htmlHeader);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public static void WriteTableRow(this StreamWriter writer, ExportType exportType, IEnumerable<string> valuesToWrite)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    string toWrite = string.Join(",", valuesToWrite);
                    writer.WriteLine(toWrite);
                    break;
                }
                case (ExportType.Html):
                {
                    //string htmlData = "<th scope=\"row\">";
                    string htmlData = string.Empty;
                    int i = 0;
                    foreach (var property in valuesToWrite)
                    {
                        htmlData += "<td>";
                        /*if (i != 0)
                        {
                            htmlData += "<td>";
                        }*/
                        htmlData += property;
                        /*if (i == 0)
                        {
                            htmlData += "</th>";
                        }
                        else
                        {
                            htmlData += "</td>";
                        }
                        i++;*/
                        htmlData += "</td>";
                    }

                    writer.WriteLine(htmlData);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public static void CreateHTMLHeader(this StreamWriter writer, string title)
        {
            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine($"<title>{title}</title>");
            writer.WriteLine("<style>");
            writer.WriteLine("html, h1, h2, h3, h4, h5, h6 {font-family: \"Arial\", cursive, sans-serif; }");
            writer.WriteLine("h1 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
            writer.WriteLine("h2 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
            writer.WriteLine("body{ font-family: \"Arial\", cursive, sans-serif; font-size: 10px }");
            writer.WriteLine("table { border-collapse: collapse;}");
            writer.WriteLine("table, th, td { border: 1px solid black; }");
            writer.WriteLine("caption { margin-bottom: 1.2em; font-family: \"Arial\", cursive, sans-serif; font-size:medium; }");
            writer.WriteLine("tr {text-align: center;}");

            writer.WriteLine("tr:nth-child(even) {background-color: #f0f8ff;}");
            writer.WriteLine("th{ background-color: #ADD8E6; height: 1.5em; }");
            writer.WriteLine("[data-negative] { background-color: red;}");

            writer.WriteLine(" p { line-height: 1.5em; margin-bottom: 1.5em;}");
            writer.WriteLine("</style> ");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
        }

        public static void CreateHTMLFooter(this StreamWriter writer)
        {
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
        }
    }
}
