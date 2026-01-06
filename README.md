<div align="center">

# 📔 Insight Journal

### *Your thoughts, your journey, your insights.*

[![C#](https://img.shields.io/badge/C%23-.NET-512BD4?style=for-the-badge&logo=csharp&logoColor=white)](https://dotnet.microsoft.com/)
[![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)](https://www.sqlite.org/)
[![License](https://img.shields.io/badge/License-Academic-blue?style=for-the-badge)](LICENSE)

**A secure, feature-rich desktop journaling application that helps you track your thoughts, moods, and personal growth through intelligent analytics.**

[Features](#-features) • [Installation](#-installation) • [Usage](#-usage) • [Tech Stack](#️-tech-stack) • [Screenshots](#-screenshots)

</div>

---

## 🌟 Overview

**Insight Journal** is a modern desktop application designed to transform the traditional journaling experience. It provides a comprehensive platform for daily reflection, mood tracking, and personal analytics, all while maintaining security and privacy through local data storage.

### 🎯 Purpose

```
✓ Enable consistent daily journaling with rich-text formatting
✓ Track emotional patterns through mood analytics  
✓ Provide insights into personal growth and habits
✓ Maintain privacy with local, encrypted storage
```

### 📊 Project Scope

| Feature Category | Description |
|-----------------|-------------|
| **Entry Management** | Single daily journal entry with full CRUD operations |
| **Content Support** | Rich-text/Markdown formatting with live preview |
| **Mood Tracking** | Primary + secondary mood selection (15 moods total) |
| **Organization** | Custom tags, categories, and pre-built tag system |
| **Navigation** | Calendar view + paginated timeline |
| **Analytics** | Dashboard with mood distribution, trends, and insights |
| **Security** | Password/PIN protection with local encryption |
| **Export** | PDF generation with date range filtering |

---

## ✨ Features

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

---

## 🛠️ Tech Stack

<div align="center">

### Core Technologies

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)](https://www.sqlite.org/)

</div>

| Category | Technologies |
|----------|-------------|
| **Framework** | C#.NET (Core) • .NET MAUI / WinForms / WPF |
| **Database** | SQLite (Local Storage) |
| **UI Components** | MudBlazor / MaterialDesignInXaml |
| **Visualization** | LiveCharts / OxyPlot |
| **Markdown** | MarkDig (Parser & Renderer) |
| **PDF Export** | iTextSharp / PdfSharp |
| **Security** | BCrypt.Net (Password Hashing) |
| **JSON** | Newtonsoft.Json |

---

## 📦 Installation

### Prerequisites
- Visual Studio 2022 or later
- .NET 6.0 SDK or higher
- Windows 10/11 (for desktop deployment)

### Steps

```bash
# 1. Clone the repository
git clone https://github.com/BishanTamang7/InsightJournal_BishanTamang.git
cd InsightJournal_BishanTamang

# 2. Restore NuGet packages
dotnet restore

# 3. Build the project
dotnet build

# 4. Run the application
dotnet run
```

### Visual Studio
1. Open `InsightJournal.sln` in Visual Studio
2. Right-click on the solution → **Restore NuGet Packages**
3. Press `F5` or click **Start** to run

---

## 💻 Usage

### 🚀 First Launch

```
1️⃣ Set up your password/PIN for security
2️⃣ Create your first journal entry
3️⃣ Select your mood and add tags
4️⃣ Start building your journaling streak!
```

### ✍️ Daily Journaling

<table>
<tr>
<td width="50%">

**Creating an Entry**
1. Click **"New Entry"** or navigate to today
2. Write using the rich-text editor
3. Select primary + secondary moods
4. Add relevant tags
5. Save (auto-timestamps recorded)

</td>
<td width="50%">

**Viewing Analytics**
1. Navigate to **Dashboard**
2. Select a date range
3. View mood distribution & trends
4. Analyze streak statistics
5. Export as PDF if needed

</td>
</tr>
</table>

---

## 📁 Project Structure

```
InsightJournal/
│
├── 📂 Models/              # Data models (Entry, Mood, Tag, User)
├── 📂 ViewModels/          # MVVM ViewModels
├── 📂 Views/               # UI pages and components
├── 📂 Services/            # Business logic and data services
├── 📂 Data/                # Database context and migrations
├── 📂 Helpers/             # Utility classes
├── 📂 Assets/              # Images, icons, and resources
├── 📂 Exports/             # PDF export output directory
├── 📄 README.md
└── 📄 InsightJournal.sln
```

---

## 📸 Screenshots

> *Coming soon - Application screenshots will be added here*

---

## 🔒 Security Features

| Feature | Description |
|---------|-------------|
| 🔐 **Password Protection** | BCrypt hashing for secure authentication |
| 💾 **Local Storage** | All data stored locally (no cloud sync) |
| 🔒 **Database Encryption** | SQLite database with encryption |
| 🚫 **No Telemetry** | Zero data collection or tracking |

---

## 🚀 Future Enhancements

<details>
<summary>Click to expand roadmap</summary>

- [ ] ☁️ Optional cloud backup
- [ ] 📱 Mobile companion app (iOS/Android)
- [ ] 🎤 Voice-to-text entry
- [ ] 🖼️ Image attachments support
- [ ] 😊 Custom mood creation
- [ ] 💡 AI-powered journaling prompts
- [ ] 🌍 Multi-language support
- [ ] 🤖 Advanced AI insights & sentiment analysis
- [ ] 📊 Export to more formats (Word, JSON)
- [ ] 🔗 Social sharing (anonymized)

</details>

---

## 📚 Documentation

| Document | Description |
|----------|-------------|
| 📐 **Architecture Diagrams** | System design and component interactions |
| 🗄️ **Database Schema (ERD)** | Entity relationships and data models |
| 📖 **API Documentation** | Service layer and method references |
| 👤 **User Manual** | Step-by-step usage guide |
| 💻 **Development Guide** | Setup and contribution guidelines |

---

## 🎓 Academic Project

This project is developed as coursework for:

**Module**: CS6004NT - Application Development  
**Institution**: London Metropolitan University / Itahari International College  
**Module Leader**: Mr. Bikram Poudel (Islington College)  
**Academic Year**: 2025-2026

### ⚠️ Academic Integrity

This project adheres to London Metropolitan University's academic integrity policies. All code is original work with proper attribution for any external libraries or resources used.

---

## 👤 Author

<div align="center">

**Bishan Tamang**  
*Student Developer*

[![GitHub](https://img.shields.io/badge/GitHub-BishanTamang7-181717?style=for-the-badge&logo=github)](https://github.com/BishanTamang7)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Connect-0A66C2?style=for-the-badge&logo=linkedin)](https://linkedin.com/in/yourprofile)

**London Met ID**: [Your ID]  
**Institution**: Itahari International College

</div>

---

## 📄 License

This project is developed for **educational purposes** as part of university coursework.

---

## 📞 Support

For issues or questions:
- 📧 Contact the module leader
- 📚 Refer to module documentation
- 🐛 [Open an issue](https://github.com/BishanTamang7/InsightJournal_BishanTamang/issues)

---

<div align="center">

### Built with 💙 using C#.NET

**Insight Journal** - *Your thoughts, your journey, your insights.*

⭐ Star this repository if you find it helpful!

</div>
