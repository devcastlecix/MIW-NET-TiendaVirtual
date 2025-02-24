
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/20/2025 16:46:29
-- Generated from EDMX file: D:\Academico\Maestria\Espania\UPM\MiW\Asignaturas\NET\Source\Practica\MIW-NET-TiendaVirtual\TiendaVirtual\Models\ModeloTiendaVirtual.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
--USE [TiendaVirtualDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ProductosLibros'
CREATE TABLE [dbo].[ProductosLibros] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(255)  NOT NULL,
    [Descripcion] nvarchar(1000)  NULL,
    [ISBN] nvarchar(25)  NOT NULL,
    [CantidadDisponible] int  NOT NULL,
    [ImagenVinculada] nvarchar(500)  NOT NULL,
    [Precio] decimal(10,2)  NOT NULL
);
GO

-- Creating table 'Pedidos'
CREATE TABLE [dbo].[Pedidos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaCompra] datetime  NULL,
    [PrecioTotal] decimal(10,2)  NOT NULL,
    [Usuario] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'Ventas'
CREATE TABLE [dbo].[Ventas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PedidoId] int  NOT NULL,
    [ProductoId] int  NOT NULL,
    [Cantidad] int  NOT NULL,
    [Subtotal] decimal(10,2)  NOT NULL
);
GO

-- Creating table 'StocksAlerta'
CREATE TABLE [dbo].[StocksAlerta] (
    [ProductoId] int  NOT NULL,
    [ReStock] bit  NOT NULL,
    [FechaUltimaAlerta] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ProductosLibros'
ALTER TABLE [dbo].[ProductosLibros]
ADD CONSTRAINT [PK_ProductosLibros]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pedidos'
ALTER TABLE [dbo].[Pedidos]
ADD CONSTRAINT [PK_Pedidos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ventas'
ALTER TABLE [dbo].[Ventas]
ADD CONSTRAINT [PK_Ventas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ProductoId] in table 'StocksAlerta'
ALTER TABLE [dbo].[StocksAlerta]
ADD CONSTRAINT [PK_StocksAlerta]
    PRIMARY KEY CLUSTERED ([ProductoId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ProductoId] in table 'Ventas'
ALTER TABLE [dbo].[Ventas]
ADD CONSTRAINT [FK_Ventas_ProductosLibros]
    FOREIGN KEY ([ProductoId])
    REFERENCES [dbo].[ProductosLibros]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ventas_ProductosLibros'
CREATE INDEX [IX_FK_Ventas_ProductosLibros]
ON [dbo].[Ventas]
    ([ProductoId]);
GO

-- Creating foreign key on [PedidoId] in table 'Ventas'
ALTER TABLE [dbo].[Ventas]
ADD CONSTRAINT [FK_Ventas_Pedidos]
    FOREIGN KEY ([PedidoId])
    REFERENCES [dbo].[Pedidos]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ventas_Pedidos'
CREATE INDEX [IX_FK_Ventas_Pedidos]
ON [dbo].[Ventas]
    ([PedidoId]);
GO

-- Creating foreign key on [ProductoId] in table 'StocksAlerta'
ALTER TABLE [dbo].[StocksAlerta]
ADD CONSTRAINT [FK_StocksAlerta_ProductosLibros]
    FOREIGN KEY ([ProductoId])
    REFERENCES [dbo].[ProductosLibros]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------