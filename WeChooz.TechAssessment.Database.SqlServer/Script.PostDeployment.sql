/*
  Données de démo (idempotent : ne s'exécute que si les tables sont vides).
  CseAudience : 0 = élu au CSE, 1 = président de CSE (aligné sur le domaine C#).
  DeliveryMode : 0 = présentiel, 1 = à distance.
*/
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Courses' AND schema_id = SCHEMA_ID(N'dbo'))
   AND NOT EXISTS (SELECT 1 FROM [dbo].[Courses])
BEGIN
    SET IDENTITY_INSERT [dbo].[Courses] ON;

    INSERT INTO [dbo].[Courses] ([CourseId], [Name], [ShortDescription], [LongDescription], [DurationDays], [CseAudience], [MaxCapacity], [TrainerFirstName], [TrainerLastName])
    VALUES
    (1,
     N'Dialogue social au sein du CSE',
     N'Comprendre le cadre légal et les bonnes pratiques du dialogue avec la direction.',
     N'## Objectifs

- Poser les bases du **dialogue social** au Comité social et économique.
- Clarifier les obligations d''information et de consultation.

## Public

Élus au CSE en activité.

## Méthode

Alternance d''apports, études de cas et quiz.',
     2,
     0,
     20,
     N'Camille',
     N'Bernard'),

    (2,
     N'Le rôle opérationnel du président de CSE',
     N'Organiser le CSE, relater les réunions et coordonner les élus.',
     N'## Objectifs

Renforcer la **fonction de présidence** : préparation, animation et suivi.

## Programme

1. Cadre légal de la présidence
2. Relations avec l''employeur et les expert·e·s
3. Outils pratiques (ordre du jour, compte rendu)

> Session adaptée aux présidents désignés ou en cours de mandat.',
     1,
     1,
     12,
     N'Mehdi',
     N'Alami'),

    (3,
     N'Négociation d''accords d''entreprise',
     N'Méthodes et mise en œuvre d''accords en entreprise.',
     N'## Objectifs

- Structurer une **négociation** (cadrage, phases, clarification des positions).
- Identifier les pièges fréquents et les leviers de consensus.

## Prérequis

Expérience d''élu ou de représentant du personnel.',
     3,
     0,
     16,
     N'Sophie',
     N'Marchal');

    SET IDENTITY_INSERT [dbo].[Courses] OFF;
END
GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Sessions' AND schema_id = SCHEMA_ID(N'dbo'))
   AND EXISTS (SELECT 1 FROM [dbo].[Courses])
   AND NOT EXISTS (SELECT 1 FROM [dbo].[Sessions])
BEGIN
    SET IDENTITY_INSERT [dbo].[Sessions] ON;

    INSERT INTO [dbo].[Sessions] ([SessionId], [CourseId], [StartDate], [DeliveryMode])
    VALUES
    (1, 1, '2026-05-12T08:30:00', 0),
    (2, 1, '2026-06-02T13:00:00', 1),
    (3, 2, '2026-05-20T09:00:00', 1),
    (4, 3, '2026-04-28T08:30:00', 0),
    (5, 1, '2026-07-01T08:30:00', 0),
    (6, 3, '2026-09-15T08:30:00', 1);

    SET IDENTITY_INSERT [dbo].[Sessions] OFF;
END
GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Participants' AND schema_id = SCHEMA_ID(N'dbo'))
   AND EXISTS (SELECT 1 FROM [dbo].[Sessions])
   AND NOT EXISTS (SELECT 1 FROM [dbo].[Participants])
BEGIN
    INSERT INTO [dbo].[Participants] ([SessionId], [LastName], [FirstName], [Email], [CompanyName])
    VALUES
    -- Session 1 (présentiel, cours 1) : 8 inscrits sur 20
    (1, N'Martin', N'Claire', N'claire.martin@atelier-du-nord.example', N'Atelier du Nord SAS'),
    (1, N'Petit', N'Julien', N'julien.petit@lumitech.example', N'Lumitech SA'),
    (1, N'Garcia', N'Inès', N'ines.garcia@logiver.example', N'Logiver'),
    (1, N'Dubois', N'Thomas', N'thomas.dubois@delta-plus.example', N'Delta Plus'),
    (1, N'Lefèvre', N'Amira', N'amira.lefevre@biovert.example', N'Biovert'),
    (1, N'Moreau', N'Lucas', N'lucas.moreau@citemet.example', N'Cité Métropole'),
    (1, N'Roux', N'Sarah', N'sarah.roux@fabrike.example', N'Fabrike Industries'),
    (1, N'Simon', N'Hugo', N'hugo.simon@terreneuve.example', N'Terre Neuve Coop'),

    -- Session 2 (à distance, cours 1) : 5 inscrits
    (2, N'Faure', N'Élodie', N'elodie.faure@orizon.example', N'Orizon Digital'),
    (2, N'Blanc', N'Karim', N'karim.blanc@novalis.example', N'Novalis'),
    (2, N'Girard', N'Maëlys', N'maelys.girard@paysage.example', N'Paysage & Co'),
    (2, N'Henry', N'Nicolas', N'nicolas.henry@urbane.example', N'Urbane Services'),
    (2, N'Perrin', N'Laura', N'laura.perrin@helios.example', N'Hélios Santé'),

    -- Session 3 (présidents, à distance)
    (3, N'Chevallier', N'Patrice', N'patrice.chevallier@maritime.example', N'Industries Maritimes SA'),
    (3, N'Ben Amar', N'Samira', N'samira.benamar@cedre.example', N'Cèdre Holding'),

    -- Session 4 (négociation présentiel) : 14 inscrits sur 16
    (4, N'Durand', N'Étienne', N'etienne.durand@granit.example', N'Granit BTP'),
    (4, N'Nguyen', N'Linh', N'linh.nguyen@streamfield.example', N'Streamfield'),
    (4, N'Lopez', N'Anaïs', N'anais.lopez@ceres.example', N'Cérès Agro'),
    (4, N'Kowalski', N'Marek', N'marek.kowalski@nordik.example', N'Nordik Logistics'),
    (4, N'Fontaine', N'Chloé', N'chloe.fontaine@avenir.example', N'Avenir Mutuelle'),
    (4, N'Bakir', N'Youssef', N'youssef.bakir@alphalab.example', N'Alpha Lab'),
    (4, N'Renard', N'Océane', N'oceane.renard@lumius.example', N'Lumius Énergies'),
    (4, N'Costa', N'Bruno', N'bruno.costa@valora.example', N'Valora Retail'),
    (4, N'Schmitt', N'Julie', N'julie.schmitt@mediance.example', N'Médiance'),
    (4, N'Obert', N'David', N'david.obert@solidar.example', N'Solidar Pro'),
    (4, N'Ibrahim', N'Yasmin', N'yasmin.ibrahim@proxitec.example', N'Proxitec'),
    (4, N'Gomes', N'Rafael', N'rafael.gomes@horizon.example', N'Horizon Vert'),
    (4, N'Tanguy', N'Pauline', N'pauline.tanguy@civitas.example', N'Civitas'),
    (4, N'Arnaud', N'Maxime', N'maxime.arnaud@neostream.example', N'Neostream'),

    -- Session 5 : aucun participant (places disponibles)

    -- Session 6 (futur, à distance) : 2 inscrits
    (6, N'Velay', N'Christine', N'christine.velay@aster.example', N'Aster Conseil'),
    (6, N'Mensah', N'Kwame', N'kwame.mensah@junction.example', N'Junction IT');
END
GO
