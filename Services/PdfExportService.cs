using InsightJournal.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace InsightJournal.Services
{
    public class PdfExportService
    {
        public PdfExportService()
        {
            // Set QuestPDF license (Community license for non-commercial use)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<string> ExportEntriesToPdfAsync(
            List<JournalEntry> entries,
            DateTime startDate,
            DateTime endDate,
            string authorName = "Journal Author")
        {
            // Generate filename with timestamp
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"Journal_Export_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}_{timestamp}.pdf";

            // Windows Downloads folder
            var downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads"
            );
            var filePath = Path.Combine(downloadsPath, fileName);

            // Ensure directory exists
            if (!Directory.Exists(downloadsPath))
            {
                Directory.CreateDirectory(downloadsPath);
            }

            // Sort entries by date descending
            var sortedEntries = entries.OrderByDescending(e => e.Date).ToList();

            // Generate PDF
            await Task.Run(() =>
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(QuestPDF.Helpers.Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(11));

                        // Header
                        page.Header().Column(column =>
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text("InsightJournal Export")
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);

                                    col.Item().Text($"Author: {authorName}")
                                        .FontSize(10)
                                        .FontColor(QuestPDF.Helpers.Colors.Grey.Darken1);

                                    col.Item().Text($"Export Date: {DateTime.Now:MMMM dd, yyyy}")
                                        .FontSize(10)
                                        .FontColor(QuestPDF.Helpers.Colors.Grey.Darken1);

                                    col.Item().Text($"Period: {startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}")
                                        .FontSize(10)
                                        .FontColor(QuestPDF.Helpers.Colors.Grey.Darken1);
                                });

                                row.ConstantItem(80).AlignRight().Text($"{entries.Count} Entries")
                                    .FontSize(12)
                                    .Bold()
                                    .FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            });

                            column.Item().PaddingTop(10).BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1);
                        });

                        // Content
                        page.Content().PaddingVertical(20).Column(contentColumn =>
                        {
                            if (sortedEntries.Count == 0)
                            {
                                contentColumn.Item().AlignCenter().PaddingVertical(50).Text("No entries found for the selected date range.")
                                    .FontSize(14)
                                    .Italic()
                                    .FontColor(QuestPDF.Helpers.Colors.Grey.Medium);
                            }
                            else
                            {
                                foreach (var entry in sortedEntries)
                                {
                                    contentColumn.Item().Border(1)
                                        .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                                        .Padding(15)
                                        .Column(entryColumn =>
                                        {
                                            // Entry Date Badge
                                            entryColumn.Item().Background(QuestPDF.Helpers.Colors.Blue.Lighten3)
                                                .Padding(8)
                                                .Text(entry.Date.ToString("dddd, MMMM dd, yyyy"))
                                                .FontSize(10)
                                                .Bold()
                                                .FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);

                                            entryColumn.Item().PaddingTop(10);

                                            // Entry Title
                                            entryColumn.Item().Text(entry.Title)
                                                .FontSize(16)
                                                .Bold()
                                                .FontColor(QuestPDF.Helpers.Colors.Black);

                                            entryColumn.Item().PaddingTop(8);

                                            // Entry Content
                                            entryColumn.Item().Text(entry.Content)
                                                .FontSize(11)
                                                .LineHeight(1.5f)
                                                .FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);

                                            entryColumn.Item().PaddingTop(10);

                                            // Entry Metadata Footer
                                            entryColumn.Item().BorderTop(1)
                                                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                                                .PaddingTop(8)
                                                .Row(footerRow =>
                                                {
                                                    footerRow.RelativeItem().Text($"Created: {entry.CreatedAt:MMM dd, yyyy h:mm tt}")
                                                        .FontSize(9)
                                                        .Italic()
                                                        .FontColor(QuestPDF.Helpers.Colors.Grey.Medium);

                                                    if (entry.UpdatedAt != entry.CreatedAt)
                                                    {
                                                        footerRow.RelativeItem().AlignRight().Text($"Updated: {entry.UpdatedAt:MMM dd, yyyy h:mm tt}")
                                                            .FontSize(9)
                                                            .Italic()
                                                            .FontColor(QuestPDF.Helpers.Colors.Grey.Medium);
                                                    }
                                                });
                                        });

                                    // Add spacing between entries
                                    if (entry != sortedEntries.Last())
                                    {
                                        contentColumn.Item().PaddingVertical(5);
                                    }
                                }
                            }
                        });

                        // Footer
                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                            text.Span(" of ");
                            text.TotalPages();
                        });
                    });
                })
                .GeneratePdf(filePath);
            });

            return filePath;
        }

        // Get statistics for export summary
        public ExportStatistics GetExportStatistics(List<JournalEntry> entries)
        {
            if (entries.Count == 0)
            {
                return new ExportStatistics();
            }

            var totalWords = entries.Sum(e => CountWords(e.Content));
            var averageWords = totalWords / entries.Count;

            return new ExportStatistics
            {
                TotalEntries = entries.Count,
                TotalWords = totalWords,
                AverageWordsPerEntry = averageWords,
                FirstEntryDate = entries.Min(e => e.Date),
                LastEntryDate = entries.Max(e => e.Date)
            };
        }

        private int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            return text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }

    public class ExportStatistics
    {
        public int TotalEntries { get; set; }
        public int TotalWords { get; set; }
        public int AverageWordsPerEntry { get; set; }
        public DateTime FirstEntryDate { get; set; }
        public DateTime LastEntryDate { get; set; }
    }
}