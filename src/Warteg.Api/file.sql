CREATE DATABASE Warteg

create Table Categories(
    id int IDENTITY(1,1) PRIMARY key,
    CategoryCode NVARCHAR(50) NOT NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL
)

CREATE TABLE Menu(
    id INT IDENTITY(1,1) PRIMARY KEY,
    MenuCode NVARCHAR(50) not NULL UNIQUE,
    Name NVARCHAR(100) not NULL,
    Price DECIMAL(18,2) not NULL,
    Stock int NOT NULL DEFAULT 0,
    CategoryId int not NULL,

    CONSTRAINT FK_Menu_Categories Foreign Key (CategoryId) REFERENCES Categories(id),
    CONSTRAINT CK_Menu_Price CHECK(Price >= 0),
    CONSTRAINT CK_Menu_Stock CHECK(Stock >= 0)
)