CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId]    NVARCHAR (150) NOT NULL,
    [ProductVersion] NVARCHAR (32)  NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
);

CREATE TABLE [AspNetRoles] (
    [Id]               NVARCHAR (450) NOT NULL,
    [Name]             NVARCHAR (256) NULL,
    [NormalizedName]   NVARCHAR (256) NULL,
    [ConcurrencyStamp] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [AspNetRoles]([NormalizedName] ASC) WHERE ([NormalizedName] IS NOT NULL);
	
CREATE TABLE [AspNetUsers] (
    [Id]                   NVARCHAR (450)     NOT NULL,
    [UserName]             NVARCHAR (256)     NULL,
    [NormalizedUserName]   NVARCHAR (256)     NULL,
    [Email]                NVARCHAR (256)     NULL,
    [NormalizedEmail]      NVARCHAR (256)     NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]       BIT                NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [EmailIndex]
    ON [AspNetUsers]([NormalizedEmail] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);
	
CREATE TABLE [AspNetRoleClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]     NVARCHAR (450) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId]
    ON [AspNetRoleClaims]([RoleId] ASC);
	


CREATE TABLE [AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (450) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId]
    ON [AspNetUserClaims]([UserId] ASC);
	
CREATE TABLE [AspNetUserLogins] (
    [LoginProvider]       NVARCHAR (128) NOT NULL,
    [ProviderKey]         NVARCHAR (128) NOT NULL,
    [ProviderDisplayName] NVARCHAR (MAX) NULL,
    [UserId]              NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
    ON [AspNetUserLogins]([UserId] ASC);
	
CREATE TABLE [AspNetUserRoles] (
    [UserId] NVARCHAR (450) NOT NULL,
    [RoleId] NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
    ON [AspNetUserRoles]([RoleId] ASC);

	
CREATE TABLE [AspNetUserTokens] (
    [UserId]        NVARCHAR (450) NOT NULL,
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [Name]          NVARCHAR (128) NOT NULL,
    [Value]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Clientes] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]    NVARCHAR (MAX) NOT NULL,
    [Cedula]    NVARCHAR (MAX) NOT NULL,
    [Email]     NVARCHAR (MAX) NOT NULL,
    [Password]  NVARCHAR (MAX) NOT NULL,
    [UsuarioId] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [Proyectos] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]      NVARCHAR (MAX) NOT NULL,
    [Descripcion] NVARCHAR (MAX) NOT NULL,
    [ClienteId]   INT            NULL,
    CONSTRAINT [PK_Proyectos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Proyectos_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Proyectos_ClienteId]
    ON [Proyectos]([ClienteId] ASC);

CREATE TABLE [Tareas] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]        NVARCHAR (MAX) NOT NULL,
    [Funcionalidad] NVARCHAR (MAX) NOT NULL,
    [ClienteId]     INT            NULL,
    [ProyectoId]    INT            NULL,
    CONSTRAINT [PK_Tareas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tareas_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]),
    CONSTRAINT [FK_Tareas_Proyectos_ProyectoId] FOREIGN KEY ([ProyectoId]) REFERENCES [Proyectos] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Tareas_ClienteId]
    ON [Tareas]([ClienteId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tareas_ProyectoId]
    ON [Tareas]([ProyectoId] ASC);

CREATE TABLE [ColaboradoresProyectos] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [ProyectoId]    INT NOT NULL,
    [ColaboradorId] INT NOT NULL,
    CONSTRAINT [PK_ColaboradoresProyectos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ColaboradoresProyectos_Clientes_ColaboradorId] FOREIGN KEY ([ColaboradorId]) REFERENCES [Clientes] ([Id]),
    CONSTRAINT [FK_ColaboradoresProyectos_Proyectos_ProyectoId] FOREIGN KEY ([ProyectoId]) REFERENCES [Proyectos] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ColaboradoresProyectos_ColaboradorId]
    ON [ColaboradoresProyectos]([ColaboradorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ColaboradoresProyectos_ProyectoId]
    ON [ColaboradoresProyectos]([ProyectoId] ASC);
	
CREATE TABLE [TareasProyectos] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [TareaId]    INT NOT NULL,
    [ProyectoId] INT NOT NULL,
	[Estado]	NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_TareasProyectos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TareasProyectos_Proyectos_ProyectoId] FOREIGN KEY ([ProyectoId]) REFERENCES [Proyectos] ([Id]),
    CONSTRAINT [FK_TareasProyectos_Tareas_TareaId] FOREIGN KEY ([TareaId]) REFERENCES [Tareas] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TareasProyectos_ProyectoId]
    ON [TareasProyectos]([ProyectoId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TareasProyectos_TareaId]
    ON [TareasProyectos]([TareaId] ASC);
	
CREATE TABLE [ColaboradoresTareas] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [TareaProyectoId] INT NOT NULL,
    [ColaboradorId]   INT NOT NULL,
    CONSTRAINT [PK_ColaboradoresTareas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ColaboradoresTareas_Clientes_ColaboradorId] FOREIGN KEY ([ColaboradorId]) REFERENCES [Clientes] ([Id]),
    CONSTRAINT [FK_ColaboradoresTareas_TareasProyectos_TareaProyectoId] FOREIGN KEY ([TareaProyectoId]) REFERENCES [TareasProyectos] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ColaboradoresTareas_ColaboradorId]
    ON [ColaboradoresTareas]([ColaboradorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ColaboradoresTareas_TareaProyectoId]
    ON [ColaboradoresTareas]([TareaProyectoId] ASC);

CREATE TABLE [LideresProyectos] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [ProyectoId] INT NOT NULL,
    [LiderId]    INT NOT NULL,
    CONSTRAINT [PK_LideresProyectos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LideresProyectos_Clientes_LiderId] FOREIGN KEY ([LiderId]) REFERENCES [Clientes] ([Id]),
    CONSTRAINT [FK_LideresProyectos_Proyectos_ProyectoId] FOREIGN KEY ([ProyectoId]) REFERENCES [Proyectos] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_LideresProyectos_LiderId]
    ON [LideresProyectos]([LiderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LideresProyectos_ProyectoId]
    ON [LideresProyectos]([ProyectoId] ASC);
	
INSERT INTO "AspNetRoles" ("Id","Name","NormalizedName","ConcurrencyStamp") VALUES 
('1','desarrollador','DESARROLLADOR','desa'),
('2','lider','LIDER','lide');