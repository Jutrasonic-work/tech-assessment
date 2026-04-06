CREATE TABLE [dbo].[Sessions] (
    [SessionId]     INT      NOT NULL IDENTITY (1, 1),
    [CourseId]      INT      NOT NULL,
    [StartDate]     DATETIME2 (7) NOT NULL,
    [DeliveryMode]  TINYINT  NOT NULL,
    CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED ([SessionId] ASC),
    CONSTRAINT [FK_Sessions_Courses] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Courses] ([CourseId]) ON DELETE CASCADE,
    CONSTRAINT [CK_Sessions_DeliveryMode] CHECK ([DeliveryMode] IN (0, 1))
);

GO

CREATE NONCLUSTERED INDEX [IX_Sessions_StartDate]
    ON [dbo].[Sessions]([StartDate] ASC);

GO

CREATE NONCLUSTERED INDEX [IX_Sessions_DeliveryMode]
    ON [dbo].[Sessions]([DeliveryMode] ASC);

GO

CREATE NONCLUSTERED INDEX [IX_Sessions_CourseId]
    ON [dbo].[Sessions]([CourseId] ASC);

GO
