USE master
GO

--CREATE DATABASE Configuracao
GO

USE Configuracao
GO

IF OBJECT_ID('Usuario', 'U') IS NULL
CREATE TABLE Usuario
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(150),
	NomeUsuario VARCHAR(50),
	Email VARCHAR(150),
	CPF VARCHAR(15),
	Ativo BIT,
	Senha VARCHAR(50)
)
GO
IF OBJECT_ID('GrupoUsuario', 'U') IS NULL
CREATE TABLE GrupoUsuario
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	NomeGrupo VARCHAR(150)
)
GO

IF OBJECT_ID('Permissao', 'U') IS NULL
CREATE TABLE Permissao
(
	Id INT PRIMARY KEY,
	Descricao VARCHAR(250)
)
GO

IF OBJECT_ID('UsuarioGrupoUsuario', 'U') IS NULL
CREATE TABLE UsuarioGrupoUsuario
(
	IdUsuario INT,
	IdGrupoUsuario INT,
	CONSTRAINT PK_UsuarioGrupoUsuario PRIMARY KEY (IdUsuario, IdGrupoUsuario)
)
GO

IF OBJECT_ID('PermissaoGrupoUsuario', 'U') IS NULL
CREATE TABLE PermissaoGrupoUsuario
(
	IdPermissao INT,
	IdGrupoUsuario INT,
	CONSTRAINT PK_PermissaoGrupoUsuario PRIMARY KEY (IdPermissao, IdGrupoUsuario)
)
GO

IF NOT EXISTS (SELECT 1 FROM SYS.INDEXES WHERE object_id = OBJECT_ID('Usuario') AND IS_PRIMARY_KEY = 1)
ALTER TABLE Usuario ADD CONSTRAINT PK_Usuario PRIMARY KEY (Id)

GO
IF NOT EXISTS (SELECT 1 FROM SYS.INDEXES WHERE object_id = OBJECT_ID('PermissaoGrupoUsuario') AND IS_PRIMARY_KEY = 1)
ALTER TABLE PermissaoGrupoUsuario ADD CONSTRAINT PK_PermissaoGrupoUsuario PRIMARY KEY (IdPermissao, IdGrupoUsuario)

GO

IF NOT EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE PARENT_OBJECT_ID = OBJECT_ID('UsuarioGrupoUsuario') AND name = 'FK_UsuarioGrupoUsuario_Usuario')
ALTER TABLE UsuarioGrupoUsuario
ADD CONSTRAINT FK_UsuarioGrupoUsuario_Usuario
FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id)

GO

IF NOT EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE PARENT_OBJECT_ID = OBJECT_ID('UsuarioGrupoUsuario') AND name = 'FK_UsuarioGrupoUsuario_GrupoUsuario')
ALTER TABLE UsuarioGrupoUsuario
ADD CONSTRAINT FK_UsuarioGrupoUsuario_GrupoUsuario
FOREIGN KEY (IdGrupoUsuario) REFERENCES GrupoUsuario(Id)

GO

IF NOT EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE PARENT_OBJECT_ID = OBJECT_ID('PermissaoGrupoUsuario') AND name = 'FK_PermissaoGrupoUsuario_Permissao')
ALTER TABLE PermissaoGrupoUsuario
ADD CONSTRAINT FK_PermissaoGrupoUsuario_Permissao
FOREIGN KEY (IdPermissao) REFERENCES Permissao(Id)

GO

IF NOT EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE PARENT_OBJECT_ID = OBJECT_ID('PermissaoGrupoUsuario') AND name = 'FK_PermissaoGrupoUsuario_GrupoUsuario')
ALTER TABLE PermissaoGrupoUsuario
ADD CONSTRAINT FK_PermissaoGrupoUsuario_GrupoUsuario
FOREIGN KEY (IdGrupoUsuario) REFERENCES GrupoUsuario(Id)

GO

IF COL_LENGTH('Usuario', 'DataCadastro') IS NULL
ALTER TABLE Usuario ADD DataCadastro DATETIME DEFAULT GETDATE()
GO

DELETE FROM Permissao
GO

INSERT INTO Permissao(Id, Descricao)VALUES(1,'Visualizar usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(2,'Cadastrar usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(3,'Alterar usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(4,'Excluir usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(5,'Visualizar grupo de usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(6,'Cadastrar grupo de usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(7,'Alterar grupo de usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(8,'Excluir grupo de usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(9,'Adicionar permissão a um grupo de usuário')
INSERT INTO Permissao(Id, Descricao)VALUES(10,'Adicionar grupo de usuário a um usuário')
GO

SELECT 
    f.name AS ForeignKey,
    OBJECT_NAME(f.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME(f.referenced_object_id) AS ReferencedTableName,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumnName
FROM 
    sys.foreign_keys AS f
INNER JOIN 
    sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
ORDER BY 
    TableName, ColumnName;
GO