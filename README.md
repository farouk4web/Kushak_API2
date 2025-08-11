# Niqash – Social Networking Platform

Niqash is a **social media-style web application** designed to foster interaction and community engagement.  
Users can create posts, comment, react with likes/loves, follow others, customize profiles, and manage account settings.  
An **admin panel** provides full user management capabilities, including viewing all users and removing inappropriate accounts.

---

## 🚀 Features

- **Posts & Interactions**
  - Create and share posts.
  - Comment on posts.
  - React to posts with likes and loves.

- **Social Networking**
  - Follow/unfollow other users.
  - View followers and following lists.

- **Profile Customization**
  - Upload profile pictures.
  - Update personal information.
  - Manage account settings.

- **Admin Panel**
  - View all users.
  - Remove inappropriate accounts.
  - Manage platform content.

---

## 🛠️ Tech Stack

| Technology             | Purpose                                 |
|------------------------|-----------------------------------------|
| ASP.NET MVC            | Core web application framework          |
| Entity Framework       | ORM for database access                 |
| Microsoft SQL Server   | Relational database                     |
| Bootstrap              | Responsive UI design                    |
| jQuery & JavaScript    | Interactive client-side functionality   |
| HTML5 & CSS3           | Markup and styling                      |

---
Niqash/
│── Controllers/ # MVC controllers for handling requests
│── Models/ # Entity and ViewModel classes
│── Views/ # Razor views for the UI
│── Scripts/ # jQuery, JavaScript files
│── Content/ # CSS, Bootstrap styles
│── App_Start/ # Route config, filters, etc.
│── Web.config # Application configuration


---

## ⚙️ Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/farouk4web/Niqash.git

   Open in Visual Studio

Open the .sln file.

Configure the Database

Update the connectionStrings section in Web.config with your SQL Server instance.

Run the initial migrations or SQL scripts to create the database schema.

Build & Run

Press F5 in Visual Studio or run via IIS Express.

🔐 User Roles
Admin
Full control over user management and platform content.

User
Create posts, comment, react, follow others, and customize their profile.

📸 Screenshots
(Add screenshots of your platform here for a better visual overview.)

📜 License
This project is licensed under the MIT License - see the LICENSE file for details.

## 📂 Project Structure

