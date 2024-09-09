CREATE DATABASE InventoryManagementsDB;

USE InventoryManagementsDB;
GO

CREATE TABLE Inventory (
	ProductID VARCHAR(10) PRIMARY KEY,
	ProductName VARCHAR(20) NOT NULL,
	Category VARCHAR(20) NOT NULL,
	ProductDescription VARCHAR(40),
	Quantity INT NOT NULL
);

INSERT INTO Inventory VALUES 
('DR1001', 'Milk', 'Dairy', 'Locally sourced organic milk', 50),
('FR1001', 'Apples', 'Fruits', 'Locally sourced organic apples', 50),
('VG1001', 'Onions', 'Vegetables', 'Locally sourced organic onions', 50),
('SN1001', 'Pototoe Chips', 'Snacks', 'Locally produced potatoes', 50),
('SW1001', 'Candy Cane', 'Sweets', 'Locally produced candy', 50),
('SN1002', 'Popcorns', 'Snacks', 'Locally produced corn', 50),
('SW1002', 'Marshmallow', 'Sweets', 'Locally produced candy', 50),
('FR1002', 'Naartjies', 'Fruits', 'Locally sourced organic naartjies', 50),
('FR1003', 'Oranges', 'Fruits', 'Locally sourced organic oranges', 50),
('VG1002', 'Tomatoes', 'Vegetables', 'Locally sourced organic tomatoes', 50),
('DR1002', 'Cheese', 'Dairy', 'Locally sourced and produced cheese', 50),
('CN1001', 'Peaches', 'Canned', 'Locally produced canned peaches', 50),
('CN1002', 'Peas', 'Canned', 'Locally produced canned peas', 50),
('CN1003', 'Beans', 'Canned', 'Locally produced canned beans', 50),
('FR1004', 'Banana', 'Fruits', 'Locally sourced organic bananas', 50),
('VG1003', 'Potatoes', 'Vegetables', 'Locally sourced organic potatoes', 50),
('VG1004', 'Carrots', 'Vegetables', 'Locally sourced organic carrots', 50),
('BR1001', 'Bread', 'Bakery', 'Locally produced bread', 50),
('CD1001', 'Coke', 'Cooldrinks', 'Locally produced cooldrinks', 50),
('CD1002', 'Lemonade', 'Cooldrinks', 'Locally produced cooldrinks', 50);

SELECT * FROM Inventory;