Create Table [dbo].[Admin]
(
    [AdminID] INT IDENTITY(1,1) PRIMARY KEY,
    [AdminEmail] NVARCHAR(50) NOT NULL,
    [AdminPassword] NVARCHAR(50) NOT NULL,
    [AdminName] NVARCHAR(50) NOT NULL,
)

INSERT INTO Admin (AdminEmail, AdminPassword, AdminName)
VALUES ('mr.guard@gmail.com', 'mrguard123', 'Mr.Guard');

Create Table [dbo].[Customer]
(
    [C_ID] INT IDENTITY(1,1) PRIMARY KEY,
    [C_Email] NVARCHAR(50) NOT NULL,
    [C_Password] NVARCHAR(50) NOT NULL,
    [C_Name] NVARCHAR(50),
    [C_Phone] INT NOT NULL,
    [C_Address] NVARCHAR (MAX) NOT NULL,
)


SELECT * FROM Admin
SELECT * FROM Customer


Create Table [dbo].[Category]
(
    [Category_ID] INT PRIMARY KEY,
    [Category_Name] NVARCHAR(50) NOT NULL,

)

Create Table [dbo].[Product]
(  
    [Product_ID] INT PRIMARY KEY,
    [Category_ID] INT NOT NULL FOREIGN KEY REFERENCES Category(Category_ID),
    [Product_Name] NVARCHAR(50) NOT NULL,
    [Price] Float NOT NULL,
    [Quantity] INT NOT NULL,
    [imagePath] NVARCHAR(MAX),

)

ALTER TABLE Product
ADD image NVARCHAR; 

Drop Table Product;
Drop Table Category;

Create Table [dbo].[Orders]
(  
    [Order_ID] INT Identity(1,1) Primary Key,
    [Product_ID] INT NOT NULL FOREIGN KEY REFERENCES Product(Product_ID),
    [C_ID] INT NOT NULL FOREIGN KEY REFERENCES Customer(C_ID),
    [Price] Float NOT NULL,
    [O_Address] NVARCHAR(100) NOT NULL,
    [Payment_Type] NVARCHAR(100) NOT NULL,
    [Order_Quantity] INT NOT NULL,
)

drop table Orders

Create Table [dbo].[Stock]
(  
    [Category_ID] INT NOT NULL FOREIGN KEY REFERENCES Category(Category_ID),
    [Product_ID] INT NOT NULL FOREIGN KEY REFERENCES Product(Product_ID),
    [Stock_Quantity] INT NOT NULL,
)



Create Table [dbo].[Review]
(  
    [R_ID] INT IDENTITY(1,1) PRIMARY KEY,
    [C_ID] INT NOT NULL FOREIGN KEY REFERENCES Customer(C_ID),
    [Product_ID] INT NOT NULL FOREIGN KEY REFERENCES Product(Product_ID),
    [Comment] NVARCHAR(100) NOT NULL,
    [Rating] FLOAT NOT NULL,
)

SELECT * FROM Orders
SELECT * FROM Product
SELECT * FROM Review

DELETE  FROM Product
DELETE  FROM Category


DELETE  FROM Review