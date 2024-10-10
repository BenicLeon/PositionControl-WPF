
INSERT INTO roles (Id,RoleName,RoleDescription) VALUES (11,'Admin','Adminova rola'),(12,'User','Userova Rola');

INSERT INTO users (Id,FirstName,LastName,Username,PaswordHash,RoleId) VALUES (7,'Leon','Benić','Lbenic','$2y$10$dz6s5bDX/htE68UUhPBC5.6/G0lHpReWtQfx/B0ZnmwJCKfF1PRAC',11),(8,'Marko','Marić','Mmaric','$2y$10$dz6s5bDX/htE68UUhPBC5.6/G0lHpReWtQfx/B0ZnmwJCKfF1PRAC',12);

INSERT INTO workpositions (Id,PositionName,PositionDescription) VALUES (8,'Glavni radnik','Upravljanje korisnicima'),(9,'Obićni radnik','Strojevi');

INSERT INTO userworkpositions (Id,UserId,PositionId,Product,AssignDate) VALUES (1,7,8,'Produkt 1','2024-06-10 00:00:00'),(2,8,9,'Produkt 2','2024-06-10 00:00:00');