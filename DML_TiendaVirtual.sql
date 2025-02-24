/****DML****/
INSERT INTO ProductosLibros (Nombre,Descripcion, ISBN, CantidadDisponible, ImagenVinculada, Precio)
VALUES 
('C# Concurrency', 
'C# Concurrency: Asynchronous and multithreaded programming',
'ISBN 978-1633438651',
110, 
'csharp-concurrency-asynchronous-multithreaded-programming.jpg', 
40.00),
('C# 13 Programming Essentials – .NET 9 Edition',
'C# 13 Programming Essentials – .NET 9 Edition: Learn C# and .Net 9 Programming using Visual Studio Code',
'ISBN 978-1965764022',
15, 
'csharp-13-programming-essentials-net-9.jpg', 
38.00),
('Head First C#',
'Head First C#: A Learner’s Guide to Real-World Programming with C# and .NET, 5th Edition',
'ISBN 978-1098141783',
 20,
 'head-first-csharp-5th.jpg', 
 26.00),
('Parallel Programming with C# and .NET',
'Parallel Programming with C# and .NET: Fundamentals of Concurrency and Asynchrony Behind Fast-Paced Applications',
'ISBN 979-8868804878',
 1,
 'parallel-programming-csharp-net-fundamentals.jpg', 
 39.00);

INSERT INTO Pedidos (FechaCompra, PrecioTotal, Usuario)
VALUES
(GETDATE(), 78.00, 'usuario1@test.com'),
(GETDATE(), 26.00, 'usuario1@test.com');

INSERT INTO Ventas (PedidoId, ProductoId, Cantidad, Subtotal)
VALUES 
(1, 1, 1, 40.00), -- Pedido 1: 1 C# Concurrency
(1, 2, 1, 38.00), -- Pedido 1: 1 C# 13 Programming Essentials – .NET 9 Edition
(2, 3, 1, 26.00); -- Pedido 2: 1 Head First C#

INSERT INTO StocksAlerta(ProductoId, ReStock, FechaUltimaAlerta)
VALUES 
(4, 0, GETDATE()); -- El libro de id 4 tiene un solo disponible que es menor de 2 por ende se genera un alerta de stock