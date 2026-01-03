namespace InsightJournal.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty; // Comma-separated tags
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}