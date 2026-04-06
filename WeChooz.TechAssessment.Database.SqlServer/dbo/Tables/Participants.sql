CREATE TABLE [dbo].[Participants] (
    [ParticipantId] INT            NOT NULL IDENTITY (1, 1),
    [SessionId]     INT            NOT NULL,
    [LastName]      NVARCHAR (100) NOT NULL,
    [FirstName]     NVARCHAR (100) NOT NULL,
    [Email]         NVARCHAR (320) NOT NULL,
    [CompanyName]   NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_Participants] PRIMARY KEY CLUSTERED ([ParticipantId] ASC),
    CONSTRAINT [FK_Participants_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[Sessions] ([SessionId]) ON DELETE CASCADE,
    CONSTRAINT [UQ_Participants_Session_Email] UNIQUE NONCLUSTERED ([SessionId] ASC, [Email] ASC)
);

GO

CREATE NONCLUSTERED INDEX [IX_Participants_SessionId]
    ON [dbo].[Participants]([SessionId] ASC);

GO
