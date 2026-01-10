namespace InsightJournal.Services
{
    public class MoodService
    {
        // Mood categories and their associated moods
        public static readonly Dictionary<string, List<string>> MoodsByCategory = new()
        {
            {
                "Positive", new List<string>
                {
                    "Happy", "Excited", "Relaxed", "Grateful", "Confident"
                }
            },
            {
                "Neutral", new List<string>
                {
                    "Calm", "Thoughtful", "Curious", "Nostalgic", "Bored"
                }
            },
            {
                "Negative", new List<string>
                {
                    "Sad", "Angry", "Stressed", "Lonely", "Anxious"
                }
            }
        };

        // Flat list of all moods
        public static List<string> AllMoods => MoodsByCategory.Values.SelectMany(m => m).ToList();

        // Get category for a specific mood
        public static string GetMoodCategory(string mood)
        {
            foreach (var category in MoodsByCategory)
            {
                if (category.Value.Contains(mood))
                {
                    return category.Key;
                }
            }
            return "Neutral"; // Default fallback
        }

        // Get color for mood based on category
        public static string GetMoodColor(string mood)
        {
            var category = GetMoodCategory(mood);
            return category switch
            {
                "Positive" => "#10b981", // Green
                "Neutral" => "#6b7280",  // Gray
                "Negative" => "#ef4444", // Red
                _ => "#6b7280"
            };
        }

        // Get emoji for mood
        public static string GetMoodEmoji(string mood)
        {
            return mood switch
            {
                // Positive
                "Happy" => "😊",
                "Excited" => "🤩",
                "Relaxed" => "😌",
                "Grateful" => "🙏",
                "Confident" => "💪",

                // Neutral
                "Calm" => "😐",
                "Thoughtful" => "🤔",
                "Curious" => "🧐",
                "Nostalgic" => "🥺",
                "Bored" => "😑",

                // Negative
                "Sad" => "😢",
                "Angry" => "😠",
                "Stressed" => "😰",
                "Lonely" => "😔",
                "Anxious" => "😟",

                _ => "😐"
            };
        }

        // Get background color for mood
        public static string GetMoodBackgroundColor(string mood)
        {
            var category = GetMoodCategory(mood);
            return category switch
            {
                "Positive" => "rgba(16, 185, 129, 0.1)", // Light green
                "Neutral" => "rgba(107, 114, 128, 0.1)",  // Light gray
                "Negative" => "rgba(239, 68, 68, 0.1)",   // Light red
                _ => "rgba(107, 114, 128, 0.1)"
            };
        }

        // Get category color
        public static string GetCategoryColor(string category)
        {
            return category switch
            {
                "Positive" => "#10b981", // Green
                "Neutral" => "#6b7280",  // Gray
                "Negative" => "#ef4444", // Red
                _ => "#6b7280"
            };
        }

        // Get category emoji
        public static string GetCategoryEmoji(string category)
        {
            return category switch
            {
                "Positive" => "😊",
                "Neutral" => "😐",
                "Negative" => "😢",
                _ => "😐"
            };
        }

        // Validate mood selection
        public static bool IsValidMood(string mood)
        {
            return AllMoods.Contains(mood);
        }

        // Get list of secondary moods (excluding primary and already selected secondary)
        public static List<string> GetAvailableSecondaryMoods(string primaryMood, string? secondaryMood1)
        {
            var moods = AllMoods.ToList();
            moods.Remove(primaryMood);

            if (!string.IsNullOrWhiteSpace(secondaryMood1))
            {
                moods.Remove(secondaryMood1);
            }

            return moods;
        }
    }
}