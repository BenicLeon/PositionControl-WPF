CREATE database positionControl;
use positionControl;

CREATE TABLE roles(
Id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
RoleName VARCHAR(40),
RoleDescription VARCHAR(255)
);

CREATE TABLE users(
Id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
FirstName VARCHAR(40),
LastName VARCHAR(40),
Username VARCHAR(40),
PaswordHash VARCHAR(40),
RoleId INT,
FOREIGN KEY (RoleId) REFERENCES roles(Id)
);

CREATE TABLE work_positions(
Id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
PositionName VARCHAR(40),
PositionDescription VARCHAR(255)
);

CREATE TABLE user_work_positions(
Id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
UserId INT,
PositionId INT,
Product VARCHAR(40),
AssignDate DATETIME,
FOREIGN KEY(UserId) REFERENCES users(Id),
FOREIGN KEY(PositionId) REFERENCES work_positions(Id)
);