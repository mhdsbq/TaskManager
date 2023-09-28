-- Users Table
CREATE TABLE Users (
                       UserId INT PRIMARY KEY IDENTITY(1,1),
                       Username NVARCHAR(255) NOT NULL,
                       Email NVARCHAR(255) NOT NULL,
                       Password NVARCHAR(255) NOT NULL
);

-- Tasks Table
CREATE TABLE Tasks (
                       TaskId INT PRIMARY KEY IDENTITY(1,1),
                       UserId INT,
                       Title NVARCHAR(255) NOT NULL,
                       Description NVARCHAR(MAX),
                       Created_at DATETIME2 DEFAULT GETDATE(),
                       Completed BIT,
                       FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Subtasks Table
CREATE TABLE Subtasks (
                          SubtaskId INT PRIMARY KEY IDENTITY(1,1),
                          TaskId INT,
                          Title NVARCHAR(255) NOT NULL,
                          Completed BIT,
                          FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId)
);

-- Sessions Table
CREATE TABLE Sessions (
                          Session_id INT PRIMARY KEY IDENTITY(1,1),
                          TaskId INT,
                          StartTime DATETIME2,
                          EndTime DATETIME2,
                          Duration INT,
                          FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId)
);
