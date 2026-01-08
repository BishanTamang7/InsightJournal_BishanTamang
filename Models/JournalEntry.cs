namespace InsightJournal.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty; // Comma-separated tags

        // Mood Tracking
        public string PrimaryMood { get; set; } = string.Empty; // Required
        public string? SecondaryMood1 { get; set; } // Optional
        public string? SecondaryMood2 { get; set; } // Optional
        public string MoodCategory { get; set; } = string.Empty; // Positive, Neutral, Negative

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}