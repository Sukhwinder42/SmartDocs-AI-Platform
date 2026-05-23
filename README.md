# 🚀 SmartDocs AI Platform

AI-powered enterprise document intelligence platform built with ASP.NET Core, OCR, Gemini AI, and Retrieval-Augmented Generation (RAG).

🔗 Live Demo: https://smartdocs-ai-herp.onrender.com/

---

## 📌 Overview

SmartDocs AI Platform enables users to upload documents and interact with them using AI.

The system extracts text from PDFs, DOCX files, and images, processes content intelligently, generates summaries, and provides contextual AI-powered document chat.

Built to simulate a real-world AI SaaS product architecture.

---

## ✨ Features

### 🔐 Authentication & Security
- JWT Authentication
- User Registration & Login
- Protected API Endpoints
- Session Management

### 📄 Intelligent Document Processing
- PDF Upload Support
- DOCX Upload Support
- Image Upload Support
- OCR Text Extraction
- PDF Parsing
- Text Cleaning Pipeline
- Document Chunking

### 🤖 AI Capabilities
- AI Summarization
- Chat With Documents
- Contextual Question Answering
- Gemini AI Integration
- Retrieval-Augmented Generation (RAG)
- AI Prompt Orchestration

### 📊 Dashboard & Frontend
- Modern SaaS Dashboard
- Document Analytics
- AI Chat Interface
- Upload Center
- Sidebar Navigation
- Responsive UI

### ⚙️ Backend Architecture
- ASP.NET Core Web API
- ASP.NET MVC Frontend
- Entity Framework Core
- SQL Server
- Repository Pattern
- Background Processing (Hangfire)

---

## 🏗️ System Architecture

```text
User Upload
      ↓
PDF / DOCX / Image
      ↓
OCR / Text Extraction
      ↓
Text Cleaning
      ↓
Chunking Service
      ↓
SQL Server Storage
      ↓
Gemini AI Processing
      ↓
AI Summary Generation
      ↓
Document Chat (RAG)
```

---

## 🛠️ Tech Stack

### Backend
- ASP.NET Core Web API
- ASP.NET MVC
- Entity Framework Core
- SQL Server
- JWT Authentication

### AI & NLP
- Gemini AI API
- OCR Processing
- RAG Architecture
- Text Chunking
- Prompt Engineering

### Document Processing
- UglyToad.PdfPig
- OpenXML SDK
- OCR Services

### Frontend
- Bootstrap
- AdminLTE
- Razor Views

### Background Jobs
- Hangfire

---

## 📷 Screenshots

### Login

<img width="1902" height="907" alt="image" src="https://github.com/user-attachments/assets/1c2673b7-dd32-4fa0-bdda-a12d4000b467" />

### Dashboard

<img width="1906" height="908" alt="image" src="https://github.com/user-attachments/assets/01cbb0fa-315b-4c43-8d23-9446c41b0d01" />

### Upload Center

<img width="1902" height="907" alt="image" src="https://github.com/user-attachments/assets/91877fa7-16c9-43c0-8816-77e1401f5ddf" />

### Documents

<img width="1902" height="905" alt="image" src="https://github.com/user-attachments/assets/7bae3e8a-6f98-4a46-b010-72e5eec67d3b" />

### AI Chat

<img width="1898" height="904" alt="image" src="https://github.com/user-attachments/assets/9f93803f-0b62-4f79-976c-29f3165e7873" />


---

## ⚡ Installation

Clone repository

```bash
git clone https://github.com/Sukhwinder42/SmartDocs-AI-Platform.git
```

Move to project folder

```bash
cd SmartDocs-AI-Platform
```

Update connection string

```json
"ConnectionStrings": {
 "DefaultConnection":
 "Server=.;Database=SmartDocsDB;
 Trusted_Connection=True;
 TrustServerCertificate=True"
}
```

Apply migrations

```bash
Add-Migration InitialCreate
Update-Database
```

Run project

```bash
dotnet run
```

Open:

```
https://localhost:xxxx/swagger
```

---

## 📡 API Modules

### Authentication
```
POST /api/auth/register
POST /api/auth/login
```

### Document APIs
```
POST /api/document/upload
GET /api/document
```

### AI APIs
```
POST /api/ai/summarize/{documentId}
POST /api/ai/ask
```

---

## 🎯 Future Improvements

- Vector Database Integration
- Semantic Search
- Azure Deployment
- Redis Caching
- Multi-document Chat
- Role Based Authorization
- Usage Analytics
- AI Streaming Responses

---

## 🚀 Deployment

Live Application:

https://smartdocs-ai-herp.onrender.com/

---

## 👨‍💻 Author

Sukhwinder Singh

GitHub:
https://github.com/Sukhwinder42

LinkedIn:
https://www.linkedin.com/in/sukhwinder-singh-b26362252/

---
