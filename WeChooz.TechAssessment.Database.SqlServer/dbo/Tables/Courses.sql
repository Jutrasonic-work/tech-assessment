CREATE TABLE [dbo].[Courses] (
    [CourseId]         INT            NOT NULL IDENTITY (1, 1),
    [Name]             NVARCHAR (200) NOT NULL,
    [ShortDescription] NVARCHAR (500) NOT NULL,
    [LongDescription]  NVARCHAR (MAX) NOT NULL,
    [DurationDays]     INT            NOT NULL,
    [CseAudience]      TINYINT        NOT NULL,
    [MaxCapacity]      INT            NOT NULL,
    [TrainerFirstName] NVARCHAR (100) NOT NULL,
    [TrainerLastName]  NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED ([CourseId] ASC),
    CONSTRAINT [CK_Courses_CseAudience] CHECK ([CseAudience] IN (0, 1)),
    CONSTRAINT [CK_Courses_DurationDays] CHECK ([DurationDays] > 0),
    CONSTRAINT [CK_Courses_MaxCapacity] CHECK ([MaxCapacity] > 0)
);

GO

CREATE NONCLUSTERED INDEX [IX_Courses_CseAudience]
    ON [dbo].[Courses]([CseAudience] ASC);

GO
