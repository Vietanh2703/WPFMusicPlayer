﻿CREATE DATABASE MusicPlayerDb
GO

USE MusicPlayerDb
GO

CREATE TABLE UserAccount (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Playlist (
    PlaylistId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    SongName NVARCHAR(255) NOT NULL,
    Url NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES UserAccount(UserId)
);
GO

INSERT INTO UserAccount(Username, Password)
VALUES('Group02','123456');

SELECT * FROM UserAccount