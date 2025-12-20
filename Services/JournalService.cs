using InsightJournal.Data;
using InsightJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace InsightJournal.Services
{
    public class JournalService
    {
        private readonly AppDbContext _context;

        public JournalService(AppDbContext context)
        {
            _context = context;
        }

        // Get entry for specific date
        public async Task<JournalEntry?> GetEntryByDateAsync(DateTime date)
        {
            var dateOnly = date.Date;
            return await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Date.Date == dateOnly);
        }

        // Get today's entry
        public async Task<JournalEntry?> GetTodayEntryAsync()
        {
            return await GetEntryByDateAsync(DateTime.Today);
        }

        // Create new entry
        public async Task<JournalEntry> CreateEntryAsync(string title, string content)
        {
            var today = DateTime.Today;

            // Check if entry exists for today
            var existing = await GetTodayEntryAsync();
            if (existing != null)
            {
                throw new Exception("You already have an entry for today!");
            }

            var entry = new JournalEntry
            {
                Date = today,
                Title = title,
                Content = content,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();

            return entry;
        }

        // Update entry
        public async Task<JournalEntry> UpdateEntryAsync(int id, string title, string content)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
            {
                throw new Exception("Entry not found!");
            }

            entry.Title = title;
            entry.Content = content;
            entry.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return entry;
        }

        // Delete entry
        public async Task DeleteEntryAsync(int id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
            {
                throw new Exception("Entry not found!");
            }

            _context.JournalEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }

        // Get all entries
        public async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            return await _context.JournalEntries
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }
    }
}