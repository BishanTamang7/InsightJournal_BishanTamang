using System.Collections.Generic;
using System.Linq;

namespace InsightJournal.Services
{
    public class TagService
    {
        // Pre-built tags
        public static readonly List<string> PredefinedTags = new()
        {
            "Work",
            "Personal",
            "Health",
            "Fitness",
            "Travel",
            "Family",
            "Friends",
            "Goals",
            "Learning",
            "Reflection",
            "Gratitude",
            "Ideas",
            "Projects",
            "Finance",
            "Hobbies"
        };

        // Parse tags from comma-separated string
        public static List<string> ParseTags(string tagsString)
        {
            if (string.IsNullOrWhiteSpace(tagsString))
                return new List<string>();

            return tagsString
                .Split(',')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();
        }

        // Convert list of tags to comma-separated string
        public static string TagsToString(List<string> tags)
        {
            return string.Join(", ", tags);
        }

        // Get tag color based on tag name
        public static string GetTagColor(string tag)
        {
            return tag switch
            {
                "Work" => "#3b82f6",      // Blue
                "Personal" => "#8b5cf6",   // Purple
                "Health" => "#10b981",     // Green
                "Fitness" => "#f59e0b",    // Orange
                "Travel" => "#06b6d4",     // Cyan
                "Family" => "#ec4899",     // Pink
                "Friends" => "#f97316",    // Orange
                "Goals" => "#eab308",      // Yellow
                "Learning" => "#6366f1",   // Indigo
                "Reflection" => "#8b5cf6", // Purple
                "Gratitude" => "#10b981",  // Green
                "Ideas" => "#f59e0b",      // Amber
                "Projects" => "#0ea5e9",   // Sky
                "Finance" => "#22c55e",    // Green
                "Hobbies" => "#a855f7",    // Purple
                _ => "#6b7280"             // Gray (default)
            };
        }
    }
}