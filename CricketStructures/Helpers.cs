using Common.Structure.ReportWriting;

namespace CricketStructures
{
    internal class Helpers
    {

        public static string GetTitle(DocumentType docType, string document, DocumentElement element)
        {
            switch (docType)
            {
                case DocumentType.Html:
                {
                    int startIndex = document.IndexOf($"<{element}>");
                    int endIndex = document.IndexOf($"</{element}>");
                    int start = startIndex > endIndex ? 0 : startIndex + 2 + element.ToString().Length;
                    int end = endIndex - start;
                    return document.Substring(start, end);
                }
                case DocumentType.Md:
                {
                    var tagString = HtmlTagToMdTitle(element);
                    int startIndex = document.IndexOf(tagString);
                    int endIndex = document.IndexOf("\r\n");
                    int start = startIndex > endIndex ? 0 : startIndex + 1 + tagString.ToString().Length;
                    int end = endIndex + 2 - start;
                    return document.Substring(start, end);
                }
                default:
                    return null;
            }
        }

        public static string HtmlTagToMdTitle(DocumentElement tag)
        {
            switch (tag)
            {
                case DocumentElement.h1:
                    return "#";
                case DocumentElement.h2:
                    return "##";
                case DocumentElement.h3:
                    return "###";
                case DocumentElement.p:
                default:
                    return "";
            }
        }
    }
}