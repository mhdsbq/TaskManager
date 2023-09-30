-- Users Table
CREATE TABLE Users (
                       UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                       Username TEXT NOT NULL,
                       Email TEXT NOT NULL,
                       Password TEXT NOT NULL
);

-- Tasks Table
CREATE TABLE Tasks (
                       TaskId INTEGER PRIMARY KEY AUTOINCREMENT,
                       UserId INTEGER,
                       Title TEXT NOT NULL,
                       Description TEXT,
                       Created_at DATETIME DEFAULT (datetime('now','localtime')),
                       Completed INTEGER,
                       FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Subtasks Table
CREATE TABLE Subtasks (
                          SubtaskId INTEGER PRIMARY KEY AUTOINCREMENT,
                          TaskId INTEGER,
                          Title TEXT NOT NULL,
                          Completed INTEGER,
                          FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId)
);

-- Sessions Table
CREATE TABLE Sessions (
                          SessionId INTEGER PRIMARY KEY AUTOINCREMENT,
                          TaskId INTEGER,
                          StartTime DATETIME,
                          EndTime DATETIME,
                          Duration INTEGER,
                          FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId)
);
