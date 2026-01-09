using InsightJournal.Data;
using InsightJournal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InsightJournal.Services
{
    public class JournalService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<JournalService> _logger;

        public JournalService(AppDbContext context, ILogger<JournalService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get entry for specific date
        public async Task<JournalEntry?> GetEntryByDateAsync(DateTime date)
        {
            try
            {
                var dateOnly = date.Date;
                _logger.LogInformation("Fetching entry for date: {Date}", dateOnly);

                return await _context.JournalEntries
                    .FirstOrDefaultAsync(e => e.Date.Date == dateOnly);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching entry for date: {Date}", date.Date);
                throw new ApplicationException("Failed to retrieve entry. Please try again.", ex);
            }
        }

        // Get today's entry
        public async Task<JournalEntry?> GetTodayEntryAsync()
        {
            return await GetEntryByDateAsync(DateTime.Today);
        }

        // Create new entry with mood and manual category
        public async Task<JournalEntry> CreateEntryAsync(
            string title,
            string content,
            List<string> tags,
            string primaryMood,
            string? secondaryMood1 = null,
            string? secondaryMood2 = null,
            string? manualCategory = null) // NEW: Optional manual category
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                _logger.LogWarning("Attempted to create entry with empty title");
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning("Attempted to create entry with empty content");
                throw new ArgumentException("Content cannot be empty.", nameof(content));
            }

            if (string.IsNullOrWhiteSpace(primaryMood))
            {
                _logger.LogWarning("Attempted to create entry without primary mood");
                throw new ArgumentException("Primary mood is required.", nameof(primaryMood));
            }

            if (!MoodService.IsValidMood(primaryMood))
            {
                _logger.LogWarning("Invalid primary mood: {Mood}", primaryMood);
                throw new ArgumentException($"Invalid mood: {primaryMood}", nameof(primaryMood));
            }

            try
            {
                var today = DateTime.Today;

                // Check if entry exists for today
                var existing = await GetTodayEntryAsync();
                if (existing != null)
                {
                    _logger.LogWarning("Attempted to create duplicate entry for today");
                    throw new InvalidOperationException("You already have an entry for today!");
                }

                // Use manual category if provided, otherwise auto-calculate
                string moodCategory = !string.IsNullOrWhiteSpace(manualCategory)
                    ? manualCategory
                    : MoodService.GetMoodCategory(primaryMood);

                var entry = new JournalEntry
                {
                    Date = today,
                    Title = title.Trim(),
                    Content = content.Trim(),
                    Tags = TagService.TagsToString(tags),
                    PrimaryMood = primaryMood,
                    SecondaryMood1 = secondaryMood1,
                    SecondaryMood2 = secondaryMood2,
                    MoodCategory = moodCategory, // Use manual or auto-calculated
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.JournalEntries.Add(entry);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created entry with ID: {EntryId}, mood: {Mood}, category: {Category}",
                    entry.Id, primaryMood, moodCategory);
                return entry;
            }
            catch (InvalidOperationException)
            {
                throw; // Re-throw business logic exceptions
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating journal entry");
                throw new ApplicationException("Failed to create entry. Please try again.", ex);
            }
        }

        // Update entry with mood and manual category
        public async Task<JournalEntry> UpdateEntryAsync(
            int id,
            string title,
            string content,
            List<string> tags,
            string primaryMood,
            string? secondaryMood1 = null,
            string? secondaryMood2 = null,
            string? manualCategory = null) // NEW: Optional manual category
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                _logger.LogWarning("Attempted to update entry {EntryId} with empty title", id);
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning("Attempted to update entry {EntryId} with empty content", id);
                throw new ArgumentException("Content cannot be empty.", nameof(content));
            }

            if (string.IsNullOrWhiteSpace(primaryMood))
            {
                _logger.LogWarning("Attempted to update entry {EntryId} without primary mood", id);
                throw new ArgumentException("Primary mood is required.", nameof(primaryMood));
            }

            if (!MoodService.IsValidMood(primaryMood))
            {
                _logger.LogWarning("Invalid primary mood: {Mood}", primaryMood);
                throw new ArgumentException($"Invalid mood: {primaryMood}", nameof(primaryMood));
            }

            try
            {
                var entry = await _context.JournalEntries.FindAsync(id);
                if (entry == null)
                {
                    _logger.LogWarning("Entry not found with ID: {EntryId}", id);
                    throw new KeyNotFoundException($"Entry with ID {id} not found!");
                }

                entry.Title = title.Trim();
                entry.Content = content.Trim();
                entry.Tags = TagService.TagsToString(tags);
                entry.PrimaryMood = primaryMood;
                entry.SecondaryMood1 = secondaryMood1;
                entry.SecondaryMood2 = secondaryMood2;

                // Use manual category if provided, otherwise auto-calculate
                entry.MoodCategory = !string.IsNullOrWhiteSpace(manualCategory)
                    ? manualCategory
                    : MoodService.GetMoodCategory(primaryMood);

                entry.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated entry with ID: {EntryId}", id);
                return entry;
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw business logic exceptions
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entry with ID: {EntryId}", id);
                throw new ApplicationException("Failed to update entry. Please try again.", ex);
            }
        }

        // Delete entry
        public async Task DeleteEntryAsync(int id)
        {
            try
            {
                var entry = await _context.JournalEntries.FindAsync(id);
                if (entry == null)
                {
                    _logger.LogWarning("Attempted to delete non-existent entry with ID: {EntryId}", id);
                    throw new KeyNotFoundException($"Entry with ID {id} not found!");
                }

                _context.JournalEntries.Remove(entry);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted entry with ID: {EntryId}", id);
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw business logic exceptions
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entry with ID: {EntryId}", id);
                throw new ApplicationException("Failed to delete entry. Please try again.", ex);
            }
        }

        // Get all entries (optimized with optional filters)
        public async Task<List<JournalEntry>> GetAllEntriesAsync(
            string? searchText = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? tag = null,
            string? moodCategory = null)
        {
            try
            {
                _logger.LogInformation("Fetching entries with filters - Search: {Search}, StartDate: {StartDate}, EndDate: {EndDate}, Tag: {Tag}, MoodCategory: {MoodCategory}",
                    searchText, startDate, endDate, tag, moodCategory);

                var query = _context.JournalEntries.AsQueryable();

                // Apply filters at database level for efficiency
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(e => e.Title.Contains(searchText) || e.Content.Contains(searchText));
                }

                if (startDate.HasValue)
                {
                    query = query.Where(e => e.Date.Date >= startDate.Value.Date);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(e => e.Date.Date <= endDate.Value.Date);
                }

                if (!string.IsNullOrWhiteSpace(tag))
                {
                    query = query.Where(e => e.Tags.Contains(tag));
                }

                if (!string.IsNullOrWhiteSpace(moodCategory))
                {
                    query = query.Where(e => e.MoodCategory == moodCategory);
                }

                var entries = await query.OrderByDescending(e => e.Date).ToListAsync();

                _logger.LogInformation("Successfully fetched {Count} entries", entries.Count);
                return entries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching journal entries");
                throw new ApplicationException("Failed to retrieve entries. Please try again.", ex);
            }
        }

        // Get entries by tag (optimized)
        public async Task<List<JournalEntry>> GetEntriesByTagAsync(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                _logger.LogWarning("Attempted to fetch entries with empty tag");
                throw new ArgumentException("Tag cannot be empty.", nameof(tag));
            }

            return await GetAllEntriesAsync(tag: tag);
        }

        // Get entries by mood category
        public async Task<List<JournalEntry>> GetEntriesByMoodCategoryAsync(string moodCategory)
        {
            if (string.IsNullOrWhiteSpace(moodCategory))
            {
                _logger.LogWarning("Attempted to fetch entries with empty mood category");
                throw new ArgumentException("Mood category cannot be empty.", nameof(moodCategory));
            }

            return await GetAllEntriesAsync(moodCategory: moodCategory);
        }

        // Get entries count for analytics
        public async Task<int> GetEntriesCountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var query = _context.JournalEntries.AsQueryable();

                if (startDate.HasValue)
                {
                    query = query.Where(e => e.Date.Date >= startDate.Value.Date);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(e => e.Date.Date <= endDate.Value.Date);
                }

                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting entries");
                return 0; // Graceful degradation
            }
        }

        // Get entries grouped by month for analytics
        public async Task<Dictionary<string, int>> GetEntriesByMonthAsync(int year)
        {
            try
            {
                var entries = await _context.JournalEntries
                    .Where(e => e.Date.Year == year)
                    .GroupBy(e => e.Date.Month)
                    .Select(g => new { Month = g.Key, Count = g.Count() })
                    .ToListAsync();

                var result = new Dictionary<string, int>();
                for (int i = 1; i <= 12; i++)
                {
                    var monthName = new DateTime(year, i, 1).ToString("MMM");
                    var count = entries.FirstOrDefault(e => e.Month == i)?.Count ?? 0;
                    result[monthName] = count;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching entries by month for year {Year}", year);
                return new Dictionary<string, int>(); // Graceful degradation
            }
        }

        // Get mood distribution for analytics
        public async Task<Dictionary<string, int>> GetMoodDistributionAsync()
        {
            try
            {
                var entries = await _context.JournalEntries.ToListAsync();
                var distribution = new Dictionary<string, int>
                {
                    { "Positive", 0 },
                    { "Neutral", 0 },
                    { "Negative", 0 }
                };

                foreach (var entry in entries)
                {
                    if (!string.IsNullOrWhiteSpace(entry.MoodCategory) && distribution.ContainsKey(entry.MoodCategory))
                    {
                        distribution[entry.MoodCategory]++;
                    }
                }

                return distribution;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting mood distribution");
                return new Dictionary<string, int>
                {
                    { "Positive", 0 },
                    { "Neutral", 0 },
                    { "Negative", 0 }
                };
            }
        }

        // Get most frequent mood
        public async Task<string?> GetMostFrequentMoodAsync()
        {
            try
            {
                var entries = await _context.JournalEntries.ToListAsync();

                if (!entries.Any())
                    return null;

                var moodCounts = entries
                    .Where(e => !string.IsNullOrWhiteSpace(e.PrimaryMood))
                    .GroupBy(e => e.PrimaryMood)
                    .Select(g => new { Mood = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .FirstOrDefault();

                return moodCounts?.Mood;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting most frequent mood");
                return null;
            }
        }
    }
}