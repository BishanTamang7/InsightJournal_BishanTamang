using InsightJournal.Models;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace InsightJournal.Services
{
    public class PdfExportService
    {
        private readonly ILogger<PdfExportService> _logger;

        public PdfExportService(ILogger<PdfExportService> logger)
        {
            _logger = logger;

            // Set QuestPDF license (Community license for non-commercial use)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<string> ExportEntriesToPdfAsync(
            List<JournalEntry> entries,
            DateTime startDate,
            DateTime endDate,
            string authorName = "Journal Author")
        {
            if (entries == null)
            {
                _logger.LogWarning("Attempted to export null entries list");
                throw new ArgumentNullException(nameof(entries));
            }

            if (string.IsNullOrWhiteSpace(authorName))
            {
                authorName = "Journal Author";
            }

            try
            {
                _logger.LogInformation("Starting PDF export for {Count} entries from {StartDate} to {EndDate}",
                    entries.Count, startDate, endDate);

                // Generate filename with timestamp
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"Journal_Export_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}_{timestamp}.pdf";

                // Get Downloads folder path
                var downloadsPath = GetDownloadsPath();
                var filePath = Path.Combine(downloadsPath, fileName);

                // Ensure directory exists
                EnsureDirectoryExists(downloadsPath);

                // Sort entries by date descending
                var sortedEntries = entries.OrderByDescending(e => e.Date).ToList();

                // Generate PDF
                await Task.Run(() =>
                {
                    try
                    {
                        GeneratePdfDocument(sortedEntries, startDate, endDate, authorName, filePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during PDF document generation");
                        throw;
                    }
                });

                _logger.LogInformation("Successfully exported PDF to: {FilePath}", filePath);
                return filePath;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Access denied when trying to write PDF to Downloads folder");
                throw new ApplicationException("Permission denied. Unable to save PDF to Downloads folder.", ex);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "IO error during PDF export");
                throw new ApplicationException("Failed to save PDF file. The file may be in use by another program.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during PDF export");
                throw new ApplicationException("Failed to export PDF. Please try again.", ex);
            }
        }

        private string GetDownloadsPath()
        {
            try
            {
                // Try to get Windows Downloads folder
                var downloadsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads"
                );

                // Fallback to Documents if Downloads doesn't exist
                if (!Directory.Exists(downloadsPath))
                {
                    _logger.LogWarning("Downloads folder not found, using Documents folder");
                    downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                return downloadsPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Downloads path");
                // Last resort fallback
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    _logger.LogInformation("Created directory: {Path}", path);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create directory: {Path}", path);
                throw new ApplicationException($"Failed to create directory: {path}", ex);
            }
        }

        private void GeneratePdfDocument(
            List<JournalEntry> sortedEntries,
            DateTime startDate,
            DateTime endDate,
            string authorName,
            string filePath)
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

                            row.ConstantItem(80).AlignRight().Text($"{sortedEntries.Count} Entries")
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
                                RenderEntryInPdf(contentColumn, entry, sortedEntries);
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
        }

        private void RenderEntryInPdf(ColumnDescriptor contentColumn, JournalEntry entry, List<JournalEntry> sortedEntries)
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

                    // Tags (if present)
                    if (!string.IsNullOrWhiteSpace(entry.Tags))
                    {
                        entryColumn.Item().PaddingTop(8);
                        entryColumn.Item().Text($"Tags: {entry.Tags}")
                            .FontSize(9)
                            .Italic()
                            .FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                    }

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

        // Get statistics for export summary
        public ExportStatistics GetExportStatistics(List<JournalEntry> entries)
        {
            if (entries == null || entries.Count == 0)
            {
                _logger.LogInformation("No entries provided for statistics calculation");
                return new ExportStatistics();
            }

            try
            {
                var totalWords = entries.Sum(e => CountWords(e.Content));
                var averageWords = totalWords / entries.Count;

                var stats = new ExportStatistics
                {
                    TotalEntries = entries.Count,
                    TotalWords = totalWords,
                    AverageWordsPerEntry = averageWords,
                    FirstEntryDate = entries.Min(e => e.Date),
                    LastEntryDate = entries.Max(e => e.Date)
                };

                _logger.LogInformation("Calculated statistics: {TotalEntries} entries, {TotalWords} words",
                    stats.TotalEntries, stats.TotalWords);

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating export statistics");
                return new ExportStatistics(); // Return empty stats on error
            }
        }

        private int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            try
            {
                return text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error counting words in text");
                return 0;
            }
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