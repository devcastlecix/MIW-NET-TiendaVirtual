/****DDL****/
-- La tabla Productos Libros
CREATE TABLE ProductosLibros (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(255) NOT NULL,
	Descripcion NVARCHAR(1000) NULL,
	ISBN NVARCHAR(25) NOT NULL,
    CantidadDisponible INT NOT NULL,
    ImagenVinculada NVARCHAR(500),
    Precio DECIMAL(10,2) NOT NULL,
	CONSTRAINT UQ_ProductosLibros_Nombre UNIQUE (Nombre),
	CONSTRAINT UQ_ProductosLibros_ISBN UNIQUE (ISBN)
);

-- Tabla Pedidos
CREATE TABLE Pedidos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaCompra DATETIME DEFAULT GETDATE() NULL,
    PrecioTotal DECIMAL(10,2) NOT NULL,
    Usuario NVARCHAR(255) NOT NULL
);

-- Tabla intermedia para la relación N:N (muchos productos en una venta)
CREATE TABLE Ventas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL, -- (Cantidad * Precio del producto)
    CONSTRAINT FK_Ventas_Pedidos FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Ventas_ProductosLibros FOREIGN KEY (ProductoId) REFERENCES ProductosLibros(Id) ON DELETE CASCADE
);

-- Crear la tabla StocksAlerta 0:1
CREATE TABLE [dbo].[StocksAlerta] (    
	ProductoId INT NOT NULL PRIMARY KEY,
    ReStock bit NOT NULL,
	FechaUltimaAlerta DATETIME DEFAULT GETDATE() NULL, 	
    CONSTRAINT FK_StocksAlerta_ProductosLibros FOREIGN KEY (ProductoId) 
        REFERENCES ProductosLibros(Id) ON DELETE CASCADE
);
GO