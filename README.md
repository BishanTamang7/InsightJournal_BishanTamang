# Insight Journal

A secure, feature-rich desktop journaling application built with C#.NET that helps you track your thoughts, moods, and personal growth through intelligent analytics and an intuitive interface.

## 📋 Project Overview

**Insight Journal** is a modern desktop application designed to transform the traditional journaling experience. It provides a comprehensive platform for daily reflection, mood tracking, and personal analytics, all while maintaining security and privacy through local data storage.

### Purpose
- Enable consistent daily journaling with rich-text formatting
- Track emotional patterns through mood analytics
- Provide insights into personal growth and habits
- Maintain privacy with local, encrypted storage

### Scope
- Single daily journal entry management (CRUD operations)
- Rich-text/Markdown content support
- Comprehensive mood and tag tracking system
- Advanced search, filter, and navigation capabilities
- Analytics dashboard with visualizations
- Streak tracking for consistency motivation
- Secure password/PIN protection
- PDF export functionality

## ✨ Key Features

### 📝 Journal Entry Management
- **One Entry Per Day**: Create, update, or delete a single entry per day
- **System Timestamps**: Automatic tracking of creation and update times
- **Rich Text Editor**: Support for bold, italics, lists, headings, and links
- **Markdown Support**: Write in Markdown for faster formatting

### 😊 Mood Tracking
- **Primary Mood**: Required selection from 15 mood options
- **Secondary Moods**: Optional selection of up to 2 additional moods
- **Mood Categories**:
  - **Positive**: Happy, Excited, Relaxed, Grateful, Confident
  - **Neutral**: Calm, Thoughtful, Curious, Nostalgic, Bored
  - **Negative**: Sad, Angry, Stressed, Lonely, Anxious

### 🏷️ Tagging & Organization
- **Custom Tags**: Create your own tags for personalized organization
- **Pre-built Tags**: 30+ predefined tags including:
  - Work, Career, Studies
  - Family, Friends, Relationships
  - Health, Fitness, Exercise
  - Travel, Nature, Hobbies
  - And many more...
- **Categories**: Organize entries by custom categories

### 🔍 Search & Filter
- Search by title or content
- Filter by date range
- Filter by mood(s)
- Filter by tags
- Combined filters for precise results

### 📅 Navigation
- **Calendar View**: Visual navigation through entries by date
- **Paginated Timeline**: Browse entries in chronological order
- **Quick Date Jump**: Navigate to specific dates instantly

### 🔥 Streak Tracking
- **Daily Streak**: Current consecutive days of journaling
- **Longest Streak**: Your personal best
- **Missed Days**: Track gaps in your journaling habit

### 📊 Analytics Dashboard
- **Mood Distribution**: Pie/bar charts showing positive, neutral, and negative mood percentages
- **Most Frequent Mood**: Identify your most common emotional state
- **Most Used Tags**: Visualize your journaling themes
- **Tag Breakdown**: Percentage distribution across categories
- **Word Count Trends**: Track writing patterns over time
- **Date Range Filtering**: Analyze any time period

### 🔒 Security & Privacy
- Password/PIN protection
- Local SQLite database storage
- No cloud synchronization (your data stays on your device)

### 📤 Export Functionality
- Export entries as PDF
- Date range selection
- Formatted output with moods and tags

### 🎨 Theme Customization
- Light/Dark mode toggle
- Custom theme options
- Consistent, user-friendly interface

## 🛠️ Technology Stack

### Framework
- **C#.NET** (Core Framework)
- **.NET MAUI / WinForms / WPF** (UI Framework)

### Database
- **SQLite** - Local database for secure data storage

### Libraries & Packages
- **Newtonsoft.Json** - JSON serialization
- **MudBlazor / MaterialDesignInXaml** - UI components
- **LiveCharts / OxyPlot** - Data visualization
- **MarkDig** - Markdown parsing
- **iTextSharp / PdfSharp** - PDF generation
- **BCrypt.Net** - Password hashing

## 📦 Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd insight-journal
```

2. Open the solution in Visual Studio 2022 or later

3. Restore NuGet packages:
```bash
dotnet restore
```

4. Build the project:
```bash
dotnet build
```

5. Run the application:
```bash
dotnet run
```

## 💻 Usage

### First Launch
1. Set up your password/PIN for security
2. Create your first journal entry
3. Select your mood and add tags
4. Start building your journaling streak!

### Daily Journaling
1. Click "New Entry" or navigate to today's date
2. Write your thoughts using the rich-text editor
3. Select your primary mood and optional secondary moods
4. Add relevant tags to categorize your entry
5. Save your entry - timestamps are automatically recorded

### Viewing Analytics
1. Navigate to the Dashboard
2. Select a date range for analysis
3. View mood distribution, streak statistics, and tag insights
4. Export data as PDF if needed

## 📁 Project Structure

```
InsightJournal/
├── Models/              # Data models (Entry, Mood, Tag, User)
├── ViewModels/          # MVVM ViewModels
├── Views/               # UI pages and components
├── Services/            # Business logic and data services
├── Data/                # Database context and migrations
├── Helpers/             # Utility classes
├── Assets/              # Images, icons, and resources
└── Exports/             # PDF export output directory
```

## 🔒 Security Features

- Local-only data storage (no cloud sync)
- Password protection with BCrypt hashing
- SQLite database encryption
- No telemetry or data collection

## 🚀 Future Enhancements

- Cloud backup option (optional)
- Mobile companion app
- Voice-to-text entry
- Image attachments
- Custom mood creation
- Journaling prompts and suggestions
- Multi-language support
- Advanced analytics with AI insights

## 📝 Documentation

Complete documentation including:
- Architecture diagrams
- Database schema (ERD)
- API documentation
- User manual
- Development guide

## 🤝 Contributing

This is an academic project developed as coursework for **CS6004NT - Application Development** at London Metropolitan University.

**Module Leader**: Mr. Bikram Poudel (Islington College)

## 📄 License

This project is developed for educational purposes as part of university coursework.

## ⚠️ Academic Integrity

This project adheres to London Metropolitan University's academic integrity policies. All code is original work with proper attribution for any external libraries or resources used.

## 👤 Author

**[Your Name]**  
**London Met ID**: [Your ID]  
**Institution**: Itahari International College  
**Year**: 2025-2026

## 📞 Support

For issues or questions related to this project, please refer to the module documentation or contact the module leader.

---

**Built with 💙 using C#.NET**

*Insight Journal - Your thoughts, your journey, your insights.*
